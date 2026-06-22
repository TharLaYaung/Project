using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// ゲームのスコア計算とハイスコアの保持、およびUI表示を管理するクラス

public class Score : MonoBehaviour
{
    // 他のクラスから Score.Instance でアクセスできるようにするための静的変数（シングルトン）
    public static Score Instance { get; set; }

    // 全プレイを通じた最高スコア
    public int highScore;

    // 今回のプレイでの現在のスコア
    public int currentScore;

    [Header("UI表示設定")]
    // 現在のスコアを表示するTextMeshProテキスト
    public TextMeshProUGUI CurrentScore;

    // ハイスコアを表示するTextMeshProテキスト
    public TextMeshProUGUI HighScore;

    
    /// インスタンス生成時の初期化
    
    private void Awake()
    {
        // シングルトンの重複チェック：既にインスタンスが存在していれば自分を破棄する
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // 唯一のインスタンスとして自分を登録
            Instance = this;
        }
    }

    
    /// ゲーム開始時の初期化（必要に応じてセーブデータからの読み込みなどを追加）
    
    void Start()
    {
        // 初期状態の表示設定など
    }

    
    /// 毎フレーム実行される更新処理
    
    void Update()
    {
        // 現在のスコアとハイスコアをUIテキストに反映
        CurrentScore.text = $"{currentScore}";
        HighScore.text = $"{highScore}";

        // 現在のスコアがハイスコアを上回った場合、数値を更新する
        if (currentScore > highScore)
        {
            highScore = currentScore;
            // ここでセーブ処理（PlayerPrefsなど）を呼ぶと、次回プレイ時も記録が残ります
        }
    }
}