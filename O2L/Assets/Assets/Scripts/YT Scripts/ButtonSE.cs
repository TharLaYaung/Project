using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シーン間遷移でも音声を途切れさせず、一元管理するため
public class ButtonSE : MonoBehaviour
{
    // 常に単一のオーディオリソースを使い回すため
    public static ButtonSE Instance { get; set; }

    // 実際の音声出力処理を担うため
    public AudioSource audioSource;

    // Input: なし / Output: なし / Side Effects: インスタンスの初期化と重複破棄
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // メモリリークや二重管理を防ぐため破棄する
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // シーン遷移時にBGMやSEが不自然に途切れないようにするため
        DontDestroyOnLoad(this);
    }

    // Input: AudioClip se / Output: なし / Side Effects: 音声再生
    public void PlaySE(AudioClip se)
    {
        // 連続した操作でも前の音が途切れないようにするため
        if (audioSource != null && se != null)
        {
            audioSource.PlayOneShot(se);
        }
    }
}
