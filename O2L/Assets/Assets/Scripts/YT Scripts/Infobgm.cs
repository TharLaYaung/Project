using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 複数シーン間で同一のBGM再生状態を維持し、管理の煩雑さを減らすためのクラス
public class Infobgm : MonoBehaviour
{
    public static Infobgm Instance { get; set; }

    public AudioSource bgminfoChannel;
    public AudioClip infoMusic;

    // 入力: なし, 出力: なし, 副作用: 重複するインスタンスの破棄、自身を登録
    private void Awake()
    {
        // 複数シーンのロードによってBGMが二重に再生されるのを防ぐため
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // 入力: なし, 出力: なし, 副作用: BGMの再生開始
    private void Start()
    {
        // ユーザーが画面遷移したことを音で認知できるように即座に再生する
        if (bgminfoChannel != null && infoMusic != null)
        {
            bgminfoChannel.PlayOneShot(infoMusic);
        }
    }
}
