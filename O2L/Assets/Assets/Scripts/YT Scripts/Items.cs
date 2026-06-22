using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ゲーム内の収集アイテムやクエストアイテムのデータを管理するクラス

public class Items : MonoBehaviour
{
    // アイテムの個数、またはスコア加算量などを設定します
    [Header("アイテム設定")]
    public int itemAmount;

    // このアイテムがどの種類（注射器など）かをインスペクターから設定します
    public ItemsType itemsType;

   
    /// アイテムの種類を定義する列挙型
    
    public enum ItemsType
    {
        // 注射器（シリンジ）
        // クエストの目的達成や特定のギミック解除に使用することを想定しています
        Syringe
    }
}