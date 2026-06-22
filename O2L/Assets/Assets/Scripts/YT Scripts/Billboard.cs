using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // メインカメラの参照を保存する変数
    public Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;  // シーン内の「MainCamera」タグがついたカメラを取得して保存
    }
    void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);   // オブジェクトをカメラの方向に向ける
        transform.Rotate(0, 180, 0);  // 180度回転させる
    }
}
