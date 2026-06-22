using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;


/// ゲーム内のすべての音響エフェクト（SE）とBGMを一括管理するクラス
/// シングルトンパターンにより、どこからでも SoundManager.Instance を通じて音を再生できます

public class SoundManager : MonoBehaviour
{
    // 静的プロパティ：他のスクリプトからアクセスするための窓口
    public static SoundManager Instance { get; set; }

    [Header("射撃用チャンネル")]
    public AudioSource ShootingChannel;

    [Header("1911（ピストル）用設定")]
    public AudioClip P1911Shot;              // 発砲音
    public AudioSource reloadingSound1911;     // リロード音用ソース
    public AudioSource emptyMagazineSound1911; // 弾切れ音用ソース

    [Header("スナイパー用設定")]
    public AudioClip SnipShot;
    public AudioSource reloadingSoundSnip;
    public AudioSource emptyMagazineSoundSnip;

    [Header("M16（ライフル）用設定")]
    public AudioClip M16Shot;
    public AudioSource reloadingSoundM16;
    public AudioSource emptyMagazineSoundM16;

    [Header("ショットガン用設定")]
    public AudioClip ShotgunShot;
    public AudioSource reloadingSoundShotgun;
    public AudioSource emptyMagazineSoundShotgun;

    [Header("投げ物（グレネード）")]
    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    [Header("ゾンビ用SE")]
    public AudioClip zombieWalking; // 歩行音
    public AudioClip zombieChase;   // 追跡時の咆哮
    public AudioClip zombieAttack;  // 攻撃音
    public AudioClip zombieHurt;    // ダメージ音
    public AudioClip zombieDeath;   // 死亡音
    public AudioSource zombieChannel;  // ゾンビ用チャンネル1
    public AudioSource zombieChannel2; // ゾンビ用チャンネル2

    [Header("プレイヤー用SE")]
    public AudioSource playerChannel;
    public AudioClip playerHurt;    // ダメージ音
    public AudioClip playerDie;     // 死亡音

    [Header("NPC・ダイアログ")]
    public AudioSource npcChannel;
    public AudioClip npcTalk;

    [Header("BGM設定")]
    public AudioSource bgmChannel;
    public AudioClip gameClearMusic;

    
    /// インスタンスの初期化とシングルトンの確立
    
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

    
    /// 装備している武器モデルに応じて、適切な射撃音を再生する
    
    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(P1911Shot); break;
            case WeaponModel.M16:
                ShootingChannel.PlayOneShot(M16Shot); break;
            case WeaponModel.Shotgun:
                ShootingChannel.PlayOneShot(ShotgunShot); break;
            case WeaponModel.Sniper:
                ShootingChannel.PlayOneShot(SnipShot); break;
        }
    }

    
    /// 装備している武器モデルに応じて、適切なリロード音を再生する
    
    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                reloadingSound1911.Play(); break;
            case WeaponModel.M16:
                reloadingSoundM16.Play(); break;
            case WeaponModel.Shotgun:
                reloadingSoundShotgun.Play(); break;
            case WeaponModel.Sniper:
                reloadingSoundSnip.Play(); break;
        }
    }
}