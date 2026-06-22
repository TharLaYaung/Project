using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// クエストの具体的な目標（ゴール）とその進捗を管理するクラス
/// [System.Serializable] を付与することでインスペクターからの設定を可能にします

[System.Serializable]
public class QuestGoal
{
    // クエストの種類（討伐か、収集か）
    public GoalType goaltype;

    // 達成に必要な数（例：10体、5個など）
    public int requiredAmount;

    // 現在の達成数
    public int currentAmount;

    
    /// 目標に到達したかどうかを判定する
    
    /// <returns>現在の数が目標数以上であれば true</returns>
    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

   
    /// 敵が倒された時に呼び出すメソッド
    
    public void EnemyKilled()
    {
        // このクエストの目標が「討伐（Kill）」である場合のみ、カウントを進める
        if (goaltype == GoalType.Kill)
        {
            currentAmount++;
        }
    }

    
    /// アイテムを拾った時に呼び出すメソッド
    
    public void ItemCollected()
    {
        // このクエストの目標が「収集（Gathering）」である場合のみ、カウントを進める
        if (goaltype == GoalType.Gathering)
        {
            currentAmount++;
        }
    }
}


/// クエストの目的の種類を定義する列挙型

public enum GoalType
{
    Kill,      // 敵を倒すクエスト
    Gathering  // アイテムを集めるクエスト
}