using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 待機時間を制御し、次の行動（巡回や追跡）への移行を決定します。

public class ZombieIdleState : StateMachineBehaviour
{
    // パトロールに移行するタイミングを計るため、経過時間を記録します。
    private float timer;

    public float idleTime = 0f;

    // 検知範囲を計算するため、プレイヤーの座標を保持します。
    private Transform player;

    public float detectionAreaRadius = 18f;


    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: timerのリセットとplayerの参照取得が行われます。
    /// 待機時間の計測を開始するため、状態遷移時に初期化します。
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: 巡回または追跡への状態遷移が行われます。
    /// 環境の変化にリアルタイムで対応するため、毎フレーム判定を行います。
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("isPatroling", true);
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer < detectionAreaRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }
}
