using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ユニットの攻撃判定（コライダー）の切り替えを管理するクラス

public class UnitController : MonoBehaviour
{
    // 攻撃判定として使用する球体コライダー（インスペクターからアサインする）
    [SerializeField] private SphereCollider attackCollider;

   
    /// ゲーム開始時に呼び出される初期化処理
    
    void Start()
    {
        // 起動時は攻撃判定が出ないように無効化しておく
        DisableAttackCollider();
    }

    
    /// 毎フレームの更新処理（現在は未使用）
    
    void Update()
    {

    }

    
    /// 攻撃判定（コライダー）を有効にする処理
    /// アニメーションイベントなどから呼び出すことを想定
    
    public void EnableAttackCollider()
    {
        // コライダーが参照されていることを確認
        if (this.attackCollider != null)
        {
            // コライダーをONにする
            this.attackCollider.enabled = true;
        }
    }

    /// 攻撃判定（コライダー）を無効にする処理
    /// 攻撃終了時などに呼び出す
   
    public void DisableAttackCollider()
    {
        // コライダーが参照されていることを確認
        if (this.attackCollider != null)
        {
            // コライダーをOFFにする
            this.attackCollider.enabled = false;
        }
    }
}