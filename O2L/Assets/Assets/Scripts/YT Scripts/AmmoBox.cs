using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    //弾の量
    public int ammoAmount = 200;

    //弾の種類
    public AmmoType ammoType;


    public enum AmmoType
    {
        RifleAmmo,       //ライフル
        ShotgunAmmo,     //ショットガン
        SniperAmmo,      //スナイパー
        LmgAmmo,         //LMG
        PistolAmmo       //ピストル
    }


}
