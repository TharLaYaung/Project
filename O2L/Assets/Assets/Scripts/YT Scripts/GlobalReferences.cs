using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 共通エフェクトのプレハブ参照をメモリ上にキャッシュし、ロード遅延を防ぐため一括管理する
public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; set; }

    [Header("エフェクトプレハブの設定")]
    // 描画負荷を考慮し、軽量な着弾パーティクルを割り当てる想定
    public GameObject bulletImpactEffectPrefab;

    // 爆発の広範囲な視覚フィードバックを提供するため保持
    public GameObject grenadeExplosionEffect;

    // 視界遮断ロジックと連動させるためスモーク用プレハブを保持
    public GameObject smokeGrenadeEffect;

    // 被弾時のダメージ認知を向上させるため血飛沫エフェクトを保持
    public GameObject bloodSprayEffect;

    private void Awake()
    {
        // 参照の競合によるNullReferenceExceptionを防ぐため単一インスタンスを保証する
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
