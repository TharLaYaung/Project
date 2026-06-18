using UnityEngine;
using UnityEditor;

namespace DeveloperTools
{
    public static class EnableReadWriteUtility
    {
        // 入力:Unityエディタ上でのアセット選択状態 / 出力:なし / 副作用:アセットのインポート設定上書きと再インポート
        // 実行時のメッシュ結合や動的コライダー生成に必要な「Read/Write」設定を複数モデルへ一括適用する
        [MenuItem("Tools/Mesh Utilities/Enable Read-Write on Selected")]
        public static void EnableReadWriteOnSelected()
        {
            // フォルダ選択時も考慮し、DeepAssetsで配下の全アセットを再帰的に取得する
            Object[] selectedObjects = Selection.GetFiltered<Object>(SelectionMode.DeepAssets);
            int modifiedCount = 0;

            foreach (Object obj in selectedObjects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path)) continue;

                ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;

                // 無駄な再インポートによるエディタのフリーズ（パフォーマンス低下）を防ぐため、未設定のモデルのみ対象とする
                if (importer != null && !importer.isReadable)
                {
                    importer.isReadable = true;
                    importer.SaveAndReimport();
                    modifiedCount++;
                    Debug.Log($"Enabled Read/Write on model: {path}");
                }
            }

            // 処理完了をユーザーへ明確に通知し、期待した数のモデルが処理されたか確認させる
            EditorUtility.DisplayDialog(
                "Mesh Read/Write Utility",
                $"Process complete!\nEnabled Read/Write on {modifiedCount} models.",
                "OK"
            );
        }
    }
}