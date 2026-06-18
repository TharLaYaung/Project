using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] private SphereCollider attackCollider;

    void Start()
    {
        // 意図しないタイミングでのダメージ発生を防ぐため初期化時に判定を消す
        DisableAttackCollider();
    }

    void Update()
    {
    }

    /// 攻撃アニメーションの特定のフレームで判定を発生させる
    /// 副作用：attackColliderのenabledをtrueにする
    public void EnableAttackCollider()
    {
        if (this.attackCollider != null)
        {
            this.attackCollider.enabled = true;
        }
    }

    /// 攻撃アニメーション終了時に不要な判定を残さないようにする
    /// 副作用：attackColliderのenabledをfalseにする
    public void DisableAttackCollider()
    {
        if (this.attackCollider != null)
        {
            this.attackCollider.enabled = false;
        }
    }
}
