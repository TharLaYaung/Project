using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 個別の武器の挙動（射撃、リロード、ADS、反動など）を制御するクラス
/// </summary>
public class Weapon : MonoBehaviour
{
    // 武器の状態管理
    public bool isActiveWeapon;       // 現在手に持っているアクティブな武器か
    public int WeaponDamage;          // 武器のダメージ量
    public Scoped scoped;              // スナイパー用のスコープ機能参照（スナイパー以外はNoneでOK）
    public Camera playerCamera;       // 弾の飛ぶ方向を計算するためのメインカメラ

    [Header("射撃設定")]
    public bool isShooting;           // 現在射撃入力中か
    public bool readyToShoot;         // 次の弾が撃てる状態か
    public bool allowReset = true;    // 射撃間隔のリセットを許可するか
    public float shootingDelay = 2f;  // 射撃間のディレイ（連射速度）

    [Header("バースト射撃")]
    public int bulletsPerBurst = 3;   // 1回のバーストで発射される弾数
    public int burstBulletLeft;       // バースト内で残り何発撃つか

    [Header("弾の拡散（ブレ）")]
    public float spreadIntensity;     // 現在の拡散度
    public float hipSpreadIntensity;  // 腰撃ち時の拡散度
    public float adsSpreadIntensity;  // ADS（サイト覗き）時の拡散度

    [Header("弾丸の設定")]
    public GameObject bulletPrefab;    // 発射される弾のプレハブ
    public Transform bulletSpawn;      // 弾が生成される銃口の位置
    public float bulletVelocity = 30;  // 弾の初速
    public float bulletPrefabLifeTime = 3f; // 弾が消えるまでの時間

    [Header("リロード設定")]
    public float reloadTime;          // リロードにかかる時間
    public int magazineSize;          // マガジンの最大容量
    public int bulletsLeft;           // 現在のマガジン内の残弾数
    public bool isReloading;          // リロード中かどうか

    [Header("スポーン時のトランスフォーム")]
    public Vector3 spawnPosition;     // 手に持った時の位置
    public Vector3 spawnRotation;     // 手に持った時の回転
    public Vector3 spawnScale;        // 手に持った時のスケール

    bool isADS;                       // 現在サイトを覗いているか（ADS中か）

    public GameObject muzzleEffect;   // 発砲時のマズルフラッシュエフェクト
    internal Animator animator;       // 武器のアニメーター

    // 武器のモデル種類
    public enum WeaponModel
    {
        Pistol1911,
        Shotgun,
        Sniper,
        LMG,
        M16
    }
    public WeaponModel thisWeaponModel;

    // 射撃モードの種類
    public enum ShootingMode
    {
        Single, // 単発
        Burst,  // 3点バーストなど
        Auto,   // フルオート
    }
    public ShootingMode currentShootingMode;

    private void Awake()
    {
        // 初期化処理
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        spreadIntensity = hipSpreadIntensity; // 初期状態は腰撃ちの精度
    }

    void Update()
    {
        if (isActiveWeapon)
        {
            // 手に持っている間は拾うための当たり判定（BoxCollider）を無効化
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider != null) boxCollider.enabled = false;

            // --- スコープ/ADS判定の修正箇所 ---
            // スナイパーモデルかつ、scopedスクリプトが参照されている場合のみ実行
            if (thisWeaponModel == WeaponModel.Sniper && scoped != null)
            {
                scoped.Scope();
            }
            else
            {
                // スナイパー以外の武器、またはスナイパーだがScopedがアタッチされていない場合
                if (Input.GetMouseButtonDown(1)) EnterADS(); // 右クリックで覗く
                if (Input.GetMouseButtonUp(1)) ExitADS();   // 右クリック離して戻す
            }

            // アウトライン（強調表示）を消す
            if (GetComponent<Outline>()) GetComponent<Outline>().enabled = false;

            // 残弾ゼロで撃とうとした時の空撃ち音
            if (bulletsLeft == 0 && isShooting)
            {
                if (SoundManager.Instance != null && SoundManager.Instance.emptyMagazineSound1911 != null)
                    SoundManager.Instance.emptyMagazineSound1911.Play();
            }

            // 射撃モードに応じた入力判定
            if (currentShootingMode == ShootingMode.Auto)
                isShooting = Input.GetKey(KeyCode.Mouse0); // 押しっぱなし
            else
                isShooting = Input.GetKeyDown(KeyCode.Mouse0); // 押した瞬間

            // 手動リロード（Rキー）
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            // 弾切れ時の自動リロード
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                Reload();
            }

            // 射撃実行
            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }
        }
    }

    /// <summary>
    /// ADS（エイム）状態に入る処理
    /// </summary>
    private void EnterADS()
    {
        if (animator != null) animator.SetTrigger("enterADS");
        isADS = true;
        if (HUBManager.Instance != null && HUBManager.Instance.crosshair != null)
            HUBManager.Instance.crosshair.SetActive(false); // 画面中央のレティクルを消す
        spreadIntensity = adsSpreadIntensity;           // 精度を上げる
    }

    /// <summary>
    /// ADS状態から抜ける処理
    /// </summary>
    private void ExitADS()
    {
        if (animator != null) animator.SetTrigger("exitADS");
        isADS = false;
        if (HUBManager.Instance != null && HUBManager.Instance.crosshair != null)
            HUBManager.Instance.crosshair.SetActive(true);  // レティクルを再表示
        spreadIntensity = hipSpreadIntensity;           // 精度を元に戻す
    }

    /// <summary>
    /// 弾を発射する実処理
    /// </summary>
    private void FireWeapon()
    {
        bulletsLeft--; // 残弾減少

        if (muzzleEffect != null) muzzleEffect.GetComponent<ParticleSystem>().Play(); // 発砲エフェクト

        // 状態に応じた反動アニメーションの再生
        if (animator != null)
        {
            // 【修正】現在の構え状態を確認。Scopedがついている場合はその内部状態も見る
            bool isAiming = isADS || (thisWeaponModel == WeaponModel.Sniper && scoped != null && scoped.IsScoped);

            if (isAiming)
                animator.SetTrigger("RECOIL_ADS");
            else
                animator.SetTrigger("RECOIL");
        }

        if (SoundManager.Instance != null) SoundManager.Instance.PlayShootingSound(thisWeaponModel); // 発砲音

        readyToShoot = false; // 次の弾を撃つまでの待機状態へ

        // 拡散を考慮した射撃方向の計算
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // 弾丸の生成と物理的な力の付与
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Bullet2 bul = bullet.GetComponent<Bullet2>();
        if (bul != null) bul.bulletDamage = WeaponDamage; // ダメージ情報の受け渡し

        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // 一定時間後に弾を消去
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        // 次の射撃までのリセットタイマー
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // バーストモード時の連続発射処理
        if (currentShootingMode == ShootingMode.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        if (animator != null) animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        int totalReservedAmmo = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);

        if (totalReservedAmmo > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = totalReservedAmmo;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(100);

        Vector3 direction = targetPoint - bulletSpawn.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0, y, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bullet != null) Destroy(bullet);
    }
}