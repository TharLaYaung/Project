using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// ゾンビの追跡状態（AnimatorのChaseステート）を管理するクラス
/// プレイヤーを追いかけ、距離に応じて攻撃状態へ移行させます

public class ZombieChaseState : StateMachineBehaviour
{
    // 移動制御用のナビメッシュエージェント
    NavMeshAgent agent;
    // 追いかける対象（プレイヤー）の座標情報
    Transform player;

    [Header("追跡設定")]
    // 追跡時の移動速度
    public float chaseSpeed = 6f;
    // 追跡を諦める距離（これ以上離れると追跡を止める）
    public float stopChasingDistance = 8f;
    // 攻撃を開始する距離
    public float attackingDistance = 2.5f;

  
    /// 追跡状態に入った瞬間に一度だけ呼ばれる

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーをタグで検索して座標を取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // 自身のNavMeshAgentコンポーネントを取得
        agent = animator.GetComponent<NavMeshAgent>();

        // エージェントの移動速度を追跡用スピードに設定
        agent.speed = chaseSpeed;
    }

   
    /// 追跡状態の間、毎フレーム呼ばれる

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ゾンビの音声（追跡中のうなり声など）が再生されていない場合、再生を開始する
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);
        }

        // NavMeshAgentに対して、プレイヤーの現在位置を目的地として設定
        // エラー防止：NavMesh上にいる場合のみ目的地を設定する
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
        // 常にプレイヤーの方を向かせる
        animator.transform.LookAt(player);

        // プレイヤーとの距離を計算
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // --- 追跡を停止するかどうかの判定 ---
        if (distanceFromPlayer > stopChasingDistance)
        {
            // 一定以上離れたら、追跡フラグを折って待機などの状態へ戻す
            animator.SetBool("isChasing", false);
        }

        // --- 攻撃を開始するかどうかの判定 ---
        if (distanceFromPlayer < attackingDistance)
        {
            // 攻撃射程内に入ったら、攻撃フラグを立てて攻撃状態へ遷移させる
            animator.SetBool("isAttacking", true);
        }
    }


    /// 追跡状態を抜ける瞬間に呼ばれる

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 目的地を現在の位置に設定し、移動を即座に停止させる
        // エラー防止：NavMesh上にいる場合のみ目的地を設定する
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(agent.transform.position);
        }

        // 再生中のゾンビ音声を停止する
        SoundManager.Instance.zombieChannel.Stop();
    }
}