using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Weapon;

/// <summary>
/// ゲーム中のHUD（ヘッドアップディスプレイ）全般を管理するクラス
/// 弾薬数、武器アイコン、投げ物（グレネード）のUI更新を担当します
/// </summary>
public class HUBManager : MonoBehaviour
{
    // シングルトン：どこからでも HUBManager.Instance でアクセス可能
    public static HUBManager Instance { get; set; }

    [Header("弾薬UI設定")]
    public TextMeshProUGUI magazineAmmoUI; // マガジン内の残弾数テキスト
    public TextMeshProUGUI totalAmmoUI;    // 予備の総弾数テキスト
    public Image ammoTypeUI;              // 弾薬の種類を示すアイコン画像

    [Header("武器アイコン設定")]
    public Image activeWeaponUI;          // 現在使用中の武器アイコン
    public Image unActiveWeaponUI;        // 背負っている（使用中ではない）武器アイコン

    [Header("投げ物（Throwables）UI設定")]
    public Image lethalUI;                // リーサル（手榴弾など）のアイコン
    public TextMeshProUGUI lethalAmountUI; // リーサルの所持数テキスト

    public Image tacticalUI;              // タクティカル（スモーク弾など）のアイコン
    public TextMeshProUGUI tacticalAmountUI; // タクティカルの所持数テキスト

    [Header("スロット用スプライト")]
    public Sprite emptySlot;              // 何も持っていない時の空スロット用画像
    public Sprite greySlot;               // 使用不可（弾切れなど）の時のグレーアウト用画像

    public GameObject crosshair;          // 画面中央のレティクル（照準）

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

    private void Update()
    {
        // WeaponManagerから現在の武器スロットにある武器情報を取得
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        // メイン武器を装備している場合
        if (activeWeapon)
        {
            // 残弾数と予備弾薬数をテキストに反映
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            // 武器モデルに応じた弾薬アイコンと武器アイコンを表示
            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);

            // サブ武器（非アクティブ武器）がある場合はそのアイコンも表示
            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            // 武器を持っていない場合はUIをクリア（空の状態にする）
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";
            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }

        // 投げ物の個数が0になったら、アイコンをグレーアウト表示にする
        if (WeaponManager.Instance.lethalsCount <= 0)
        {
            lethalUI.sprite = greySlot;
        }
        if (WeaponManager.Instance.tacticalsCount <= 0)
        {
            tacticalUI.sprite = greySlot;
        }
    }

    /// <summary>
    /// 武器モデルに対応するスプライトをResourcesフォルダから読み込む
    /// </summary>
    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol1911:
                return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.M16:
                return Resources.Load<GameObject>("M16_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Shotgun:
                return Resources.Load<GameObject>("Shotgun_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Sniper:
                return Resources.Load<GameObject>("Sniper_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.LMG:
                return Resources.Load<GameObject>("LMG_Weapon").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    
    /// 武器モデルに対応する弾薬アイコンスプライトをResourcesフォルダから読み込む
    
    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol1911:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.M16:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Shotgun:
                return Resources.Load<GameObject>("Shotgun_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Sniper:
                return Resources.Load<GameObject>("Sniper_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.LMG:
                return Resources.Load<GameObject>("LMG_Ammo").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    
    /// 現在アクティブではない方の武器スロットを検索して返す
    
    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    
    /// 投げ物（リーサル・タクティカル）のUI表示と個数を更新する
    
    internal void UpdateThrowablesUI()
    {
        lethalAmountUI.text = $"{WeaponManager.Instance.lethalsCount}";
        tacticalAmountUI.text = $"{WeaponManager.Instance.tacticalsCount}";

        // 装備中のリーサルの種類に応じてアイコンを切り替え
        switch (WeaponManager.Instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }

        // 装備中のタクティカルの種類に応じてアイコンを切り替え
        switch (WeaponManager.Instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smoke_Grenade:
                tacticalUI.sprite = Resources.Load<GameObject>("Smoke_Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}