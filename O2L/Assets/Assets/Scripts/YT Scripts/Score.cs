using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// プレイ中のスコア状態を管理し、UIへの反映とハイスコアの更新を行う
public class Score : MonoBehaviour
{
    public static Score Instance { get; set; }

    public int highScore;
    public int currentScore;

    [Header("UI表示設定")]
    [UnityEngine.Serialization.FormerlySerializedAs("CurrentScore")]
    public TextMeshProUGUI currentScoreUi;

    [UnityEngine.Serialization.FormerlySerializedAs("HighScore")]
    public TextMeshProUGUI highScoreUi;

    // Input: なし, Output: なし, Side Effects: シングルトンの重複を破棄し、自身を永続化する
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Input: なし, Output: なし, Side Effects: 初期状態を設定する
    private void Start()
    {
    }

    // Input: なし, Output: なし, Side Effects: UIにスコアを描画し、必要に応じてハイスコアを更新する
    private void Update()
    {
        currentScoreUi.text = $"{currentScore}";
        highScoreUi.text = $"{highScore}";

        if (currentScore > highScore)
        {
            highScore = currentScore;
        }
    }
}
