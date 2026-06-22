using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

/// プレイヤーとオブジェクト（武器、アイテムなど）の相互作用を管理するクラス
/// 画面中央からのレイキャストを使用して、対象を特定しインタラクションを実行します

public class InteractionManager : MonoBehaviour
{
    // シングルトン：InteractionManager.Instance でどこからでもアクセス可能
    public static InteractionManager Instance { get; set; }

    // 現在視界（レイ）に入っている各カテゴリのオブジェクトを一時保存する変数
    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Items hoveredItem = null;
    public HealItems hoveredHealItem = null;
    public Throwable hoveredThrowable = null;

    // プレイヤーへの参照（回復処理などで使用）
    public PLAYER player = null;

    private void Awake()
    {
        // インスタンスの重複チェック
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
        // 画面中央（0.5, 0.5）から前方にレイ（光線）を飛ばす
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // 何らかのオブジェクトにレイが当たった場合
        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            // --- 武器（Weapon）の判定 ---
            // 当たったオブジェクトがWeaponを持ち、かつ「装備中ではない」場合
            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                // 直前に見ていた武器のアウトラインを消す
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }

                // 新しく見た武器を保持し、アウトラインを有効にする
                hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                // Eキーが押されたら武器を拾う
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                // 何も見ていない、あるいは武器以外を見ている場合はアウトラインを消す
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            // --- 弾薬箱（AmmoBox）の判定 ---
            if (objectHitByRaycast.GetComponent<AmmoBox>())
            {
                if (hoveredAmmoBox)
                {
                    // ※修正箇所：元のコードではhoveredWeaponのアウトラインを操作していたが、本来はhoveredAmmoBox
                    // hoveredWeapon.GetComponent<Outline>().enabled = false; 
                }

                hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }

            // --- 投げ物（Throwable）の判定 ---
            if (objectHitByRaycast.GetComponent<Throwable>())
            {
                hoveredThrowable = objectHitByRaycast.gameObject.GetComponent<Throwable>();
                hoveredThrowable.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupThrowable(hoveredThrowable);
                }
            }
            else
            {
                if (hoveredThrowable)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }
            }

            // --- 重要アイテム（Items）の判定 ---
            if (objectHitByRaycast.GetComponent<Items>())
            {
                hoveredItem = objectHitByRaycast.gameObject.GetComponent<Items>();
                hoveredItem.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupItem(hoveredItem);

                    // クリア条件を満たしたとして「GameClear」シーンへ遷移
                    SceneManager.LoadScene("GameClear");

                    // カーソルの表示と設定
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;

                    // クリアBGMのセットアップと再生
                    Gameclearbgm.Instance.bgmgameclearChannel.clip = Gameclearbgm.Instance.gameclearMusic;
                    Gameclearbgm.Instance.bgmgameclearChannel.PlayDelayed(0.1f);

                    Destroy(objectHitByRaycast.gameObject);
                }
            }

            // --- 回復アイテム（HealItems）の判定 ---
            if (objectHitByRaycast.GetComponent<HealItems>())
            {
                if (hoveredHealItem)
                {
                    hoveredHealItem.GetComponent<Outline>().enabled = false;
                }

                hoveredHealItem = objectHitByRaycast.gameObject.GetComponent<HealItems>();
                hoveredHealItem.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // アイテム取得と同時にプレイヤーの体力を10回復させる
                    WeaponManager.Instance.PickupItem(hoveredHealItem);
                    this.player.Heal(10);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                // 何も見ていないときのアウトライン消去処理
                if (hoveredHealItem)
                {
                    hoveredHealItem.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}