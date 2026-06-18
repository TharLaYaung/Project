using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの体力回復ロジックとアイテム種別を紐づけるためのデータクラス
public class HealItems : MonoBehaviour
{
    [Header("アイテムの設定")]
    // 回復効果の強弱や所持上限を調整可能にするためパラメータ化
    public int itemAmount;

    // 複数種類の回復アイテムに対して個別の処理を分岐させるための識別子
    public HealItemsType healitemsType;

    public enum HealItemsType
    {
        Medkit
    }
}
