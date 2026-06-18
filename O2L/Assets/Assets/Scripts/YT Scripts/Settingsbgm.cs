using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 設定画面など特定のシーンでBGMを持続させるため、シングルトンで管理する
public class Settingsbgm : MonoBehaviour
{
    public static Settingsbgm Instance { get; set; }

    [UnityEngine.Serialization.FormerlySerializedAs("bgmsettingsChannel")]
    public AudioSource bgmSettingsChannel;

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

    // Input: なし, Output: なし, Side Effects: BGMの再生を開始する
    private void Start()
    {
        if (bgmSettingsChannel != null)
        {
            bgmSettingsChannel.Play();
        }
    }
}
