using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// マウスの動きに合わせてカメラやオブジェクトの回転（視点移動）を制御するクラス

public class MouseMovement : MonoBehaviour
{
    // マウス感度
    public float mouseSensitivity = 100f;

    // 現在の回転角（X軸とY軸）
    float xRotation = 0f;
    float yRotation = 0f;
    private float currentXRotation = 0f;
    private float currentYRotation = 0f;

    // 上下方向の回転制限（クランプ）
    // 真上や真後ろを向こうとして画面が反転するのを防ぎます
    public float topClamp = -90f;
    public float bottomClamp = 90f;

    
    /// ゲーム開始時の初期化処理
    
    void Start()
    {
        // マウスカーソルをゲーム画面内に固定し、非表示にする
        Cursor.lockState = CursorLockMode.Locked;
        currentXRotation = xRotation;
        currentYRotation = yRotation;
    }

    
    /// 毎フレーム実行される回転処理
    
    void Update()
    {
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 2f) * 50f;
        float smoothSpeed = PlayerPrefs.GetFloat("SmoothSpeed", 10f);

        // マウスの移動量を取得し、感度とフレーム時間を掛けて調整する
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        // マウスの上下の動き（Y）に合わせて、X軸周りの回転を計算（符号は反転させる）
        xRotation -= mouseY;

        // 上下の視界が制限角度（-90度～90度など）を超えないように固定する
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // マウスの左右の動き（X）に合わせて、Y軸周りの回転を加算
        yRotation += mouseX;

        currentXRotation = xRotation;
        currentYRotation = yRotation;

        // 計算した回転角を Quaternion に変換してオブジェクトに適用する
        // Z軸は回転させないため 0f を指定
        transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);
    }
}