using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// NPCなどにアタッチし、プレイヤーにクエストを提示・発行するクラス
public class QuestGiver : MonoBehaviour
{
    // 発行するクエストの情報
    public Quest quest;

    // クエストを受け取るプレイヤーへの参照
    public PLAYER player;

    [Header("UI要素")]
    public GameObject questWindow;          // クエスト情報の表示ウィンドウ
    public TextMeshProUGUI titleText;       // クエスト名のテキスト
    public TextMeshProUGUI descriptionText; // 説明文のテキスト
    public TextMeshProUGUI scoreText;       // 獲得スコアの表示テキスト
    public TextMeshProUGUI rewardText;      // 報酬内容の表示テキスト

    /// ゲーム開始時の初期化
    void Awake()
    {
        // 最初はクエストウィンドウを閉じておく
        if (questWindow != null)
        {
            questWindow.SetActive(false);
        }
    }

    /// プレイヤーがNPCの周囲（トリガー）に入った時の処理
    void OnTriggerEnter(Collider other)
    {
        // 接触したのがプレイヤーであればウィンドウを開く
        if (other.CompareTag("Player"))
        {
            OpenQuestWindow();
        }
    }

    /// クエストの内容をUIに反映させて表示する
    public void OpenQuestWindow()
    {
        // --- ヌルポ (NullReferenceException) 対策 ---
        // Inspectorでアタッチし忘れているものがないか、実行時に分かりやすく警告を出します。
        // これを入れることで、今後エラーが起きた際にどこが原因か一瞬で分かるようになります。
        if (quest == null) { return; }
        if (titleText == null) { return; }
        if (descriptionText == null) { return; }
        if (scoreText == null) { return; }
        if (questWindow == null) { return; }
        // ---------------------------------------------

        // クエストのデータをUIテキストに流し込む
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        scoreText.text = quest.Score.ToString();
        // rewardText.text = quest.Reward.ToString(); // 報酬表示が必要な場合

        // UI操作のためにマウスカーソルを表示し、範囲内に固定する
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // すでにクエストを受注済みの場合は、ウィンドウを表示しない
        if (quest.isActive == true)
        {
            questWindow.SetActive(false);
        }
        else
        {
            questWindow.SetActive(true);
        }
    }

    /// クエスト受注ボタンが押された時の処理
    public void AcceptQuest()
    {
        // プレイヤーの参照が漏れている場合もエラーを出して処理を中断します
        if (player == null)
        {
            return;
        }

        // ウィンドウを閉じる
        // ※ Unityの仕様上、オブジェクト破棄時の挙動の都合で「?.」よりも「!= null」の方が安全です
        if (questWindow != null)
        {
            questWindow.SetActive(false);
        }

        // クエストを「進行中」状態にする
        quest.isActive = true;

        // プレイヤー側にこのクエストを登録し、追跡を開始させる
        player.quest = quest;
    }

    /// プレイヤーが離れたら自動的にウィンドウを閉じる
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (questWindow != null)
            {
                questWindow.SetActive(false);
            }
        }
    }

    /// 閉じるボタンなどで手動でウィンドウを閉じる処理
    public void Close()
    {
        if (questWindow != null)
        {
            questWindow.SetActive(false);
        }
    }
}