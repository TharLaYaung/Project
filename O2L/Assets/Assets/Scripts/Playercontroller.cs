using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;


/// プレイヤーの移動、回転、アニメーション、ジャンプ、およびステータスを管理するクラス

public class Playercontroller : MonoBehaviour
{
    // --- 変数宣言（Inspectorで設定可能） ---

    // Rigidbodyの速度制限用（現状のコードでは計算に使用されていないが定義済み）
    public float maxVelocity = 10f;

    // キャラクターのアニメーションを制御するAnimator
    [SerializeField] private Animator animator;

    // 通常時の移動速度
    [SerializeField] private float speed = 1.0f;

    // ダッシュ（Shiftキー）時の移動速度
    [SerializeField] private float Runspeed = 2.0f;

    // 現在適用されている移動速度（speed または Runspeed が代入される）
    [SerializeField] private float Defaultspeed;

    // キャラクターが振り向く際の回転速度
    [SerializeField] private float rotateSpeed = 500.0f;

    // プレイヤーが装備している銃のオブジェクト（死亡時に破棄するため）
    [SerializeField] private GameObject HandGun;

    // ジャンプした時に加える力の強さ
    public float jumpForce = 5f;

    // 地面に接地しているか（ジャンプ可能か）の判定フラグ
    private bool isJumping = true;

    // 物理演算コンポーネントへの参照
    private Rigidbody rb;

    // 現在のプレイヤー体力
    [SerializeField] public int PHp = 3;

    // 最大体力
    [SerializeField] public int MaxHp = 10;

    // 現在のフレームでの移動方向ベクトル
    private Vector3 direction = Vector3.zero;

    
    /// 初期化処理
   
    void Start()
    {
        // 自身のRigidbodyを取得
        rb = GetComponent<Rigidbody>();

        // 物理演算による勝手な回転（衝突で倒れるなど）を防ぐ
        rb.freezeRotation = true;

        // 初期速度を通常速度に設定
        Defaultspeed = speed;

        // 体力を全回復状態で開始
        PHp = MaxHp;
    }

    
    /// 衝突検知（地面に着いた時の判定）
    
    void OnCollisionEnter(Collision collision)
    {
        // 何らかのオブジェクトに触れたら「ジャンプ可能」状態に戻す
        isJumping = true;
    }

    
    /// フレームごとの更新処理
    
    void Update()
    {
        // 入力に基づいて移動方向を計算
        this.direction = MoveDirection();

        // 移動入力がある場合（ベクトルの大きさがほぼ0ではない場合）
        if (!Mathf.Approximately(this.direction.magnitude, 0.0f))
        {
            // 移動方向を向くように回転させる
            UpdateRotate(this.direction);

            if (this.animator != null)
            {
                // Shiftキーが押されている間は「走り」状態
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.animator.SetBool("Running", true);
                    Defaultspeed = Runspeed; // 速度をダッシュ用に変更
                }
                else
                {
                    // 通常移動（歩き）状態
                    this.animator.SetBool("Walk", true);
                    this.animator.SetBool("Running", false);
                    Defaultspeed = speed; // 速度を通常用に変更
                }
            }
        }
        else
        {
            // 移動入力がない場合はアニメーションを停止（待機状態）
            if (this.animator != null)
            {
                this.animator.SetBool("Walk", false);
                this.animator.SetBool("Running", false);
            }
        }

        // transform.localPosition を直接書き換えて移動（フレームレートに依存しないよう Time.deltaTime を掛ける）
        this.transform.localPosition += this.direction * Defaultspeed * Time.deltaTime;
    }

    
    /// 入力キーをチェックして移動方向を決定するメソッド
    
    /// <returns>正規化された移動方向ベクトル</returns>
    Vector3 MoveDirection()
    {
        Vector3 direction = Vector3.zero;

        // メインカメラの向きを基準にして移動方向を決定（カメラが向いている方が「前」になる）
        if (Input.GetKey(KeyCode.W))
        {
            direction += Quaternion.Euler(Camera.main.transform.eulerAngles) * Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Quaternion.Euler(Camera.main.transform.eulerAngles) * Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Quaternion.Euler(Camera.main.transform.eulerAngles) * Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Quaternion.Euler(Camera.main.transform.eulerAngles) * Vector3.right;
        }

        // ジャンプ処理：Spaceキーが押され、かつ接地している場合
        if (Input.GetKeyDown(KeyCode.Space) && isJumping)
        {
            // 上方向に瞬間的な力を加える
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false; // 空中にいる間はジャンプ不可にする
            this.animator.SetBool("Jump", true);
        }
        else
        {
            // ジャンプ中でない場合、アニメーターのJumpフラグを下ろす
            if (this.animator != null)
            {
                this.animator.SetBool("Jump", false);
            }
        }

        // キャラクターが浮き上がらないよう、水平方向の移動成分のみを抽出
        direction.y = 0.0f;

        // ベクトルの長さを1にする（斜め移動で速くならないようにするため）
        return direction.normalized;
    }

    
    /// キャラクターを指定した方向へ滑らかに向かせるメソッド
   
    void UpdateRotate(Vector3 direction)
    {
        // 現在の回転状態
        Quaternion from = this.transform.rotation;

        // 指定方向を向いた時の回転状態を計算
        Quaternion to = Quaternion.LookRotation(direction);

        // rotateSpeedの速度で、現在の回転を目標の回転へ近づける
        Quaternion rotation = Quaternion.RotateTowards(from, to, this.rotateSpeed * Time.deltaTime);

        // 計算結果を反映
        this.transform.rotation = rotation;
    }

    
    /// プレイヤー死亡時の処理
    
    public void Death()
    {
        // プレイヤーが存在する場合、装備している銃をシーンから削除する
        if (this.gameObject != null)
        {
            Destroy(this.HandGun);
        }
    }
}