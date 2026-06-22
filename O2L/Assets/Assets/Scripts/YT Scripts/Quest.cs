using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 個々のクエストデータを保持するクラス
/// [System.Serializable] を付けることで、Unityのエディタ（インスペクター）上で内容を編集できるようになります

[System.Serializable]
public class Quest
{
    // クエストが現在進行中かどうか
    public bool isActive;

    // クエストのタイトル
    public string title;

    // クエストの詳しい説明文
    [TextArea]
    public string description;

    // クエスト達成時に得られるスコア
    public int Score;

    // クエスト達成時に得られる報酬（お金やアイテムなど）
    public int Reward;

    // クエストの達成条件を管理するクラス（QuestGoal）への参照
    public QuestGoal goal;


    /// クエストを完了させるためのメソッド

    public void Complete()
    {
        // 進行状態を終了にする
        isActive = false;

        // コンソールに完了したクエスト名を表示
        Debug.Log(title + "was completed!");

        // ここに報酬の付与処理や、完了後のエフェクトなどを追加することが一般的です
    }
}