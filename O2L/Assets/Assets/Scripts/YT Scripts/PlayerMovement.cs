using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// プレイヤーの移動や姿勢制御（しゃがみ動作など）を管理するクラス

public class PlayerMovement : MonoBehaviour
{
    // しゃがんでいる時のコライダーの高さ
    public float crouchHeight = 1f;

    // 立っている時のコライダーの高さ
    public float standingHeight = 2f;

    // プレイヤーの当たり判定を制御するカプセルコライダー
    private CapsuleCollider capsuleCollider;

 
    /// ゲーム開始時に一度だけ実行される初期化処理
   
    void Start()
    {
        // 自身にアタッチされている CapsuleCollider コンポーネントを取得
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

  
    /// 毎フレーム実行される更新処理
   
    void Update()
    {
        // 左コントロールキー（LeftControl）が押されている間
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // コライダーの高さを「しゃがみ用」に設定
            // これにより、低い場所を潜り抜けたり、当たり判定を小さくしたりできます
            capsuleCollider.height = crouchHeight;
        }
        else
        {
            // キーを離している時は、高さを「立ち状態」に戻す
            capsuleCollider.height = standingHeight;
        }
    }
}