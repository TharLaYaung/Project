using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// グローバルなBGM再生制御を一元化するためシングルトンパターンを採用
public class Gameclearbgm : MonoBehaviour
{
    public static Gameclearbgm Instance { get; set; }

    // BGMの音量やエフェクトを独立して管理するため専用のAudioSourceを割り当てる
    public AudioSource bgmgameclearChannel;

    // インスペクターから曲を動的に変更できるように保持
    public AudioClip gameclearMusic;

    private void Awake()
    {
        // 意図しないBGMの重複再生を防ぐため、既存のインスタンスを優先する
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // 他のオーディオ再生を中断させないためPlayOneShotを利用する
        if (bgmgameclearChannel != null && gameclearMusic != null)
        {
            bgmgameclearChannel.PlayOneShot(gameclearMusic);
        }
    }
}
