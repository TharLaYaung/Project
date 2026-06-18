using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// アイテムがドロップした際の物理的な動き（跳ねる演出など）を制御するクラス
public class ItemsDrop : MonoBehaviour
{
    private Rigidbody itemRb;

    [Header("ドロップ設定")]
    public float dropForce = 5f;

    // 入力: なし, 出力: なし, 副作用: アイテムに上方向の力を加える
    void Start()
    {
        if (TryGetComponent(out itemRb))
        {
            // 敵を倒した時にアイテムが飛び出す視覚的なフィードバックを与えるため
            itemRb.AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        }
    }
}
