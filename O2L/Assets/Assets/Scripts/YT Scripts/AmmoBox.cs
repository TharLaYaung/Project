using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーがフィールド上で弾薬を補充できるようにするため
public class AmmoBox : MonoBehaviour
{
    private const int DEFAULT_AMMO_AMOUNT = 200;

    // アイテムとして取得した際に増加する弾薬の数
    public int ammoAmount = DEFAULT_AMMO_AMOUNT;

    // 拾った際にどの銃の弾薬を補充するかを判定するため
    public AmmoType ammoType;

    public enum AmmoType
    {
        RifleAmmo,
        ShotgunAmmo,
        SniperAmmo,
        LmgAmmo,
        PistolAmmo
    }
}
