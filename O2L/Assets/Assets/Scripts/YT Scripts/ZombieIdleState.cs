using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ゾンビの待機状態（AnimatorのIdleステート）を管理するクラス
/// 時間経過によるパトロールへの移行や、プレイヤー検知による追跡への移行を制御します

public class ZombieIdleState : StateMachineBehaviour
{
    // 待機時間を計測するためのタイマー
    float timer;

    // 待機する時間の長さ（インスペクターで設定可能）
    public float idleTime = 0f;

    // プレイヤーの座標情報
    Transform player;

    // プレイヤーを検知する範囲の半径
    public float detectionAreaRadius = 18f;


    /// 待機状態に入った瞬間に一度だけ呼ばれる

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // タイマーをリセット
        timer = 0;
        // プレイヤーをタグで検索して参照を取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    /// 待機状態の間、毎フレーム呼ばれる

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ---- パトロール状態（Patrol State）への遷移判定 ---- //

        // 経過時間を加算
        timer += Time.deltaTime;
        // 設定された待機時間を超えた場合
        if (timer > idleTime)
        {
            // パトロールフラグを立てて、パトロール状態へ遷移させる
            animator.SetBool("isPatroling", true);
        }

        /* // ---- 叫び状態（Scream State）への遷移判定（コメントアウト中） ---- //
        // プレイヤーとの距離を計算し、叫び範囲内であれば叫びアニメーションへ移行する処理
        float distanceFromPlayer1 = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer1 < screamingAreaRadius)
        {
            animator.SetBool("isScreaming", true);
        }
        */

        // ---- 追跡状態（Chase State）への遷移判定 ---- //

        // プレイヤーとの距離を計算
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // プレイヤーが検知範囲（半径18mなど）の中に入った場合
        if (distanceFromPlayer < detectionAreaRadius)
        {
            // 追跡フラグを立てて、追跡状態へ遷移させる
            animator.SetBool("isChasing", true);
        }
    }
}