using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

/// プレイヤーの視線先にあるオブジェクトを検知し、状況に応じたインタラクションを実行するためのクラス
public class InteractionManager : MonoBehaviour
{
    private const float SCREEN_CENTER_RATIO = 0.5f;
    private const int HEAL_AMOUNT = 10;
    private const float BGM_DELAY = 0.1f;

    public static InteractionManager Instance { get; set; }

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Items hoveredItem = null;
    public HealItems hoveredHealItem = null;
    public Throwable hoveredThrowable = null;

    public PLAYER player = null;

    // 入力: なし, 出力: なし, 副作用: インスタンスの登録、重複破棄
    private void Awake()
    {
        // 複数シーンのロードによってマネージャーが二重に生成されるのを防ぐため
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // 入力: なし, 出力: なし, 副作用: 対象のハイライト状態更新とインタラクションの実行
    private void Update()
    {
        // 常に画面中央を基準として視線判定を行うため
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(SCREEN_CENTER_RATIO, SCREEN_CENTER_RATIO, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            // プレイヤーがすでに装備中の武器を再度拾うという不自然な動作を防ぐため
            if (objectHitByRaycast.TryGetComponent(out Weapon weapon) && !weapon.isActiveWeapon)
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }

                hoveredWeapon = weapon;
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            if (objectHitByRaycast.TryGetComponent(out AmmoBox ammoBox))
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }

                hoveredAmmoBox = ammoBox;
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

            if (objectHitByRaycast.TryGetComponent(out Throwable throwable))
            {
                hoveredThrowable = throwable;
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

            if (objectHitByRaycast.TryGetComponent(out Items item))
            {
                hoveredItem = item;
                hoveredItem.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupItem(hoveredItem);

                    // シリンジ等の重要アイテム取得はゲームクリア条件を満たすため
                    SceneManager.LoadScene("GameClear");

                    // UI操作を可能にするためカーソルの制限を解除する
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;

                    if (Gameclearbgm.Instance != null && Gameclearbgm.Instance.bgmgameclearChannel != null)
                    {
                        Gameclearbgm.Instance.bgmgameclearChannel.clip = Gameclearbgm.Instance.gameclearMusic;
                        Gameclearbgm.Instance.bgmgameclearChannel.PlayDelayed(BGM_DELAY);
                    }

                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredItem)
                {
                    hoveredItem.GetComponent<Outline>().enabled = false;
                }
            }

            if (objectHitByRaycast.TryGetComponent(out HealItems healItem))
            {
                if (hoveredHealItem)
                {
                    hoveredHealItem.GetComponent<Outline>().enabled = false;
                }

                hoveredHealItem = healItem;
                hoveredHealItem.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // プレイヤーの生存率を上げるために体力を回復させる
                    WeaponManager.Instance.PickupItem(hoveredHealItem);
                    if (this.player != null)
                    {
                        this.player.Heal(HEAL_AMOUNT);
                    }
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredHealItem)
                {
                    hoveredHealItem.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
