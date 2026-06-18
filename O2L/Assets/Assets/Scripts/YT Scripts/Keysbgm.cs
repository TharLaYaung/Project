using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 特定のシーンにおけるBGMの再生状態を維持し、管理を容易にするためのクラス
public class Keysbgm : MonoBehaviour
{
    public static Keysbgm Instance { get; set; }

    public AudioSource bgmkeysChannel;
    public AudioClip keysMusic;

    // 入力: なし, 出力: なし, 副作用: インスタンスの登録、重複破棄
    private void Awake()
    {
        // 複数回のシーンロードによって同じBGMが重なって再生されるのを防ぐため
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // 入力: なし, 出力: なし, 副作用: BGMの再生
    private void Start()
    {
        // ユーザーがシーンの雰囲気を即座に感じ取れるようにするため
        if (bgmkeysChannel != null && keysMusic != null)
        {
            bgmkeysChannel.PlayOneShot(keysMusic);
        }
    }
}
