using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

// 敵キャラクターのダメージ処理やステータスを管理し、ゲームの進行を円滑にするため
public class Enemy : MonoBehaviour
{
    private const int DEATH_ANIM_MIN = 0;
    private const int DEATH_ANIM_MAX = 2;
    private const float ENEMY_DESTROY_DELAY = 6f;
    private const int SCORE_REWARD = 10;
    private const float BLIND_DURATION = 0.5f;
    private const float ATTACK_RANGE = 2.5f;
    private const float CHASE_START_RANGE = 18f;
    private const float CHASE_STOP_RANGE = 8f;
    private const float ITEM_DROP_Y_OFFSET = 1f;
    private const float SCRIPT_DESTROY_DELAY = 9f;

    [Header("Components")]
    [SerializeField] private SphereCollider attackCollider;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Slider hpSlider;

    [Header("Status")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int hp = 100;

    [Header("External References")]
    public HitMarker hm;
    public Animator animator;
    public Throwable throwable;
    public Quest quest;
    public GameObject[] itemsDrop;

    private NavMeshAgent navAgent;
    private Transform player;

    public bool isDead;

    private float blindTimer = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        hpSlider.value = 1f;
    }

    // Input: damageAmount (受けるダメージ量)
    // Output: なし
    // Side Effects: HPを減少させ、0以下なら死亡処理へ移行する
    public void TakeDamage(int damageAmount)
    {
        // 既に死亡している場合は処理を中断し、二重に死亡判定されるのを防ぐため
        if (hp <= 0) return;

        hp -= damageAmount;

        if (hp <= 0)
        {
            // パターン化を防ぐため、ランダムに死亡アニメーションを選択する
            int randomValue = Random.Range(DEATH_ANIM_MIN, DEATH_ANIM_MAX);

            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }

            DisableAttackCollider();
            DisableCapsuleCollider();

            ItemDrop();
            Destroy(enemy, ENEMY_DESTROY_DELAY);

            isDead = true;

            // プレイヤーに報酬を与えるためスコアを加算する
            if (isDead)
            {
                Score.Instance.currentScore += SCORE_REWARD;
            }

            // クエストの進行状況を更新するためプレイヤー側の参照を探す
            player = GameObject.FindGameObjectWithTag("Player").transform;
            if (this.isDead)
            {
                player.GetComponent<PLAYER>().quest.goal.EnemyKilled();
            }

            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);
        }
        else
        {
            // 生存していることをプレイヤーに視覚的・聴覚的に伝えるため
            animator.SetTrigger("DAMAGE");
            hm.getHitmarker();

            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }

    // Input: なし
    // Output: なし
    // Side Effects: アニメーションを再生し、一定時間視界を奪う状態にする
    public void Blind()
    {
        // スモーク弾等の効果で一定時間敵の行動を制限するため
        blindTimer = BLIND_DURATION;
        animator.SetBool("isBlinding", true);
    }

    // 開発時にエディタ上で敵の索敵や攻撃の範囲を視覚化し、調整しやすくするため
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ATTACK_RANGE);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, CHASE_START_RANGE);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, CHASE_STOP_RANGE);
    }

    private void Update()
    {
        // UI上で常に現在の体力を可視化するため
        hpSlider.value = (float)hp / maxHp;

        // 回復アイテムなどで最大HPを超過しないよう制限をかけるため
        if (hp > maxHp)
        {
            hp = maxHp;
        }

        if (blindTimer > 0)
        {
            blindTimer -= Time.deltaTime;
            
            // 時間経過で盲目状態を解除し、再び行動可能にするため
            if (blindTimer <= 0)
            {
                animator.SetBool("isBlinding", false);
            }
        }
    }

    // Input: なし
    // Output: なし
    // Side Effects: 攻撃用コライダーを無効化する
    public void DisableAttackCollider()
    {
        // 死亡後や被ダメージ中に攻撃判定が残り続けるのを防ぐため
        if (this.attackCollider != null)
        {
            this.attackCollider.enabled = false;
        }
    }

    // Input: なし
    // Output: なし
    // Side Effects: カプセルコライダーを無効化する
    public void DisableCapsuleCollider()
    {
        // 死亡した敵にプレイヤーが引っかかったり、不要な物理演算が起きるのを防ぐため
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = false;
        }
    }

    // Input: なし
    // Output: なし
    // Side Effects: ドロップアイテムを生成し、数秒後にこのスクリプトを破棄する
    private void ItemDrop()
    {
        for (int i = 0; i < itemsDrop.Length; i++)
        {
            // 地面に埋まらないようにY軸を少しずらしてアイテムを生成するため
            Instantiate(itemsDrop[i], transform.position + new Vector3(0, ITEM_DROP_Y_OFFSET, 0), Quaternion.identity);
            
            // ドロップ後に不要になったスクリプトを破棄してメモリを解放するため
            Destroy(this, SCRIPT_DESTROY_DELAY);
        }
    }
}
