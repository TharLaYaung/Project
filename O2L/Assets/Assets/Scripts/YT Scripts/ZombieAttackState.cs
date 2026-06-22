using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// ゾンビの攻撃状態（AnimatorのAttackステート）を管理するクラス
/// StateMachineBehaviourを継承し、アニメーションの遷移に合わせて実行されます

public class ZombieAttackState : StateMachineBehaviour
{
    // ターゲットとなるプレイヤーの座標情報
    Transform player;
    // ゾンビ自身の移動制御用エージェント
    NavMeshAgent agent;

    // 攻撃を止めて追跡などに戻る距離のしきい値
    public float stopAttackingDistance = 2.5f;

   
    /// 攻撃アニメーションが開始された瞬間に一度だけ呼ばれる
  
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // シーン内からプレイヤーを探して座標情報を取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // アニメーターがアタッチされているオブジェクトからNavMeshAgentを取得
        agent = animator.GetComponent<NavMeshAgent>();
    }

    
    /// 攻撃アニメーション再生中、毎フレーム呼ばれる
   
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ゾンビの攻撃音が再生されていない場合、攻撃SEを再生する
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieAttack);
        }

        // 常にプレイヤーの方を向くように回転を制御
        LookAtPlayer();

        // プレイヤーと自分自身の距離を計算
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // プレイヤーが攻撃範囲外まで離れた場合
        if (distanceFromPlayer > stopAttackingDistance)
        {
            // アニメーターの「isAttacking」フラグを折って、追跡状態などへ遷移させる
            animator.SetBool("isAttacking", false);
        }
    }

   
    /// ゾンビの向きをプレイヤーの方へ向ける処理
   
    private void LookAtPlayer()
    {
        // プレイヤーへの方向ベクトルを計算
        Vector3 direction = player.position - agent.transform.position;
        // その方向を向くための回転を適用
        agent.transform.rotation = Quaternion.LookRotation(direction);

        // Y軸（左右）の回転のみを残し、X軸やZ軸の傾き（のけぞり等）を防ぐ
        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    
    /// 攻撃状態から別の状態（追跡など）へ移る瞬間に呼ばれる
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 攻撃状態が終わったので、再生中のゾンビの音声を停止する
        SoundManager.Instance.zombieChannel.Stop();
    }
}