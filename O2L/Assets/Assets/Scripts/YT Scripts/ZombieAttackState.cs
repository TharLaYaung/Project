using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// アニメーションと同期して攻撃の開始と終了を制御します。

public class ZombieAttackState : StateMachineBehaviour
{
    // 攻撃時のターゲットとの距離を計算するため、プレイヤーの座標を保持します。
    private Transform player;
    // 回転や移動を制御するため、NavMeshAgentを保持します。
    private NavMeshAgent agent;

    // プレイヤーが逃げた際に攻撃をキャンセルするため、距離のしきい値を設定します。
    public float stopAttackingDistance = 2.5f;

   
    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: playerとagentの参照が取得されます。
    /// 状態に遷移した直後に必要なコンポーネントを準備するため、一度だけ取得を行います。
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }

    
    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: 攻撃音の再生、プレイヤーの追従、状態の遷移が行われます。
    /// プレイヤーの位置に応じて攻撃の継続可否を判定するため、毎フレーム状態を監視します。
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieAttack);
        }

        LookAtPlayer();

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

   
    /// 攻撃が外れないよう、常にプレイヤーの方向を向くようにします。
    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        // 不自然な傾きを防ぐため、Y軸の回転のみを適用します。
        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    
    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: 攻撃音が停止されます。
    /// 状態遷移後も音が鳴り続けるのを防ぐため、音声を停止します。
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.zombieChannel.Stop();
    }
}
