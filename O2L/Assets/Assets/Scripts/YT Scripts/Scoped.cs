using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ADS（覗き込み）時の視界変化やカメラ動作を制御するため、状態と演出を管理する
public class Scoped : MonoBehaviour
{
    private const float DEFAULT_SCOPED_FOV = 15f;

    public Animator animator;

    // 外部の武器状態と同期するため、現在の覗き込み状態を内部保持する
    private bool isScoped = false;
    public bool IsScoped => isScoped;

    [Header("カメラ・UI設定")]
    public Camera mainCamera;
    public GameObject scoped;
    public float scopedFov = DEFAULT_SCOPED_FOV;
    private float normalFov;

    public GameObject hitmarker;
    public GameObject weaponRender;

    // Input: なし, Output: なし, Side Effects: UIの初期化と元のFOVを保持する
    private void Start()
    {
        if (scoped != null) scoped.SetActive(false);
        if (hitmarker != null) hitmarker.SetActive(false);

        if (mainCamera != null)
        {
            normalFov = mainCamera.fieldOfView;
        }
    }

    // Input: なし, Output: なし, Side Effects: マウス入力に応じてスコープのON/OFFを切り替える
    public void Scope()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            OnEnterScope();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            OnExitScope();
        }
    }

    // Input: なし, Output: なし, Side Effects: FOVやUIを変更し、武器モデルを非表示にする
    private void OnEnterScope()
    {
        isScoped = true;

        if (animator != null) animator.SetBool("Scoped", true);
        if (scoped != null) scoped.SetActive(true);

        if (HUBManager.Instance != null && HUBManager.Instance.crosshair != null)
        {
            HUBManager.Instance.crosshair.SetActive(false);
        }

        if (weaponRender != null) weaponRender.SetActive(false);
        if (mainCamera != null) mainCamera.fieldOfView = scopedFov;
    }

    // Input: なし, Output: なし, Side Effects: FOVやUIを元に戻し、武器モデルを再表示する
    private void OnExitScope()
    {
        isScoped = false;

        if (animator != null) animator.SetBool("Scoped", false);
        if (scoped != null) scoped.SetActive(false);

        if (HUBManager.Instance != null && HUBManager.Instance.crosshair != null)
        {
            HUBManager.Instance.crosshair.SetActive(true);
        }

        if (weaponRender != null) weaponRender.SetActive(true);
        if (mainCamera != null) mainCamera.fieldOfView = normalFov;
    }
}
