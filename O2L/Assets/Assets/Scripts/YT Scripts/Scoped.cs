using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// スナイパーライフルなどの武器でスコープを覗き込む動作を管理するクラス

public class Scoped : MonoBehaviour
{
    // 武器のアニメーションを制御するアニメーター
    public Animator animator;

    // --- エラー解消のための追加箇所 ---
    // 現在スコープを覗いているかどうかの内部フラグ
    private bool isScoped = false;

    // 外部（Weapon.csなど）から状態を確認するためのプロパティ
    
    public bool IsScoped => isScoped;
    

    [Header("カメラ・UI設定")]
    // メインカメラの参照（FOVを変更するために使用）
    public Camera mainCamera;

    // スコープ使用時に表示するUI（黒い枠など）
    public GameObject scoped;

    // スコープを覗いた時の視野角（値が小さいほどズームされる）
    public float scopedFOV = 15f;

    // 通常時の視野角を保存しておく変数
    private float normalFOV;

    // ヒットマーカーのUI
    public GameObject hitmarker;

    // 武器自体のモデル（スコープ中は邪魔になるため非表示にする用）
    public GameObject WeaponRender;

    
    /// 開始時の初期化処理
   
    private void Start()
    {
        // 初期状態ではスコープUIとヒットマーカーを非表示にする
        if (scoped != null) scoped.SetActive(false);
        if (hitmarker != null) hitmarker.SetActive(false);

        // カメラの初期視野角を保存
        if (mainCamera != null)
        {
            normalFOV = mainCamera.fieldOfView;
        }
    }

    
    

    
    /// スコープの切り替え処理を行うメソッド
   
    public void Scope()
    {
        // マウスの右クリック（Fire2）が押された瞬間
        if (Input.GetButtonDown("Fire2"))
        {
            OnEnterScope();
        }
        // 右クリックを離した瞬間
        else if (Input.GetButtonUp("Fire2"))
        {
            OnExitScope();
        }
    }

   
    /// スコープズーム開始時の処理
   
    private void OnEnterScope()
    {
        isScoped = true;

        // アニメーターの "Scoped" パラメータを有効にして覗き込みモーションを開始
        if (animator != null) animator.SetBool("Scoped", true);

        // スコープ用のUIを表示
        if (scoped != null) scoped.SetActive(true);

        // 画面中央のレティクル（腰撃ち用）を非表示にする
        if (HUBManager.Instance != null && HUBManager.Instance.crosshair != null)
        {
            HUBManager.Instance.crosshair.SetActive(false);
        }

        // 武器のモデルを非表示にする（カメラにめり込むのを防ぐ）
        if (WeaponRender != null) WeaponRender.SetActive(false);

        // カメラの視野角を変更してズームさせる
        if (mainCamera != null) mainCamera.fieldOfView = scopedFOV;
    }

   
    /// スコープ解除時の処理
    
    private void OnExitScope()
    {
        isScoped = false;

        // アニメーションを通常状態に戻す
        if (animator != null) animator.SetBool("Scoped", false);

        // スコープUIを隠す
        if (scoped != null) scoped.SetActive(false);

        // レティクルを再表示（Weapon.csのExitADSとの整合性を保つ）
        if (HUBManager.Instance != null && HUBManager.Instance.crosshair != null)
        {
            HUBManager.Instance.crosshair.SetActive(true);
        }

        // 武器モデルを再表示する
        if (WeaponRender != null) WeaponRender.SetActive(true);

        // 視野角を元の広さに戻す
        if (mainCamera != null) mainCamera.fieldOfView = normalFOV;
    }
}