using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class UnusedAssetsFinder : EditorWindow
{
    private List<string> unusedAssets = new List<string>();
    private Vector2 scrollPosition;

    private bool scanAndCleanUnusedFonts = true;
    private bool excludeShadersAndIncludes = true;
    private bool excludeTextAndConfigs = true;
    private bool excludeSpritesAndUI = true;

    // Input: なし / Output: なし / 副作用: エディタウィンドウのインスタンス化と表示
    // 開発者が任意のタイミングでアセット整理機能にアクセスするための導線確保
    [MenuItem("Tools/Find Unused Assets")]
    public static void ShowWindow()
    {
        GetWindow<UnusedAssetsFinder>("Unused Assets Finder");
    }

    // Input: なし / Output: なし / 副作用: GUIの描画、アセットの削除操作の実行
    // ツールのUI表示および、ユーザーアクションに応じた処理の分岐を管理する
    private void OnGUI()
    {
        GUILayout.Label("Find Unused Assets in Project", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Scan Safety Settings", EditorStyles.miniBoldLabel);

        scanAndCleanUnusedFonts = EditorGUILayout.ToggleLeft("Scan & Clean Unused Fonts (Keeps ONLY the active font/fallbacks)", scanAndCleanUnusedFonts);
        excludeShadersAndIncludes = EditorGUILayout.ToggleLeft("Protect Shaders & Shader Libraries (.shader, .cginc)", excludeShadersAndIncludes);
        excludeTextAndConfigs = EditorGUILayout.ToggleLeft("Protect Text & Localization Files (.txt, .json, .csv)", excludeTextAndConfigs);
        excludeSpritesAndUI = EditorGUILayout.ToggleLeft("Protect Sprites, Icons & UI Assets (.spriteatlas, UI folders)", excludeSpritesAndUI);

        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        if (GUILayout.Button("Scan Project (Based on Open Scenes)", GUILayout.Height(30)))
        {
            ScanForUnusedAssets();
        }

        if (unusedAssets.Count > 0)
        {
            EditorGUILayout.HelpBox($"{unusedAssets.Count} unused assets found. Note: Assets in 'Resources' or 'StreamingAssets' folders are excluded as they are always loaded dynamically.", MessageType.Info);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear List", GUILayout.Width(100)))
            {
                unusedAssets.Clear();
            }

            GUI.color = Color.red;
            if (GUILayout.Button("Delete All Listed", GUILayout.Width(140)))
            {
                // 一括削除によるプロジェクト破壊を防ぐため、実行前に必ず最終確認を行う
                if (EditorUtility.DisplayDialog("Delete All Assets?", $"Are you sure you want to permanently delete all {unusedAssets.Count} unused assets? This operation cannot be undone!", "Yes, Delete All", "Cancel"))
                {
                    foreach (string assetPath in unusedAssets)
                    {
                        AssetDatabase.DeleteAsset(assetPath);
                    }
                    unusedAssets.Clear();
                    AssetDatabase.Refresh();
                }
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < unusedAssets.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(unusedAssets[i], EditorStyles.miniLabel);

                if (GUILayout.Button("Ping", GUILayout.Width(50)))
                {
                    var obj = AssetDatabase.LoadAssetAtPath<Object>(unusedAssets[i]);
                    EditorGUIUtility.PingObject(obj);
                }

                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    // 個別削除時も誤操作によるファイル消失を防ぐためダイアログで保護
                    if (EditorUtility.DisplayDialog("Delete Asset?", $"Are you sure you want to delete {Path.GetFileName(unusedAssets[i])}?", "Yes", "No"))
                    {
                        AssetDatabase.DeleteAsset(unusedAssets[i]);
                        unusedAssets.RemoveAt(i);
                        AssetDatabase.Refresh();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
            GUILayout.Label("No active scan results. Click 'Scan Project' to start.", EditorStyles.wordWrappedLabel);
        }
    }

    // Input: なし / Output: なし / 副作用: unusedAssetsリストの更新
    // 全アセットと使用中アセットの差分を計算し、安全に削除可能な候補を特定する
    private void ScanForUnusedAssets()
    {
        unusedAssets.Clear();

        // 実行時エラーやコンパイルエラーを防ぐため、スクリプトや動的読み込み対象フォルダはスキャンから除外
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths()
            .Where(path => path.StartsWith("Assets/")
                           && !Directory.Exists(path)
                           && !path.Contains("/Editor/")
                           && !path.Contains("/Resources/")
                           && !path.Contains("/StreamingAssets/")
                           && !path.EndsWith(".cs")
                           && !path.EndsWith(".unity"))
            .ToArray();

        string[] activeScenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        // Build Settingsが未設定の環境でも動作を担保するため、現在のアクティブシーンを代替使用する
        if (activeScenes.Length == 0)
        {
            activeScenes = new string[] { UnityEngine.SceneManagement.SceneManager.GetActiveScene().path };
        }

        string[] dependencies = AssetDatabase.GetDependencies(activeScenes, true);
        HashSet<string> usedAssetsHash = new HashSet<string>(dependencies);

        ProtectActiveFonts(usedAssetsHash);

        foreach (string assetPath in allAssetPaths)
        {
            if (ShouldProtectAsset(assetPath, usedAssetsHash))
            {
                continue;
            }

            if (!usedAssetsHash.Contains(assetPath))
            {
                unusedAssets.Add(assetPath);
            }
        }
    }

    // Input: usedAssetsHash (使用中アセットのHashSet) / Output: なし / 副作用: 引数のHashSetにフォント関連パスを追加
    // 標準の依存関係解析では漏れやすいTMProの動的参照ファイル（マテリアル、フォールバック等）を保護する
    private void ProtectActiveFonts(HashSet<string> usedAssetsHash)
    {
        TMP_Text[] activeTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();
        foreach (var text in activeTexts)
        {
            if (text.gameObject.scene == SceneManager.GetActiveScene() && text.font != null)
            {
                string fontPath = AssetDatabase.GetAssetPath(text.font);
                if (!string.IsNullOrEmpty(fontPath))
                {
                    usedAssetsHash.Add(fontPath);

                    if (text.font.material != null)
                    {
                        string matPath = AssetDatabase.GetAssetPath(text.font.material);
                        if (!string.IsNullOrEmpty(matPath)) usedAssetsHash.Add(matPath);
                    }
                    if (text.font.atlasTexture != null)
                    {
                        string texPath = AssetDatabase.GetAssetPath(text.font.atlasTexture);
                        if (!string.IsNullOrEmpty(texPath)) usedAssetsHash.Add(texPath);
                    }
                }

                if (text.font.fallbackFontAssetTable != null)
                {
                    foreach (var fallback in text.font.fallbackFontAssetTable)
                    {
                        if (fallback != null)
                        {
                            string fallbackPath = AssetDatabase.GetAssetPath(fallback);
                            if (!string.IsNullOrEmpty(fallbackPath))
                            {
                                usedAssetsHash.Add(fallbackPath);
                                if (fallback.atlasTexture != null)
                                {
                                    string fallbackTex = AssetDatabase.GetAssetPath(fallback.atlasTexture);
                                    if (!string.IsNullOrEmpty(fallbackTex)) usedAssetsHash.Add(fallbackTex);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Input: path (評価対象のパス), usedAssetsHash / Output: 保護対象か否かのbool値 / 副作用: なし
    // ユーザー設定および拡張子情報から、動的ロードされるリスクが高いアセットを除外判定する
    private bool ShouldProtectAsset(string path, HashSet<string> usedAssetsHash)
    {
        string ext = Path.GetExtension(path).ToLower();

        // TMProの基幹設定やシェーダーが削除されるとUI描画が完全に破損するため、強制的に保護する
        if (path.Contains("TMP Settings") || path.Contains("TextMesh Pro/Resources") || path.Contains("TextMesh Pro/Shaders"))
        {
            return true;
        }

        if (!scanAndCleanUnusedFonts)
        {
            // フォントの未使用スキャンが無効化されている場合、種類を問わずすべてのフォントファイルを一律保護
            if (ext == ".ttf" || ext == ".otf" || ext == ".woff" || ext == ".dfont" || ext == ".ttc" || ext == ".fontsettings")
            {
                return true;
            }
            if (path.Contains("TextMesh Pro") || path.Contains("TMPro"))
            {
                return true;
            }
        }
        else
        {
            if (usedAssetsHash.Contains(path))
            {
                return true;
            }
        }

        if (excludeShadersAndIncludes)
        {
            if (ext == ".shader" || ext == ".cginc" || ext == ".hlsl" || ext == ".glslinc" || ext == ".compute" || ext == ".shadervariants" || ext == ".cg")
            {
                return true;
            }
        }

        if (excludeTextAndConfigs)
        {
            if (ext == ".txt" || ext == ".json" || ext == ".xml" || ext == ".csv" || ext == ".yaml" || ext == ".ini")
            {
                return true;
            }
        }

        if (excludeSpritesAndUI)
        {
            if (ext == ".spriteatlas")
            {
                return true;
            }

            string lowerPath = path.ToLower();
            if (lowerPath.Contains("sprite") || lowerPath.Contains("icon") || lowerPath.Contains("/ui/") || lowerPath.Contains("/textures/ui"))
            {
                return true;
            }
        }

        return false;
    }
}