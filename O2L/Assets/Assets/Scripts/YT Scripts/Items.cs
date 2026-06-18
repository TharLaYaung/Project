using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ゲーム内の収集アイテムやクエストアイテムのデータを管理するクラス
public class Items : MonoBehaviour
{
    [Header("アイテム設定")]
    public int itemAmount;
    public ItemsType itemsType;

    public enum ItemsType
    {
        // クエストの目的達成や特定のギミック解除に使用するため
        Syringe
    }
}
