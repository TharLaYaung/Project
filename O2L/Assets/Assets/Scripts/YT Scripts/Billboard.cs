using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 常にプレイヤーの視点に合わせた表示を行うため
public class Billboard : MonoBehaviour
{
    private const float Y_AXIS_CORRECTION = 180f;

    // 常にカメラの正面を向かせて視認性を確保するため
    public Camera mainCamera;

    // Input: なし / Output: なし / Side Effects: カメラの取得
    void Start()
    {
        // 実行時に動的にカメラを取得することでシーン依存を減らすため
        mainCamera = Camera.main;
    }

    // Input: なし / Output: なし / Side Effects: オブジェクトの回転
    void LateUpdate()
    {
        // プレイヤーから常に正面が見えるようにするため
        transform.LookAt(mainCamera.transform);
        // LookAtにより背面が向いてしまうため、正面に向き直す
        transform.Rotate(0, Y_AXIS_CORRECTION, 0);
    }
}
