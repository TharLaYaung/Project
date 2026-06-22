using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// キー（鍵）に関連するシーンや演出でのBGM再生を管理するクラス
/// シングルトンパターンを採用し、プロジェクト全体で唯一の管理オブジェクトとして機能させます

public class Keysbgm : MonoBehaviour
{
    // 静的プロパティ：他のクラスから Keysbgm.Instance でこのオブジェクトにアクセスできます
    public static Keysbgm Instance { get; set; }

    // BGMを再生するためのオーディオソース
    public AudioSource bgmkeysChannel;

    // 再生するBGM（オーディオクリップ）
    public AudioClip keysMusic;

    
    /// インスタンスが読み込まれた際に実行（Startより先に呼ばれる）
    
    private void Awake()
    {
        // シングルトンの重複防止：
        // すでにインスタンスが存在している場合、新しいオブジェクトを破棄して二重再生やメモリ消費を防ぐ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // このオブジェクトを唯一の正規インスタンスとして登録
            Instance = this;
        }
    }

    
    /// オブジェクトが有効になった最初のフレームで実行
    
    private void Start()
    {
        // 設定されたオーディオソースでBGMを一度だけ再生
        // PlayOneShotを使用することで、他のSEと重なってもBGMとして再生され続けます
        if (bgmkeysChannel != null && keysMusic != null)
        {
            bgmkeysChannel.PlayOneShot(keysMusic);
        }
    }
}