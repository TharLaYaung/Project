using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// シーン遷移時のローディング画面と進捗バーを制御するクラス

public class Loading : MonoBehaviour
{
    [Header("UI設定")]
    public GameObject loading;          // ローディング画面全体のパネル
    public Slider slider;               // 進捗を表示するスライダー（0～1）
    public TextMeshProUGUI progessText; // 進捗率（%）を表示するテキスト

    
    /// 外部（ボタンなど）からシーン読み込みを開始するためのメソッド
    
    
    public void Load(int sceneIndex)
    {
        // 非同期読み込み用のコルーチンを開始
        StartCoroutine(LoadAsyncchronously(sceneIndex));
    }

  
    /// シーンをバックグラウンドで読み込み、進捗をUIに反映するコルーチン
   
    IEnumerator LoadAsyncchronously(int sceneIndex)
    {
        // シーンの非同期読み込みを開始
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // ローディング画面を表示する
        loading.SetActive(true);

        // 読み込みが完了するまでループ
        while (!operation.isDone)
        {
            // operation.progressは0.0～0.9までしか上がらない仕様（残りの0.1はシーンのアクティベート）
            // そのため、0.9で割ることで0.0～1.0の範囲に正規化し、Mathf.Clamp01で安全に制限する
            float progess = Mathf.Clamp01(operation.progress / .9f);

            // スライダーの値を更新
            slider.value = progess;

            // テキストを「〇〇%」の形式で更新
            progessText.text = progess * 100f + "%";

            // 次のフレームまで待機（これにより処理を止めずにUIを更新できる）
            yield return null;
        }
    }
}