using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ゲーム全体で共通して使用するエフェクトなどのリソース（プレハブ）を一括管理するクラス

public class GlobalReferences : MonoBehaviour
{
    // シングルトンインスタンス：どこからでも GlobalReferences.Instance でアクセス可能
    public static GlobalReferences Instance { get; set; }

    [Header("エフェクトプレハブの設定")]
    // 弾が着弾した時のエフェクト
    public GameObject bulletImpactEffectPrefab;

    // 手榴弾の爆発エフェクト
    public GameObject grenadeExplosionEffect;

    // スモーク弾の効果エフェクト
    public GameObject smokeGrenadeEffect;

    // ダメージを受けた時の血飛沫エフェクト
    public GameObject bloodSprayEffect;

    
    /// インスタンスの初期化と重複チェック
    
    private void Awake()
    {
        // すでにインスタンスが存在し、自分自身でない場合は重複を避けるために破棄
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // このオブジェクトを唯一の参照先として登録
            Instance = this;
        }
    }
}