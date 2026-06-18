using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// クエストの基本データと進捗状態を管理するためのクラス
[System.Serializable]
public class Quest
{
    public bool isActive;
    public string title;

    [TextArea]
    public string description;

    public int score;
    public int reward;
    public QuestGoal goal;

    /// クエスト完了時の状態更新を行う
    /// 副作用: 進行状態のフラグを解除する
    public void Complete()
    {
        isActive = false;
        Debug.Log(title + "was completed!");
    }
}
