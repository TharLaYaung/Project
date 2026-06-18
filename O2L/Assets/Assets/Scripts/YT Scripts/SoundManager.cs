using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

// 全体的な音響（BGM・SE）を一元管理するため、シングルトンとしてアクセスを提供する
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    [Header("射撃用チャンネル")]
    [UnityEngine.Serialization.FormerlySerializedAs("ShootingChannel")]
    public AudioSource shootingChannel;

    [Header("1911（ピストル）用設定")]
    [UnityEngine.Serialization.FormerlySerializedAs("P1911Shot")]
    public AudioClip p1911Shot;
    public AudioSource reloadingSound1911;
    public AudioSource emptyMagazineSound1911;

    [Header("スナイパー用設定")]
    [UnityEngine.Serialization.FormerlySerializedAs("SnipShot")]
    public AudioClip snipShot;
    public AudioSource reloadingSoundSnip;
    public AudioSource emptyMagazineSoundSnip;

    [Header("M16（ライフル）用設定")]
    [UnityEngine.Serialization.FormerlySerializedAs("M16Shot")]
    public AudioClip m16Shot;
    public AudioSource reloadingSoundM16;
    public AudioSource emptyMagazineSoundM16;

    [Header("ショットガン用設定")]
    [UnityEngine.Serialization.FormerlySerializedAs("ShotgunShot")]
    public AudioClip shotgunShot;
    public AudioSource reloadingSoundShotgun;
    public AudioSource emptyMagazineSoundShotgun;

    [Header("投げ物（グレネード）")]
    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    [Header("ゾンビ用SE")]
    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;
    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;

    [Header("プレイヤー用SE")]
    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDie;

    [Header("NPC・ダイアログ")]
    public AudioSource npcChannel;
    public AudioClip npcTalk;

    [Header("BGM設定")]
    public AudioSource bgmChannel;
    public AudioClip gameClearMusic;

    // Input: なし, Output: なし, Side Effects: シングルトンの重複を破棄し自身を登録する
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

    // Input: 武器モデル列挙体, Output: なし, Side Effects: 武器の種類に応じた発砲音を再生する
    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                shootingChannel.PlayOneShot(p1911Shot); break;
            case WeaponModel.M16:
                shootingChannel.PlayOneShot(m16Shot); break;
            case WeaponModel.Shotgun:
                shootingChannel.PlayOneShot(shotgunShot); break;
            case WeaponModel.Sniper:
                shootingChannel.PlayOneShot(snipShot); break;
        }
    }

    // Input: 武器モデル列挙体, Output: なし, Side Effects: 武器の種類に応じたリロード音を再生する
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
