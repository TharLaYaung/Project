using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


/// プレイヤーの武装や所持アイテムの状況を追跡・更新し、システム全体で共有できるようにします。
public class WeaponManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static WeaponManager Instance { get; set; }

    [Header("武器スロットの設定")]
    public List<GameObject> weaponSlots;     // 武器を格納するスロットのリスト
    public GameObject activeWeaponSlot;      // 現在選択されているアクティブなスロット

    [Header("アイテム(回復薬など)")]
    public int totalItem = 0;                // 所持しているアイテムの総数

    [Header("弾薬管理")]
    public int totalRifleAmmo = 0;           // ライフル弾の総数
    public int totalPistolAmmo = 0;          // ピストル弾の総数
    public int totalShotgunAmmo = 0;         // ショットガン弾の総数
    public int totalSniperAmmo = 0;          // スナイパー弾の総数
    public int totalLmgAmmo = 0;             // LMG弾の総数

    [Header("投擲物の発射設定")]
    public float throwForce = 5f;            // 基本の投擲力
    public float tacticalThrowForce = 5f;    // タクティカル用の投擲力
    public GameObject grenadePrefab;         // 投擲弾のプレハブ
    public GameObject throwableSpawn;        // 投擲物を生成する場所（プレイヤーの視点など）
    public float forceMultiplier = 1;        // 長押しなどによる力の倍率
    public float forceMultiplierLimit = 2f;  // 力の最大倍率

    [Header("レサール(殺傷用投擲物)")]
    public int maxLethals = 4;               // 最大所持数
    public int lethalsCount = 0;             // 現在の所持数
    public Throwable.ThrowableType equippedLethalType; // 装備中の種類

    [Header("タクティカル(補助用投擲物)")]
    public int maxTacticals = 3;             // 最大所持数
    public int tacticalsCount = 0;           // 現在の所持数
    public Throwable.ThrowableType equippedTacticalType; // 装備中の種類
    public GameObject smokeGrenadePrefab;    // スモーク弾のプレハブ

    /// Input: なし
    /// Output: なし
    /// Side Effects: Instanceに自身を登録し、重複があれば破棄します。
    /// シングルトンパターンを実現するため、起動時にチェックを行います。
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// Input: なし
    /// Output: なし
    /// Side Effects: スロット切り替えや投擲処理が行われます。
    /// プレイヤーの入力を即座に反映させるため、毎フレーム監視します。
    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
                weaponSlot.SetActive(true);
            else
                weaponSlot.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchActiveSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchActiveSlot(1);

        if (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.T))
        {
            forceMultiplier += Time.deltaTime;
            if (forceMultiplier > forceMultiplierLimit)
                forceMultiplier = forceMultiplierLimit;
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (lethalsCount > 0) ThrowLethal();
            forceMultiplier = 1; 
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (tacticalsCount > 0) ThrowTactical();
            forceMultiplier = 1; 
        }
    }


    /// Input: pickedupWeapon
    /// Output: なし
    /// Side Effects: スロットに武器が追加されます。
    /// 取得した武器を適切に装備状態にするため、内部関数を呼び出します。
    public void PickupWeapon(GameObject pickedupWeapon)
    {
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

   
    /// Input: pickedupWeapon
    /// Output: なし
    /// Side Effects: 現在の武器が破棄され、新しい武器が設定されます。
    /// インベントリの枠を管理するため、古い武器を捨てて新しい武器と入れ替えます。
    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);

        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedupWeapon.transform.localScale = new Vector3(weapon.spawnScale.x, weapon.spawnScale.y, weapon.spawnScale.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    /// Input: ammo (弾薬ボックス)
    /// Output: なし
    /// Side Effects: 対応する弾薬の総数が増加します。
    /// 弾切れを防ぐため、拾った弾薬をリソースとして追加します。
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

    /// Input: item (通常アイテム)
    /// Output: なし
    /// Side Effects: totalItemが増加します。
    /// プレイヤーの生存率を高めるため、拾ったアイテムを所持品に追加します。
    internal void PickupItem(Items item)
    {
        if (item.itemsType == Items.ItemsType.Syringe)
            totalItem += item.itemAmount;
    }

    /// Input: item (回復アイテム)
    /// Output: なし
    /// Side Effects: totalItemが増加します。
    /// 回復手段を提供するため、専用のアイテムを所持数に加算します。
    internal void PickupItem(HealItems item)
    {
        if (item.healitemsType == HealItems.HealItemsType.Medkit)
            totalItem += item.itemAmount;
    }

    /// Input: pickedupWeapon
    /// Output: なし
    /// Side Effects: 現在の武器がフィールドにドロップされます。
    /// 新しい武器を装備するスペースを確保するため、古い武器を手放します。
    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;
        }
    }

    /// Input: slotNumber
    /// Output: なし
    /// Side Effects: アクティブな武器が切り替わります。
    /// 状況に応じて適切な武器を使用できるようにするため、装備を切り替えます。
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

    /// Input: bulletsToDecrease, thisWeaponModel
    /// Output: なし
    /// Side Effects: 指定された武器の弾薬が減少します。
    /// リロード時に消費した弾薬を正確に反映するため、総弾数から差し引きます。
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

    /// Input: thisWeaponModel
    /// Output: 残弾数 (int)
    /// 武器ごとに適切なリロードを行えるよう、現在の残弾数を取得します。
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

    /// Input: throwable
    /// Output: なし
    /// Side Effects: 投擲物の種類に応じて取得処理を分岐します。
    /// アイテムの性質に合わせた管理を行うため、種類別に振り分けます。
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

    /// Input: tactical
    /// Output: なし
    /// Side Effects: タクティカル装備が更新されます。
    /// 補助アイテムを上限内で管理するため、取得時にチェックと更新を行います。
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

    /// Input: lethal
    /// Output: なし
    /// Side Effects: レサール装備が更新されます。
    /// 攻撃アイテムを上限内で管理するため、取得時にチェックと更新を行います。
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

    /// Input: なし
    /// Output: なし
    /// Side Effects: 投擲物が生成され、発射されます。
    /// 敵にダメージを与えるため、殺傷用の投擲物をプレイヤーの視点方向に投げます。
    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab(equippedLethalType);
        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.VelocityChange);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;
        lethalsCount -= 1;

        if (lethalsCount <= 0) equippedLethalType = Throwable.ThrowableType.None;

        HUBManager.Instance.UpdateThrowablesUI();
    }

    /// Input: なし
    /// Output: なし
    /// Side Effects: タクティカル投擲物が生成され、発射されます。
    /// 視界の遮断や戦況のコントロールのため、補助用の投擲物を投げます。
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

    /// Input: throwableType
    /// Output: GameObject (対応するプレハブ)
    /// 正しいオブジェクトを生成するため、種類に対応したプレハブを返します。
    private GameObject GetThrowablePrefab(Throwable.ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwable.ThrowableType.Grenade: return grenadePrefab;
            case Throwable.ThrowableType.Smoke_Grenade: return smokeGrenadePrefab;
            default: return new GameObject();
        }
    }
}
