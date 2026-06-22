using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// カメラの回転（視点操作）とプレイヤーへの追従を管理するクラス
/// マウス入力に基づいた回転と、カメラシェイク機能を提供します

public class Cameracontroller : MonoBehaviour
{
    // --- 変数宣言 ---

    // 追従対象となるプレイヤーのゲームオブジェクト
    [SerializeField] private GameObject player;

    // インスペクターから調整可能なマウス感度設定
    [SerializeField] private Vector2 sensitivity;

    // 水平方向（左右）の回転角を保持する変数
    private float horizontalAngle;

    // 前フレームのプレイヤー位置を記録し、移動量を計算するための変数
    private Vector3 targetPosition;

    // カメラ自身のTransformコンポーネントへの参照
    private Transform cameraTransform;

    // マウス感度（複数の感度変数が定義されていますが、現在は下のspeedH/Vが主に使用されています）
    [SerializeField] public float mouseSensitivity = 2f;
    public float speedV = 2.0f; // 垂直方向の回転速度
    public float speedH = 2.0f; // 水平方向の回転速度

    // 現在の累積回転角
    private float xRotation = 0f; // 上下（ピッチ）
    private float yRotation = 0f; // 左右（ヨー）
    private float currentXRotation = 0f;
    private float currentYRotation = 0f;

    // カメラを揺らす演出（Camerashakeスクリプト）への参照
    public Camerashake camerashake;


   
    /// 初期化処理
   
    void Start()
    {
        // マウスカーソルを非表示にし、画面中央に固定（FPSなどで一般的）
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 初期位置をターゲット位置として記録
        this.targetPosition = this.player.transform.localPosition;
        
        currentXRotation = xRotation;
        currentYRotation = yRotation;
    }

    
    /// フレームごとの更新（入力処理と回転）
    
    void Update()
    {
        // ポーズメニューが開いていない場合のみカメラ操作を許可
        if (!PauseMenu.instance.isPaused)
        {
            float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 2f);
            float smoothSpeed = PlayerPrefs.GetFloat("SmoothSpeed", 10f);

            // マウスの移動量を取得し、回転角に加算
            // GetAxisRaw("Mouse Y") は上下、"Mouse X" は左右の動き
            xRotation -= sensitivity * Input.GetAxisRaw("Mouse Y");
            yRotation += sensitivity * Input.GetAxisRaw("Mouse X");

            // 上下の回転角度を制限（真上や真後ろを向いて画面が反転するのを防ぐ）
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            currentXRotation = xRotation;
            currentYRotation = yRotation;

            // 計算した回転角をカメラの角度（Euler角）に適用
            transform.eulerAngles = new Vector3(currentXRotation, currentYRotation, 0f);
        }
    }

    
    /// 全てのUpdateが完了した後に呼ばれる更新処理
    /// プレイヤーの移動にカメラを同期させます
    
    private void LateUpdate()
    {
        // プレイヤーの今フレームの移動量を計算し、カメラの位置に加算する（追従）
        // (現在の位置 - 前フレームの位置) = 移動した距離
        this.transform.localPosition += this.player.transform.localPosition - this.targetPosition;

        // 次のフレームのために現在の位置を保存
        this.targetPosition = this.player.transform.localPosition;

        // プレイヤーの位置を軸にして、水平方向に回転させる（三人称視点などの場合に利用）
        this.transform.RotateAround(this.player.transform.localPosition, Vector3.up, this.horizontalAngle);
    }

    
    /// 外部（武器の発砲時など）から呼び出してカメラを揺らす関数
    
    public void CameraShake()
    {
        // Camerashakeスクリプトのコルーチンを呼び出す（持続時間 0.15秒、強さ 0.4）
        StartCoroutine(camerashake.Shake(0.15f, 0.4f));
    }
}