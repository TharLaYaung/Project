using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu
{
    public class UIMenuManager : MonoBehaviour
    {
        private Animator CameraObject;

        [Header("メニュー")]
        [Tooltip("メインメニューボタン用のメニュー")]
        public GameObject mainMenu;
        [Tooltip("最初のボタンリスト")]
        public GameObject firstMenu;
        [Tooltip("PLAYボタンがクリックされた時のメニュー")]
        public GameObject playMenu;
        [Tooltip("EXITボタンがクリックされた時のメニュー")]
        public GameObject exitMenu;
        [Tooltip("オプションの第4メニュー")]
        public GameObject extrasMenu;

        public enum Theme { custom1, custom2, custom3 };
        [Header("テーマ設定")]
        public Theme theme;
        
        // private int themeIndex; 
        public ThemedUIData themeController;

        [Header("パネル")]
        [Tooltip("すべてのサブメニューを親としてまとめるUIパネル")]
        public GameObject mainCanvas;
        [Tooltip("CONTROLS（操作）ウィンドウタブを保持するUIパネル")]
        public GameObject PanelControls;
        [Tooltip("VIDEO（ビデオ）ウィンドウタブを保持するUIパネル")]
        public GameObject PanelVideo;
        [Tooltip("GAME（ゲーム）ウィンドウタブを保持するUIパネル")]
        public GameObject PanelGame;
        [Tooltip("KEY BINDINGS（キー割り当て）ウィンドウタブを保持するUIパネル")]
        public GameObject PanelKeyBindings;
        [Tooltip("KEY BINDINGS内のMOVEMENT（移動）用UIサブパネル")]
        public GameObject PanelMovement;
        [Tooltip("KEY BINDINGS内のCOMBAT（戦闘）用UIサブパネル")]
        public GameObject PanelCombat;
        [Tooltip("KEY BINDINGS内のGENERAL（一般）用UIサブパネル")]
        public GameObject PanelGeneral;

        [Header("設定画面")]
        [Tooltip("設定でGAMEタブが選択された時のハイライト画像")]
        public GameObject lineGame;
        [Tooltip("設定でVIDEOタブが選択された時のハイライト画像")]
        public GameObject lineVideo;
        [Tooltip("設定でCONTROLSタブが選択された時のハイライト画像")]
        public GameObject lineControls;
        [Tooltip("設定でKEY BINDINGSタブが選択された時のハイライト画像")]
        public GameObject lineKeyBindings;
        [Tooltip("KEY BINDINGSでMOVEMENTサブタブが選択された時のハイライト画像")]
        public GameObject lineMovement;
        [Tooltip("KEY BINDINGSでCOMBATサブタブが選択された時のハイライト画像")]
        public GameObject lineCombat;
        [Tooltip("KEY BINDINGSでGENERALサブタブが選択された時のハイライト画像")]
        public GameObject lineGeneral;

        [Header("ロード画面")]
        [Tooltip("trueの場合、ユーザーの入力があるまでロードされたシーンは起動しません")]
        public bool waitForInput = true;
        public GameObject loadingMenu;
        [Tooltip("ロード画面内のロードバースライダーUI要素")]
        public Slider loadingBar;
        public TMP_Text loadPromptText;
        public KeyCode userPromptKey;

        [Header("効果音 (SFX)")]
        [Tooltip("ホバー音(HOVER SOUND)のAudio Sourceコンポーネントを保持するGameObject")]
        public AudioSource hoverSound;
        [Tooltip("オーディオスライダー音のAudio Sourceコンポーネントを保持するGameObject")]
        public AudioSource sliderSound;
        [Tooltip("設定画面切り替え時のスワイプ音(SWOOSH SOUND)のAudio Sourceコンポーネントを保持するGameObject")]
        public AudioSource swooshSound;

        // 入力:なし / 出力:なし / 副作用:各メニューの初期表示状態を設定
        // シーン開始時のUI状態を初期状態にリセットする
        void Start()
        {
            CameraObject = transform.GetComponent<Animator>();

            playMenu.SetActive(false);
            exitMenu.SetActive(false);
            if (extrasMenu) extrasMenu.SetActive(false);
            firstMenu.SetActive(true);
            mainMenu.SetActive(true);

            SetThemeColors();
        }

        // 入力:なし / 出力:なし / 副作用:UIの色定義を更新
        // 選択されたテーマに応じてUIコンポーネントの色を一括適用する
        void SetThemeColors()
        {
            switch (theme)
            {
                case Theme.custom1:
                    themeController.currentColor = themeController.custom1.graphic1;
                    themeController.textColor = themeController.custom1.text1;
                    break;
                case Theme.custom2:
                    themeController.currentColor = themeController.custom2.graphic2;
                    themeController.textColor = themeController.custom2.text2;
                    break;
                case Theme.custom3:
                    themeController.currentColor = themeController.custom3.graphic3;
                    themeController.textColor = themeController.custom3.text3;
                    break;
                default:
                    Debug.Log("無効なテーマが選択されました。");
                    break;
            }
        }

        // 入力:なし / 出力:なし / 副作用:プレイメニューを表示
        // 既存メニューを非表示にし、ゲーム開始用メニューへ遷移する
        public void PlayCampaign()
        {
            exitMenu.SetActive(false);
            if (extrasMenu) extrasMenu.SetActive(false);
            playMenu.SetActive(true);
        }

        // 入力:なし / 出力:なし / 副作用:プレイメニューを表示、メインメニュー非表示
        // モバイル端末の画面サイズ制約を考慮し、背面のメインメニューも消去する
        public void PlayCampaignMobile()
        {
            exitMenu.SetActive(false);
            if (extrasMenu) extrasMenu.SetActive(false);
            playMenu.SetActive(true);
            mainMenu.SetActive(false);
        }

        // 入力:なし / 出力:なし / 副作用:メインメニューを再表示
        public void ReturnMenu()
        {
            playMenu.SetActive(false);
            if (extrasMenu) extrasMenu.SetActive(false);
            exitMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        // 入力:遷移先シーン名 / 出力:なし / 副作用:非同期ロード開始
        // 空文字によるロード例外を防ぐため、シーン名がある場合のみ実行
        public void LoadScene(string scene)
        {
            if (scene != "")
            {
                StartCoroutine(LoadAsynchronously(scene));
            }
        }

        // 入力:なし / 出力:なし / 副作用:プレイメニューを非表示
        public void DisablePlayCampaign()
        {
            playMenu.SetActive(false);
        }

        // 入力:なし / 出力:なし / 副作用:カメラアニメーション状態を変更
        // メニュー遷移時のカメラ移動をトリガーする
        public void Position2()
        {
            DisablePlayCampaign();
            CameraObject.SetFloat("Animate", 1);
        }

        // 入力:なし / 出力:なし / 副作用:カメラアニメーション状態を初期化
        public void Position1()
        {
            CameraObject.SetFloat("Animate", 0);
        }

        // 入力:なし / 出力:なし / 副作用:全ての設定パネルとハイライトを非表示
        // 他のタブ選択時に排他表示を保証するためのリセット処理
        void DisablePanels()
        {
            PanelControls.SetActive(false);
            PanelVideo.SetActive(false);
            PanelGame.SetActive(false);
            PanelKeyBindings.SetActive(false);

            lineGame.SetActive(false);
            lineControls.SetActive(false);
            lineVideo.SetActive(false);
            lineKeyBindings.SetActive(false);

            PanelMovement.SetActive(false);
            lineMovement.SetActive(false);
            PanelCombat.SetActive(false);
            lineCombat.SetActive(false);
            PanelGeneral.SetActive(false);
            lineGeneral.SetActive(false);
        }

        // 入力:なし / 出力:なし / 副作用:対象パネルとハイライトのみ表示
        // UIの排他制御による単一タブ表示を実現する
        public void GamePanel()
        {
            DisablePanels();
            PanelGame.SetActive(true);
            lineGame.SetActive(true);
        }

        public void VideoPanel()
        {
            DisablePanels();
            PanelVideo.SetActive(true);
            lineVideo.SetActive(true);
        }

        public void ControlsPanel()
        {
            DisablePanels();
            PanelControls.SetActive(true);
            lineControls.SetActive(true);
        }

        public void KeyBindingsPanel()
        {
            DisablePanels();
            MovementPanel();
            PanelKeyBindings.SetActive(true);
            lineKeyBindings.SetActive(true);
        }

        public void MovementPanel()
        {
            DisablePanels();
            PanelKeyBindings.SetActive(true);
            PanelMovement.SetActive(true);
            lineMovement.SetActive(true);
        }

        public void CombatPanel()
        {
            DisablePanels();
            PanelKeyBindings.SetActive(true);
            PanelCombat.SetActive(true);
            lineCombat.SetActive(true);
        }

        public void GeneralPanel()
        {
            DisablePanels();
            PanelKeyBindings.SetActive(true);
            PanelGeneral.SetActive(true);
            lineGeneral.SetActive(true);
        }

        // 入力:なし / 出力:なし / 副作用:ホバー音等の再生
        // ユーザーの入力操作に対する聴覚的なフィードバック
        public void PlayHover()
        {
            hoverSound.Play();
        }

        public void PlaySFXHover()
        {
            sliderSound.Play();
        }

        public void PlaySwoosh()
        {
            swooshSound.Play();
        }

        // 入力:なし / 出力:なし / 副作用:終了確認メニューを表示
        // 誤操作によるゲーム終了を防止するため確認フローを挟む
        public void AreYouSure()
        {
            // Fix: Directly quit the game because the exit confirmation UI seems to be broken
            QuitGame();
        }

        // 入力:なし / 出力:なし / 副作用:メインメニュー非表示、終了確認メニュー表示
        // モバイル環境で画面占有を避けるため背面メニューを消去する
        public void AreYouSureMobile()
        {
            // Fix: Directly quit the game
            QuitGame();
        }

        // 入力:なし / 出力:なし / 副作用:追加メニューを表示
        public void ExtrasMenu()
        {
            playMenu.SetActive(false);
            if (extrasMenu) extrasMenu.SetActive(true);
            exitMenu.SetActive(false);
        }

        // 入力:なし / 出力:なし / 副作用:アプリケーションの終了処理
        // 開発時の利便性確保のため、エディタ実行時のみPlayモードを解除する
        public void QuitGame()
        {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // 入力:ロード対象シーン名 / 出力:IEnumerator / 副作用:シーン非同期ロード
        // 意図しないタイミングでの遷移を防ぐため、90%時点でユーザー入力を待機する
        IEnumerator LoadAsynchronously(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            mainCanvas.SetActive(false);
            loadingMenu.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .95f);
                loadingBar.value = progress;

                if (operation.progress >= 0.9f && waitForInput)
                {
                    loadPromptText.text = "Press " + userPromptKey.ToString().ToUpper() + " to continue";
                    loadingBar.value = 1;

                    if (Input.GetKeyDown(userPromptKey))
                    {
                        operation.allowSceneActivation = true;
                    }
                }
                else if (operation.progress >= 0.9f && !waitForInput)
                {
                    operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}