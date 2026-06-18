using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneDirector : MonoBehaviour
{
    private GameObject player;
    public static bool isPaused = false;

    /// ゲーム本編を開始する
    /// 副作用：タイムスケールを通常に戻し、Loadingシーンをロードする。カーソルを非表示にしてロックする。
    public void Play()
    {
        // 停止状態から復帰させるためタイムスケールを初期化する
        Time.timeScale = 1f;
        SceneManager.LoadScene("Loading");

        // プレイヤーの視点操作を妨げないためカーソルを隠して固定する
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// チュートリアルを開始する
    /// 副作用：タイムスケールを通常に戻し、Tutorialシーンをロードする。カーソルを非表示にしてロックする。
    public void Tutorial()
    {
        // 停止状態から復帰させるためタイムスケールを初期化する
        Time.timeScale = 1f;
        SceneManager.LoadScene("Tutorial");

        // プレイヤーの視点操作を妨げないためカーソルを隠して固定する
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// 設定画面を開く
    /// 副作用：Settingsシーンをロードする。カーソルを表示してウィンドウ内に制限する。
    public void Settings()
    {
        SceneManager.LoadScene("Settings");

        // UI操作を可能にするためカーソルを表示状態にする
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// アプリケーションを終了する
    /// 副作用：エディタのプレイモードまたはビルドされたアプリを終了する。
    public void Exit()
    {
        // エディタ上でも終了動作を確認できるようにする
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // ビルド版でのゲーム終了処理を実行する
        Application.Quit();
    }

    /// 現在のマップを再読み込みしてリスタートする
    /// 副作用：タイムスケールを初期化し、保存されたマップシーンをロードする。
    public void Restart()
    {
        // 停止状態から復帰させるためタイムスケールを初期化する
        Time.timeScale = 1f;
        // プレイヤーが最後に選択したマップを復元するためPlayerPrefsを使用する
        SceneManager.LoadScene(PlayerPrefs.GetString("SelectedMap", "GameScene"));

        // プレイヤーの視点操作を妨げないためカーソルを隠して固定する
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// メインメニューに戻る
    /// 副作用：タイムスケールを初期化し、MainMenuシーンをロードする。カーソルを表示する。
    public void Menu()
    {
        // 停止状態から復帰させるためタイムスケールを初期化する
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

        // UI操作を可能にするためカーソルを表示状態にする
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// キー割り当て設定画面を開く
    /// 副作用：KeySettingsシーンをロードし、カーソルを表示する。
    public void KeySettings()
    {
        SceneManager.LoadScene("KeySettings");

        // UI操作を可能にするためカーソルを表示状態にする
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// 情報・クレジット画面を開く
    /// 副作用：Infoシーンをロードし、カーソルを表示する。
    public void Info()
    {
        SceneManager.LoadScene("Info");

        // UI操作を可能にするためカーソルを表示状態にする
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
