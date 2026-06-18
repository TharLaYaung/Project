using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// クエストの達成条件と現在の進捗を判定・管理するクラス
[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;
    public int requiredAmount;
    public int currentAmount;

    /// 目標の達成状況を判定する
    /// 出力: 規定数に達していればtrue、それ以外はfalse
    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

    /// 対象の敵を討伐した際の進捗更新を行う
    /// 副作用: 討伐クエストの場合のみカウントを加算
    public void EnemyKilled()
    {
        if (goalType == GoalType.Kill)
        {
            currentAmount++;
        }
    }

    /// 対象のアイテムを収集した際の進捗更新を行う
    /// 副作用: 収集クエストの場合のみカウントを加算
    public void ItemCollected()
    {
        if (goalType == GoalType.Gathering)
        {
            currentAmount++;
        }
    }
}

public enum GoalType
{
    Kill,
    Gathering
}
