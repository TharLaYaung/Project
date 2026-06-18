using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// シーン遷移時のローディング画面と進捗バーを制御するクラス
public class Loading : MonoBehaviour
{
    private const float LOADING_COMPLETE_RATIO = 0.9f;
    private const float PERCENTAGE_MULTIPLIER = 100f;

    [Header("UI設定")]
    public GameObject loading;
    public Slider slider;
    public TextMeshProUGUI progessText;

    // 入力: 遷移先シーン番号, 出力: なし, 副作用: 非同期読み込みコルーチンの開始
    public void Load(int sceneIndex)
    {
        // メインスレッドを止めずにバックグラウンドでシーンを読み込むため
        StartCoroutine(LoadAsyncchronously(sceneIndex));
    }

    // 入力: 遷移先シーン番号, 出力: IEnumerator, 副作用: UIへの進捗状況の反映
    IEnumerator LoadAsyncchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loading.SetActive(true);

        // 読み込み完了まで待機し、進行状況をユーザーに視覚的に伝えるため
        while (!operation.isDone)
        {
            float progess = Mathf.Clamp01(operation.progress / LOADING_COMPLETE_RATIO);

            slider.value = progess;

            progessText.text = progess * PERCENTAGE_MULTIPLIER + "%";

            // 1フレーム待機してUIの更新を画面に反映させるため
            yield return null;
        }
    }
}
