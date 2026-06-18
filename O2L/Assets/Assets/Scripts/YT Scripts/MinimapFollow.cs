using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ミニマップのカメラをプレイヤーに追従させ、俯瞰視点を維持するクラス
public class MinimapFollow : MonoBehaviour
{
    private const float CAMERA_DOWNWARD_ANGLE = 90f;

    [Header("ターゲット設定")]
    [Tooltip("カメラが追従するプレイヤーまたはオブジェクトを指定します")]
    public Transform playerTarget;

    [Tooltip("プレイヤーからの垂直方向の高さ（距離）です")]
    public float heightOffset = 10f;

    [Header("回転設定")]
    [Tooltip("プレイヤーのY軸回転に合わせてミニマップも回転させるかどうか")]
    public bool rotateWithPlayer = true;

    [Header("スムージング（滑らかさ）")]
    [Tooltip("オンにすると、カメラの動きがより滑らかになります")]
    public bool useSmoothing = false;
    public float smoothSpeed = 0.125f;

    // 入力: なし, 出力: なし, 副作用: プレイヤーオブジェクトの検索と設定
    private void Start()
    {
        // 開発者がインスペクターで設定し忘れた場合でも自動でプレイヤーを追従対象にするため
        if (playerTarget == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTarget = playerObj.transform;
            }
        }
    }

    // 入力: なし, 出力: なし, 副作用: カメラの位置と回転の更新
    private void LateUpdate()
    {
        if (playerTarget == null) return;

        try
        {
            Vector3 newPosition = playerTarget.position;
            newPosition.y += heightOffset;

            // プレイヤーの動きに対してカメラが遅れて追従するような演出を加えるため
            if (useSmoothing)
            {
                transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
            }
            else
            {
                transform.position = newPosition;
            }

            // プレイヤーの向いている方向を常にミニマップの上方向にするため
            if (rotateWithPlayer)
            {
                transform.rotation = Quaternion.Euler(CAMERA_DOWNWARD_ANGLE, playerTarget.eulerAngles.y, 0f);
            }
            else
            {
                // 方角を固定し、絶対的な位置関係を把握しやすくするため
                transform.rotation = Quaternion.Euler(CAMERA_DOWNWARD_ANGLE, 0f, 0f);
            }
        }
        catch (System.Exception)
        {
            // 予期せぬエラーでカメラ更新が停止した場合の調査用（現在はコメントアウト）
            // Debug.LogError($"MinimapFollow エラー: {ex.Message}");
        }
    }
}
