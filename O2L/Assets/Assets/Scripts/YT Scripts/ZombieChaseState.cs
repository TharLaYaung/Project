using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// プレイヤーを追跡し、距離に応じて攻撃に切り替えます。

public class ZombieChaseState : StateMachineBehaviour
{
    // 追従を行うため、移動を制御するNavMeshAgentを保持します。
    private NavMeshAgent agent;
    // ターゲットとの距離を計算するため、プレイヤーの座標を保持します。
    private Transform player;

    [Header("追跡設定")]
    // 追跡時の移動速度
    public float chaseSpeed = 6f;
    // 追跡を辞める距離（これ以上離れると追跡を止める）
    public float stopChasingDistance = 8f;
    // 攻撃を開始する距離
    public float attackingDistance = 2.5f;

  
    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: agentとplayerの参照が取得され、移動速度が更新されます。
    /// 追跡処理に必要な準備を行うため、状態遷移時に初期化します。

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーをタグで検索して座標を取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // 内部のNavMeshAgentコンポーネントを取得
        agent = animator.GetComponent<NavMeshAgent>();

        // エージェントの移動速度を追跡用プロファイルに設定
        agent.speed = chaseSpeed;
    }

   
    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: プレイヤーへの追従と、距離に応じた状態の切り替えが行われます。
    /// プレイヤーの移動にリアルタイムで対応するため、毎フレーム判定を行います。

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ゾンビの鳴き声（追跡中のものなど）が再生されていなければ、音声を再生
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);
        }

        // NavMeshAgentに対して、プレイヤーの現在位置を目的地として設定
        // エラー回避：NavMesh上にいる場合のみ目的地を設定する
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
        // 常にプレイヤーの方向を向く
        animator.transform.LookAt(player);

        // プレイヤーとの距離を計算
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // --- 追跡を終了するかどうかの判定 ---
        if (distanceFromPlayer > stopChasingDistance)
        {
            // 一定以上離れたら、追跡フラグを折って待機などの状態へ遷移
            animator.SetBool("isChasing", false);
        }

        // --- 攻撃を開始するかどうかの判定 ---
        if (distanceFromPlayer < attackingDistance)
        {
            // 攻撃範囲内に入ったら、攻撃フラグを立てて攻撃状態へ遷移
            animator.SetBool("isAttacking", true);
        }
    }


    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: 移動が停止し、音声が止まります。
    /// 追跡を安全に終了させるため、現在の位置で停止させます。
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(agent.transform.position);
        }

        SoundManager.Instance.zombieChannel.Stop();
    }
}
