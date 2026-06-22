using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// ミニマップカメラの位置と回転を制御し、
/// プレイヤーを上空から追従させるスクリプトです。

public class MinimapFollow : MonoBehaviour
{
    [Header("ターゲット設定")]
    [Tooltip("カメラが追従するプレイヤーまたはオブジェクトを指定します。")]
    public Transform playerTarget;

    [Tooltip("プレイヤーからの垂直方向の高さ（距離）です。")]
    public float heightOffset = 10f;

    [Header("回転設定")]
    [Tooltip("プレイヤーのY軸回転に合わせてミニマップも回転させるかどうか。")]
    public bool rotateWithPlayer = true;

    [Header("スムージング（滑らかさ）")]
    [Tooltip("オンにすると、カメラの動きがより滑らかになります。")]
    public bool useSmoothing = false;
    public float smoothSpeed = 0.125f;

    private void Start()
    {
        // ターゲットが未設定の場合、"Player"タグの付いたオブジェクトを自動検索します。
        if (playerTarget == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTarget = playerObj.transform;
            }
            else
            {
                // Debug.LogWarning("MinimapFollow: ターゲットが未設定で、'Player'タグのオブジェクトも見つかりません。");
            }
        }
    }

    /// <summary>
    /// LateUpdateを使用することで、プレイヤーの移動完了後に
    /// カメラの位置を更新し、ガタつきを防ぎます。
    /// </summary>
    private void LateUpdate()
    {
        if (playerTarget == null) return;

        try
        {
            // プレイヤーの位置に基づいた新しい座標を計算
            Vector3 newPosition = playerTarget.position;
            newPosition.y += heightOffset;

            // カメラの位置を適用
            if (useSmoothing)
            {
                transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
            }
            else
            {
                transform.position = newPosition;
            }

            if (rotateWithPlayer)
            {
                // プレイヤーのY軸回転に追従しつつ、X軸は真下(90度)を固定
                transform.rotation = Quaternion.Euler(90f, playerTarget.eulerAngles.y, 0f);
            }
            else
            {
                // 北を固定したトップダウン表示
                transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            }
        }
        catch (System.Exception ex)
        {
            // Debug.LogError($"MinimapFollow エラー: {ex.Message}");
        }
    }
}
