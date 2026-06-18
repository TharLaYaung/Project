using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Weapon;

// プレイヤーのステータスや装備情報をリアルタイムに視覚化するためUIを一元管理する
public class HUBManager : MonoBehaviour
{
    public static HUBManager Instance { get; set; }

    private const int EMPTY_ITEM_COUNT = 0;

    [Header("弾薬UI設定")]
    // 射撃可否の判断基準となるため、マガジン内弾数を強調表示する
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("武器アイコン設定")]
    // 誤操作を防ぐため、現在アクティブな武器を視覚的に明示する
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("投げ物（Throwables）UI設定")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    [Header("スロット用スプライト")]
    // 装備がない状態をユーザーに認識させるためプレースホルダーを用意
    public Sprite emptySlot;
    // リソース枯渇状態を直感的に伝えるためグレーアウト画像を使用
    public Sprite greySlot;

    public GameObject crosshair;

    private void Awake()
    {
        // 複数シーンを跨いでもUIの整合性を保つためシングルトン化する
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
        // UIの遅延を防ぐため、毎フレームWeaponManagerの状態をポーリングして同期する
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            // 武器未装備時は残存UIが誤解を与えないよう初期状態にリセットする
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";
            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }

        // 枯渇時に使用不可であることを明確にするため、アイコンを無効化表示に切り替える
        if (WeaponManager.Instance.lethalsCount <= EMPTY_ITEM_COUNT)
        {
            lethalUI.sprite = greySlot;
        }
        if (WeaponManager.Instance.tacticalsCount <= EMPTY_ITEM_COUNT)
        {
            tacticalUI.sprite = greySlot;
        }
    }

    // 動的なメモリ確保を抑えるため、必要時のみResourcesからスプライトを取得する
    // Input: 武器モデル列挙型 Output: 対応する武器画像スプライト
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

    // 弾薬の種類を視覚的に区別させるため、専用のスプライトをロードする
    // Input: 武器モデル列挙型 Output: 対応する弾薬画像スプライト
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

    // 武器切り替え時の描画更新に利用するため、非アクティブなスロットを特定する
    // Output: 現在アクティブではない武器スロットのGameObject
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

    // プレイヤーがアイテム使用時に残数と種類を正確に把握できるようUIを同期する
    internal void UpdateThrowablesUI()
    {
        lethalAmountUI.text = $"{WeaponManager.Instance.lethalsCount}";
        tacticalAmountUI.text = $"{WeaponManager.Instance.tacticalsCount}";

        switch (WeaponManager.Instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }

        switch (WeaponManager.Instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smoke_Grenade:
                tacticalUI.sprite = Resources.Load<GameObject>("Smoke_Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
