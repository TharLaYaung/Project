using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// ゾンビの巡回状態（AnimatorのPatrolステート）を管理するクラス
/// ウェイポイント（移動目標地点）をランダムに巡り、プレイヤーを検知すると追跡に移行します

public class ZombiePatrolingState : StateMachineBehaviour
{
    // 巡回時間を計測するためのタイマー
    float timer;

    // 巡回を続ける時間の長さ（デフォルト10秒）
    public float patrolingTime = 10f;

    // プレイヤーの座標情報
    Transform player;

    // ナビゲーション（自動移動）を制御するエージェント
    NavMeshAgent agent;

    // プレイヤーを検知する範囲の半径
    public float detectionArea = 18f;

    // 巡回時の移動速度
    public float patrolSpeed = 2f;

    // 巡回地点（ウェイポイント）を格納するリスト
    List<Transform> waypointsList = new List<Transform>();

    
    /// 巡回状態に入った瞬間に一度だけ呼ばれる（初期化処理）
  
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーをタグで検索して参照を取得
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // アニメーターが付いているオブジェクトからNavMeshAgentを取得
        agent = animator.GetComponent<NavMeshAgent>();

        // エージェントの速度を巡回用に設定し、タイマーをリセット
        agent.speed = patrolSpeed;
        timer = 0;

        // ---- 最初の巡回地点への移動開始 ---- //

        // ウェイポイントの親オブジェクトをタグで検索
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");

        // 親オブジェクトの子要素（各地点）をリストにすべて追加
        foreach (Transform t in waypointCluster.transform)
        {
            waypointsList.Add(t);
        }

        // リストの中からランダムに一つ地点を選び、移動目標に設定
        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
        // エラー防止：NavMesh上にいる場合のみ目的地を設定する
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(nextPosition);
        }
    }

    
    /// 巡回状態の間、毎フレーム呼ばれる
  
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ---- ゾンビの環境音（足音など）の再生 ---- //
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
            // 少し遅らせて再生（足音の重なり防止など）
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }

        // ---- ウェイポイントへの到着確認と次の地点への移動 ---- //

        // 目的地までの残り距離が、停止距離以下になった（到着した）場合
        // エラー防止：NavMesh上にいるかどうかも確認する
        if (agent.isOnNavMesh && agent.remainingDistance <= agent.stoppingDistance)
        {
            // 次のランダムな地点を選択して移動開始
            var random = Random.Range(0, waypointsList.Count);
            var pos = waypointsList[random].position;
            agent.SetDestination(pos);
        }

        // ---- 待機状態（Idle State）への遷移判定 ---- //

        // 巡回時間を加算
        timer += Time.deltaTime;
        // 一定時間巡回を続けたら待機状態に戻る
        if (timer > patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }

        // ---- 追跡状態（Chase State）への遷移判定 ---- //

        // プレイヤーとの距離を計算
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // プレイヤーが検知範囲内に入った場合
        if (distanceFromPlayer < detectionArea)
        {
            // 追跡フラグを立てて遷移させる
            animator.SetBool("isChasing", true);
        }
    }

    
    /// 巡回状態から別の状態へ移る直前に呼ばれる
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // エージェントをその場に停止させる（現在地を目的地に設定）
        // エラー防止：NavMesh上にいる場合のみ目的地を設定する
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(agent.transform.position);
        }

        // 巡回時の音声を停止
        SoundManager.Instance.zombieChannel.Stop();
    }
}