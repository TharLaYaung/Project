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
    [SerializeField] Slider LoadingSlider;      // ローディング進捗を表示するスライダー
    [SerializeField] TextMeshProUGUI LoadingText; // メッセージを表示するテキストメッシュプロ

    [Header("挙動設定")]
    [SerializeField] float LoadSpeed = 0.6f;        // ゲージが溜まるスピード
    [SerializeField] float tipChangeInterval = 1.5f; // メッセージを切り替える間隔（秒）
    //[SerializeField] string menuSceneName = "GameScene"; // デフォルトの遷移先シーン名

    // シーンの演出を管理するスクリプトへの参照（必要に応じて使用）
    public SceneDirector SD;

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

    int lastTipIndex = -1;    // 直前に表示したメッセージのインデックス
    bool isLoadingDone = false; // ローディング（演出）が完了したかどうかのフラグ

    
    /// 開始時にローディング演出とメッセージのループを開始する
    
    private void Start()
    {
        // スライダーをリセット
        LoadingSlider.value = 0f;

        // ローディングゲージを増やすコルーチンを開始
        StartCoroutine(FakeLoading());

        // メッセージを定期的に切り替えるコルーチンを開始
        StartCoroutine(CycleTips());
    }

    
    /// 一定速度でスライダーの値を増やし、完了後にシーンを遷移させる
    
    IEnumerator FakeLoading()
    {
        // スライダーが満タン（1.0）になるまで繰り返す
        while (LoadingSlider.value < 1f)
        {
            // 時間経過に合わせて値を増加させる
            LoadingSlider.value += LoadSpeed * Time.deltaTime;
            yield return null;
        }

        // 演出完了フラグを立てる
        isLoadingDone = true;

        // PlayerPrefsから選択されたマップ名を取得（保存されていなければGameScene）
        string sceneToLoad = PlayerPrefs.GetString("SelectedMap", "GameScene");

        // 指定されたシーンへ遷移
        SceneManager.LoadScene(sceneToLoad);
    }

    /// ローディングが終わるまで、一定間隔でランダムなメッセージを表示する
    
    IEnumerator CycleTips()
    {
        // 最初のメッセージを表示
        SetRandomTip();

        // ローディング演出が続いている間、ループを回す
        while (!isLoadingDone)
        {
            // 指定された秒数待機
            yield return new WaitForSeconds(tipChangeInterval);

            // 次のメッセージを表示
            SetRandomTip();
        }
    }

    
    /// リストの中からランダムにメッセージを選択し、UIに表示する
    
    void SetRandomTip()
    {
        // メッセージリストが空の場合は何もしない
        if (tips == null || tips.Length == 0) return;

        // ランダムにインデックスを選択
        int index = UnityEngine.Random.Range(0, tips.Length);

        // 2回連続で同じメッセージにならないように調整
        if (index == lastTipIndex && tips.Length > 1)
        {
            index = (index + 1) % tips.Length;
        }

        // 選択したインデックスを保存
        lastTipIndex = index;

        // テキストUIにメッセージを反映
        if (LoadingText != null)
        {
            LoadingText.text = tips[index];
        }
    }
}