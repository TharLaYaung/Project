using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

/// 敵キャラクターのステータス、ダメージ処理、死亡、ドロップアイテムを制御するクラス

public class Enemy : MonoBehaviour
{
    [Header("コンポーネント参照")]
    [SerializeField] private SphereCollider attackCollider; // 攻撃判定用のコライダー
    [SerializeField] private GameObject enemy;           // 敵自身のオブジェクト参照
    [SerializeField] private Slider hpSlider;            // HP表示用のUIスライダー

    [Header("ステータス")]
    [SerializeField] private int MaxHP = 100;            // 最大体力
    [SerializeField] private int HP = 100;               // 現在の体力

    [Header("外部クラス参照")]
    public HitMarker hm;                                 // ヒットマーカー演出用
    public Animator animator;                           // アニメーション制御用
    public Throwable throwable;                         // 投擲物関連（必要に応じて使用）
    public Quest quest;                                  // クエスト管理用
    public GameObject[] ItemsDrop;                      // 死亡時にドロップするアイテム的配列

    private NavMeshAgent navAgent;                      // 移動制御用（NavMesh）
    private Transform player;                           // プレイヤーのTransform情報

    public bool isDead;                                 // 死亡フラグ

    private float blindTimer = 0f;                      // 目くらましタイマー

    void Start()
    {
        // 各コンポーネントの取得と初期化
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        hpSlider.value = 1f; // HPバーを満タンにする
    }

    
    /// ダメージを受ける処理
    
    public void TakeDamage(int damageAmount)
    {
        // すでに死亡している場合は処理しない
        if (HP <= 0) return;

        HP -= damageAmount;

        // 体力が0以下になったら死亡処理
        if (HP <= 0)
        {
            // 死亡アニメーションをランダムで2種類から選択
            int randomValue = Random.Range(0, 2); // 0 または 1

            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }

            // 当たり判定と攻撃判定を無効化
            DisableAttackCollider();
            DisableCapsuleCollider();

            // アイテムドロップとオブジェクトの削除予約（6秒後）
            ItemDrop();
            Destroy(enemy, 6f);

            isDead = true;

            // スコア加算処理
            if (isDead)
            {
                Score.Instance.currentScore += 10;
            }

            // プレイヤーを探して、クエストの討伐カウントを更新する
            player = GameObject.FindGameObjectWithTag("Player").transform;
            if (this.isDead)
            {
                player.GetComponent<PLAYER>().quest.goal.EnemyKilled();
            }

            // 死亡時のSE再生
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);
        }
        else
        {
            // 生存している場合はダメージアニメーションとヒットマーカー表示
            animator.SetTrigger("DAMAGE");
            hm.getHitmarker();

            // ダメージ時のSE再生
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }

    
    /// 目くらまし状態にする（スモーク弾などの演出用）
    
    public void Blind()
    {
        blindTimer = 0.5f; // keep blinded for 0.5s after leaving smoke
        animator.SetBool("isBlinding", true);
    }

   
    /// インスペクター上で射程や検知範囲を視覚化するためのデバッグ表示
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f); // 攻撃範囲

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f);  // 検知・追跡開始範囲

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 8f);   // 追跡停止範囲
    }

    void Update()
    {
        // HPバー（スライダー）の表示を更新
        hpSlider.value = (float)HP / MaxHP;

        // HPが最大値を超えないように制限
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }

        if (blindTimer > 0)
        {
            blindTimer -= Time.deltaTime;
            if (blindTimer <= 0)
            {
                animator.SetBool("isBlinding", false);
            }
        }
    }

   
    /// 攻撃用の判定（コライダー）を無効化する
   
    public void DisableAttackCollider()
    {
        if (this.attackCollider != null)
        {
            this.attackCollider.enabled = false;
        }
    }

    
    /// 自身の当たり判定（カプセルコライダー）を無効化する
    
    public void DisableCapsuleCollider()
    {
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = false;
        }
    }

    
    /// 設定されたアイテムを生成してドロップさせる
    
    private void ItemDrop()
    {
        for (int i = 0; i < ItemsDrop.Length; i++)
        {
            // 少し上に浮かせた位置にアイテムを生成
            Instantiate(ItemsDrop[i], transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            // 自身を削除予約（9秒後）
            Destroy(this, 9f);
        }
    }
}