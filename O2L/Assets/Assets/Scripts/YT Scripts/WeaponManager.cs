using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


/// 武器、弾薬、投擲物などのプレイヤーの装備全体を管理するマネージャークラス
/// シングルトンパターンを採用し、武器の持ち替えやアイテムの拾得を制御します

public class WeaponManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static WeaponManager Instance { get; set; }

    [Header("武器スロットの設定")]
    public List<GameObject> weaponSlots;     // 武器を格納するスロット（1番、2番など）
    public GameObject activeWeaponSlot;      // 現在選択されているアクティブなスロット

    [Header("アイテム（回復薬など）")]
    public int totalItem = 0;                // 所持しているアイテムの総数

    [Header("弾薬管理")]
    public int totalRifleAmmo = 0;           // ライフル弾の総数
    public int totalPistolAmmo = 0;          // ピストル弾の総数
    public int totalShotgunAmmo = 0;         // ショットガン弾の総数
    public int totalSniperAmmo = 0;          // スナイパー弾の総数
    public int totalLmgAmmo = 0;             // LMG弾の総数

    [Header("投擲物の共通設定")]
    public float throwForce = 5f;            // 基本の投げる力
    public float tacticalThrowForce = 5f;    // タクティカル用の投げる力
    public GameObject grenadePrefab;         // 手榴弾のプレハブ
    public GameObject throwableSpawn;        // 投擲物を生成する位置（プレイヤーの手元など）
    public float forceMultiplier = 1;        // 長押しなどによる力の倍率
    public float forceMultiplierLimit = 2f;  // 力の最大倍率

    [Header("リーサル（殺傷用投擲物）")]
    public int maxLethals = 4;               // 最大所持数
    public int lethalsCount = 0;             // 現在の所持数
    public Throwable.ThrowableType equippedLethalType; // 装備中の種類

    [Header("タクティカル（補助用投擲物）")]
    public int maxTacticals = 3;             // 最大所持数
    public int tacticalsCount = 0;           // 現在の所持数
    public Throwable.ThrowableType equippedTacticalType; // 装備中の種類
    public GameObject SmokeGrenadePrefab;    // スモーク弾のプレハブ

    private void Awake()
    {
        // シングルトンの初期化処理
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // 最初のスロット（通常は1番武器）をアクティブに設定
        activeWeaponSlot = weaponSlots[0];

        // 投擲物の初期状態を「なし」に設定
        equippedLethalType = Throwable.ThrowableType.None;
        equippedTacticalType = Throwable.ThrowableType.None;
    }

    private void Update()
    {
        // スロットの有効/無効を更新（アクティブなスロットのみ表示）
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
                weaponSlot.SetActive(true);
            else
                weaponSlot.SetActive(false);
        }

        // 数字キー 1 と 2 で武器スロットを切り替え
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchActiveSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchActiveSlot(1);

        // GキーまたはTキーを押している間、投げる力を溜める
        if (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.T))
        {
            forceMultiplier += Time.deltaTime;
            if (forceMultiplier > forceMultiplierLimit)
                forceMultiplier = forceMultiplierLimit;
        }

        // Gキーを離した時にリーサル投擲物を投げる
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (lethalsCount > 0) ThrowLethal();
            forceMultiplier = 1; // 倍率をリセット
        }

        // Tキーを離した時にタクティカル投擲物を投げる
        if (Input.GetKeyUp(KeyCode.T))
        {
            if (tacticalsCount > 0) ThrowTactical();
            forceMultiplier = 1; // 倍率をリセット
        }
    }

    
    /// 武器を拾った際のメイン処理
   
    public void PickupWeapon(GameObject pickedupWeapon)
    {
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

   
    /// 拾った武器を現在のアクティブなスロットに追加し、座標や回転を調整
    
    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        // すでに武器を持っている場合は捨てる
        DropCurrentWeapon(pickedupWeapon);

        // 武器をスロットの子要素に設定
        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        // 武器固有の設定（位置、スケール、回転）を反映
        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedupWeapon.transform.localScale = new Vector3(weapon.spawnScale.x, weapon.spawnScale.y, weapon.spawnScale.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    
    /// 弾薬箱を拾った際、種類に応じて総弾数を加算
   
    internal void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo: totalPistolAmmo += ammo.ammoAmount; break;
            case AmmoBox.AmmoType.RifleAmmo: totalRifleAmmo += ammo.ammoAmount; break;
            case AmmoBox.AmmoType.ShotgunAmmo: totalShotgunAmmo += ammo.ammoAmount; break;
            case AmmoBox.AmmoType.SniperAmmo: totalSniperAmmo += ammo.ammoAmount; break;
            case AmmoBox.AmmoType.LmgAmmo: totalLmgAmmo += ammo.ammoAmount; break;
        }
    }

    
    /// 一般アイテム（注射器など）を拾った際の処理
   
    internal void PickupItem(Items item)
    {
        if (item.itemsType == Items.ItemsType.Syringe)
            totalItem += item.itemAmount;
    }

   
    /// 回復アイテム（メディキットなど）を拾った際の処理（オーバーロード）
    
    internal void PickupItem(HealItems item)
    {
        if (item.healitemsType == HealItems.HealItemsType.Medkit)
            totalItem += item.itemAmount;
    }

   
    /// 現在装備している武器を、新しく拾う武器があった場所に置く（交換処理）
   
    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            // 拾った武器が置いてあった場所に、現在の武器を配置
            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;
        }
    }

   
    /// スロットを切り替える際のロジック。旧武器を非アクティブにし、新武器をアクティブにする
   
    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    
    /// リロード等で弾薬を使用した際、総弾数から減算する
    
    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M16: totalRifleAmmo -= bulletsToDecrease; break;
            case Weapon.WeaponModel.Pistol1911: totalPistolAmmo -= bulletsToDecrease; break;
            case Weapon.WeaponModel.Shotgun: totalShotgunAmmo -= bulletsToDecrease; break;
            case Weapon.WeaponModel.Sniper: totalSniperAmmo -= bulletsToDecrease; break;
            case Weapon.WeaponModel.LMG: totalLmgAmmo -= bulletsToDecrease; break;
        }
    }

   
    /// 特定の武器モデルに対応する現在の所持弾数を返す
   
    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M16: return totalRifleAmmo;
            case Weapon.WeaponModel.Pistol1911: return totalPistolAmmo;
            case Weapon.WeaponModel.Shotgun: return totalShotgunAmmo;
            case Weapon.WeaponModel.Sniper: return totalSniperAmmo;
            case Weapon.WeaponModel.LMG: return totalLmgAmmo;
            default: return 0;
        }
    }

    
    /// 投擲アイテムを拾った際の分岐（リーサルかタクティカルか）
   
    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickupThrowableAsLethal(Throwable.ThrowableType.Grenade);
                break;
            case Throwable.ThrowableType.Smoke_Grenade:
                PickupThrowableAsTactical(Throwable.ThrowableType.Smoke_Grenade);
                break;
        }
    }

   
    /// タクティカル投擲物を拾う処理。上限チェックとUI更新を行う
    
    private void PickupThrowableAsTactical(Throwable.ThrowableType tactical)
    {
        if (equippedTacticalType == tactical || equippedTacticalType == Throwable.ThrowableType.None)
        {
            equippedTacticalType = tactical;
            if (tacticalsCount < maxTacticals)
            {
                tacticalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUBManager.Instance.UpdateThrowablesUI();
            }
        }
    }

    /// リーサル投擲物を拾う処理。上限チェックとUI更新を行う
   
    private void PickupThrowableAsLethal(Throwable.ThrowableType lethal)
    {
        if (equippedLethalType == lethal || equippedLethalType == Throwable.ThrowableType.None)
        {
            equippedLethalType = lethal;
            if (lethalsCount < maxLethals)
            {
                lethalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUBManager.Instance.UpdateThrowablesUI();
            }
        }
    }

    
    /// リーサル投擲物を生成し、カメラの前方方向へ力を加えて飛ばす
   
    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab(equippedLethalType);
        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        // 溜めた力（forceMultiplier）を乗せて発射
        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.VelocityChange);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;
        lethalsCount -= 1;

        if (lethalsCount <= 0) equippedLethalType = Throwable.ThrowableType.None;

        HUBManager.Instance.UpdateThrowablesUI();
    }

   
    /// タクティカル投擲物を生成し、飛ばす処理
   
    private void ThrowTactical()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(equippedTacticalType);
        GameObject throwable = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (tacticalThrowForce * forceMultiplier), ForceMode.VelocityChange);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;
        tacticalsCount -= 1;

        if (tacticalsCount <= 0) equippedTacticalType = Throwable.ThrowableType.None;

        HUBManager.Instance.UpdateThrowablesUI();
    }

   
    /// 投擲物のタイプから対応するプレハブを返すユーティリティ
   
    private GameObject GetThrowablePrefab(Throwable.ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwable.ThrowableType.Grenade: return grenadePrefab;
            case Throwable.ThrowableType.Smoke_Grenade: return SmokeGrenadePrefab;
            default: return new GameObject();
        }
    }
}