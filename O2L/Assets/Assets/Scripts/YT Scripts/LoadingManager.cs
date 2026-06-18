using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

/// フェイクローディング演出と、読み込み中のメッセージ切り替えを管理するクラス
public class LoadingManager : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI loadingText;

    [Header("挙動設定")]
    [SerializeField] float loadSpeed = 0.6f;
    [SerializeField] float tipChangeInterval = 1.5f;

    public SceneDirector sd;

    [Header("ローディングメッセージリスト")]
    [TextArea]
    [SerializeField]
    string[] tips =
    {
        "ファイルを読み込み中...",
        "既存のファイルを展開中...",
        "ピクセルをレンダリング中...",
        "機能を拡張中...",
        "シェーダーをコンパイル中..."
    };

    int lastTipIndex = -1;
    bool isLoadingDone = false;

    // 入力: なし, 出力: なし, 副作用: ローディング演出とメッセージのループ開始
    private void Start()
    {
        loadingSlider.value = 0f;

        // すぐにシーンが切り替わると不自然なため、演出としてのローディングを行う
        StartCoroutine(FakeLoading());

        // 待機中のプレイヤーを飽きさせないためにテキストを順次切り替える
        StartCoroutine(CycleTips());
    }

    // 入力: なし, 出力: IEnumerator, 副作用: スライダー値の更新とシーン遷移
    IEnumerator FakeLoading()
    {
        // 偽の進捗バーを徐々に進めるため
        while (loadingSlider.value < 1f)
        {
            loadingSlider.value += loadSpeed * Time.deltaTime;
            yield return null;
        }

        isLoadingDone = true;

        string sceneToLoad = PlayerPrefs.GetString("SelectedMap", "GameScene");

        SceneManager.LoadScene(sceneToLoad);
    }

    // 入力: なし, 出力: IEnumerator, 副作用: UIテキストの定期的な更新
    IEnumerator CycleTips()
    {
        SetRandomTip();

        // ローディングが完了するまで継続してメッセージを更新するため
        while (!isLoadingDone)
        {
            yield return new WaitForSeconds(tipChangeInterval);
            SetRandomTip();
        }
    }

    // 入力: なし, 出力: なし, 副作用: UIテキストの書き換え
    void SetRandomTip()
    {
        // 表示可能なメッセージが存在しない場合のクラッシュを防ぐため
        if (tips == null || tips.Length == 0) return;

        int index = UnityEngine.Random.Range(0, tips.Length);

        // 同じメッセージが連続で表示される違和感をなくすため
        if (index == lastTipIndex && tips.Length > 1)
        {
            index = (index + 1) % tips.Length;
        }

        lastTipIndex = index;

        if (loadingText != null)
        {
            loadingText.text = tips[index];
        }
    }
}
