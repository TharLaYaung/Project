using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 回復アイテムのデータと種類を管理するクラス

public class HealItems : MonoBehaviour
{
    // アイテムの個数、または回復量などを設定します
    [Header("アイテムの設定")]
    public int itemAmount;

    // このアイテムがどの種類（メディキットなど）かを設定します
    public HealItemsType healitemsType;

    
    /// 回復アイテムの種類を定義する列挙型
    
    public enum HealItemsType
    {
        // 救急キット（今後、包帯や薬などをここに追加できます）
        Medkit
    }
}