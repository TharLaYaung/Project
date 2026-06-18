using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// 一定範囲を自律的に移動し、プレイヤーを索敵します。

public class ZombiePatrolingState : StateMachineBehaviour
{
    // 巡回時間を計測し、一定時間で待機状態へ戻すためのタイマーです。
    private float timer;

    // 巡回を終えるまでの時間（初期値10秒）。
    public float patrolingTime = 10f;

    // プレイヤーの位置を追跡するため、座標情報を保持します。
    private Transform player;

    // 自律移動を行うため、NavMeshAgentを保持します。
    private NavMeshAgent agent;

    // プレイヤーを検知する距離。
    public float detectionArea = 18f;

    // 巡回中の移動速度。
    public float patrolSpeed = 2f;

    // 巡回ルートをランダムに決定するため、ウェイポイントのリストを保持します。
    private List<Transform> waypointsList = new List<Transform>();

    
    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: agentの設定と最初の移動先が決定されます。
    /// 巡回を開始するため、必要なコンポーネントを取得し、ランダムな目的地を設定します。
  
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーをタグで検索して取得
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // アニメーターが付いているオブジェクトからNavMeshAgentを取得
        agent = animator.GetComponent<NavMeshAgent>();

        // エージェントの速度を巡回速度に設定し、タイマーをリセット
        agent.speed = patrolSpeed;
        timer = 0;

        // ---- 最初の巡回地点への移動開始 ---- //

        // エージェントのウェイポイントをタグで検索
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");

        // ウェイポイントの子供（各地点）をリストに全て格納
        foreach (Transform t in waypointCluster.transform)
        {
            waypointsList.Add(t);
        }

        // リストの中からランダムに一点を選び、移動先に設定
        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        // エラー回避：NavMesh上にある場合のみ目的地を設定
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(nextPosition);
        }
    }

    
    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: 巡回ルートの更新、音声再生、および状態遷移が行われます。
    /// 周囲の状況変化（目的地到着、時間経過、プレイヤー検知）に即座に対応します。
  
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ---- ゾンビの足音再生処理 ---- //
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
            // 遅延再生
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }

        // ---- エージェントへの到達確認と次の地点への移動 ---- //

        // 目的地までの距離が、停止範囲以下になった場合（到達した場合）
        // エラー回避：NavMesh上にあるかどうかを確認
        if (agent.isOnNavMesh && agent.remainingDistance <= agent.stoppingDistance)
        {
            // 次のランダムな地点を選んで移動開始
            var random = Random.Range(0, waypointsList.Count);
            var pos = waypointsList[random].position;
            agent.SetDestination(pos);
        }

        // ---- 待機状態(Idle State)への遷移判定 ---- //

        // 巡回時間を加算
        timer += Time.deltaTime;
        // 一定時間巡回を終えたら待機状態へ戻す
        if (timer > patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }

        // ---- 追跡状態(Chase State)への遷移判定 ---- //

        // プレイヤーとの距離を計算
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // プレイヤーが検知範囲内に入った場合
        if (distanceFromPlayer < detectionArea)
        {
            // 追跡フラグを立てて遷移
            animator.SetBool("isChasing", true);
        }
    }

    
    /// Input: animator, stateInfo, layerIndex
    /// Output: なし
    /// Side Effects: agentの移動と音声が停止します。
    /// 意図しない挙動を防ぐため、状態遷移時に初期化状態へ戻します。
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(agent.transform.position);
        }

        SoundManager.Instance.zombieChannel.Stop();
    }
}
