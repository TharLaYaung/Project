using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


/// アイテムがドロップした際の物理的な動き（跳ねる演出など）を制御するクラス

public class ItemsDrop : MonoBehaviour
{
    // アイテムの物理演算を扱うためのリジッドボディ
    private Rigidbody itemRb;

    // アイテムが飛び出す力の強さ
    [Header("ドロップ設定")]
    public float dropForce = 5f;

   
    /// オブジェクトが生成された瞬間に実行される処理
    
    void Start()
    {
        // 自身のリジッドボディコンポーネントの取得を試みる
        // GetComponent<Rigidbody>() よりも安全で効率的な書き方です
        if (TryGetComponent(out itemRb))
        {
            // アイテムを真上（Vector3.up）に向かって、瞬間的な力（Impulse）で弾き飛ばす
            // これにより、敵を倒した時にアイテムがポーンと飛び出す演出になります
            itemRb.AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        }
    }
}