using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// プレイヤーの視点移動をマウス入力と連動させるためのクラス
public class MouseMovement : MonoBehaviour
{
    private const float DEFAULT_SENSITIVITY = 2f;
    private const float SENSITIVITY_MULTIPLIER = 50f;
    private const float DEFAULT_SMOOTH_SPEED = 10f;

    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private float currentXRotation = 0f;
    private float currentYRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    // 入力: なし, 出力: なし, 副作用: マウスカーソルのロック
    void Start()
    {
        // プレイヤーが誤ってゲーム画面外をクリックするのを防ぐため
        Cursor.lockState = CursorLockMode.Locked;
        currentXRotation = xRotation;
        currentYRotation = yRotation;
    }

    // 入力: なし, 出力: なし, 副作用: カメラの回転の更新
    void Update()
    {
        // プレイヤーが設定した感度設定を反映させるため
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", DEFAULT_SENSITIVITY) * SENSITIVITY_MULTIPLIER;
        float smoothSpeed = PlayerPrefs.GetFloat("SmoothSpeed", DEFAULT_SMOOTH_SPEED);

        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;

        // 視点が真上や真下を超えて反転する不自然な挙動を防ぐため
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        yRotation += mouseX;

        currentXRotation = xRotation;
        currentYRotation = yRotation;

        // プレイヤーの頭の傾き（Z軸回転）は不要なため0に固定する
        transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);
    }
}
