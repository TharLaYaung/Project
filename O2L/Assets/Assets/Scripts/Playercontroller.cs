using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public float maxVelocity = 10f;

    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float runSpeed = 2.0f;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float rotateSpeed = 500.0f;
    [SerializeField] private GameObject handGun;

    public float jumpForce = 5f;
    private bool isJumping = true;
    private Rigidbody rb;

    [SerializeField] public int pHp = 3;
    [SerializeField] public int maxHp = 10;

    private Vector3 direction = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 物理演算による意図しない回転を防ぐため固定する
        rb.freezeRotation = true;
        defaultSpeed = speed;
        // 開始時に体力を最大値で初期化する
        pHp = maxHp;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 地面等との接触時に再ジャンプ可能状態へ復帰させる
        isJumping = true;
    }

    void Update()
    {
        this.direction = MoveDirection();

        // 停止時の不要な回転処理を省くため移動入力の有無を確認する
        if (!Mathf.Approximately(this.direction.magnitude, 0.0f))
        {
            UpdateRotate(this.direction);

            if (this.animator != null)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    this.animator.SetBool("Running", true);
                    defaultSpeed = runSpeed;
                }
                else
                {
                    this.animator.SetBool("Walk", true);
                    this.animator.SetBool("Running", false);
                    defaultSpeed = speed;
                }
            }
        }
        else
        {
            if (this.animator != null)
            {
                this.animator.SetBool("Walk", false);
                this.animator.SetBool("Running", false);
            }
        }

        // フレームレートへの依存をなくすためTime.deltaTimeを使用する
        this.transform.localPosition += this.direction * defaultSpeed * Time.deltaTime;
    }

    /// 入力方向を計算し、進行方向ベクトルを返す
    /// 入力：WASDキーとスペースキー
    /// 出力：正規化された進行方向ベクトル
    /// 副作用：ジャンプ時にRigidbodyに力を加え、アニメーションフラグを変更する
    Vector3 MoveDirection()
    {
        Vector3 direction = Vector3.zero;

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

        if (Input.GetKeyDown(KeyCode.Space) && isJumping)
        {
            // 瞬間的なジャンプを実現するためImpulseで力を加える
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // 連続ジャンプを防ぐためフラグを解除する
            isJumping = false;
            this.animator.SetBool("Jump", true);
        }
        else
        {
            if (this.animator != null)
            {
                this.animator.SetBool("Jump", false);
            }
        }

        // 浮き上がりを防ぐためY軸の移動成分を破棄する
        direction.y = 0.0f;
        // 斜め移動時の加速を防ぐため正規化する
        return direction.normalized;
    }

    /// 目標の方向へプレイヤーを回転させる
    /// 入力：目標の進行方向ベクトル
    /// 副作用：プレイヤーのtransform.rotationを更新する
    void UpdateRotate(Vector3 direction)
    {
        Quaternion from = this.transform.rotation;
        Quaternion to = Quaternion.LookRotation(direction);
        // 急な向きの変更を避け、滑らかに振り向かせるためRotateTowardsを使用する
        Quaternion rotation = Quaternion.RotateTowards(from, to, this.rotateSpeed * Time.deltaTime);
        this.transform.rotation = rotation;
    }

    /// プレイヤー死亡時の処理
    /// 副作用：アタッチされている銃オブジェクトを削除する
    public void Death()
    {
        // プレイヤー消滅時に武器だけが残る不自然さを防ぐため削除する
        if (this.gameObject != null)
        {
            Destroy(this.handGun);
        }
    }
}
