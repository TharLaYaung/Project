using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;


/// ゲームのポーズ（一時停止）メニューを管理するスクリプト
/// 時間の停止、UIの表示非表示、カーソルの制御などを行います

public class PauseMenu : MonoBehaviour
{
    // --- 公開変数（インスペクターで設定） ---

    // メインメニューのシーン名
    public string mainMenuName = "MainMenu";

    // 現在ポーズ中かどうかのフラグ
    public bool isPaused;

    // ポーズ時に表示する画面パネル
    public GameObject pauseScreen;

    // 照準（クロスヘア）UI
    public GameObject crosshair;

    // ヒットマークUI
    public GameObject hitmark;

    // シングルトン（他スクリプトから PauseMenu.instance でアクセス可能にする）
    public static PauseMenu instance;

  
    /// スクリプトのインスタンスが読み込まれた際の処理
    
    private void Awake()
    {
        
        instance = this;
    }

   

   
    /// 開始処理
    
    public void Start()
    {
        // カーソルをゲームウィンドウ内に制限（Confined）する
        Cursor.lockState = CursorLockMode.Confined;
    }

    
    /// 毎フレームの更新処理
    
    public void Update()
    {
        // 「ESC」キーが押されたらポーズ状態を切り替える
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnPause();
        }
    }

    
    /// ポーズ状態の反転（実行・解除）を制御するメインロジック
    
    public void PauseUnPause()
    {
        if (isPaused)
        {
            // --- ポーズ解除（再開）時の処理 ---

            // カーソルを非表示にする
            Cursor.visible = false;
            // フラグを解除
            isPaused = false;
            // ポーズ画面を消す
            pauseScreen.SetActive(false);
            // 時間の流れを通常に戻す
            Time.timeScale = 1f;
            // ゲーム中用のUI（照準など）を再表示
            // Null参照エラーを回避するためのチェックを追加
            if (crosshair != null) crosshair.SetActive(true);
            if (hitmark != null) hitmark.SetActive(true);
        }
        else
        {
            // --- ポーズ実行時の処理 ---

            // カーソルを表示し、ウィンドウ内に制限
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // フラグを立てる
            isPaused = true;
            // ポーズ画面を表示
            pauseScreen.SetActive(true);
            // 時間の流れを完全に停止
            Time.timeScale = 0f;
            // ゲーム中用のUIを隠す
            // Null参照エラーを回避するためのチェックを追加
            if (crosshair != null) crosshair.SetActive(false);
            if (hitmark != null) hitmark.SetActive(false);

           
        }
    }

    
   

   
    /// ゲームを終了する処理
    
    public void Exit()
    {
        // ポーズを解除して時間を通常に戻す
        Time.timeScale = 1f;

        // main menu scene が設定されていればタイトルに戻る
        if (!string.IsNullOrEmpty(mainMenuName))
        {
            SceneManager.LoadScene(mainMenuName);
        }
        else
        {
            // Unityエディタ上での再生を停止する（条件付きコンパイル）
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            // ビルドされた実際のアプリを終了する
            Application.Quit();
        }
    }
}