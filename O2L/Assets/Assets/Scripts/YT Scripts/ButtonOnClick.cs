using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI操作時のフィードバックとして音声を提供するため
public class ButtonOnClick : MonoBehaviour
{
    [Header("再生する効果音の設定")]
    // どの音声を鳴らすかインスペクター上で柔軟に変更できるようにするため
    public AudioClip clip;

    // Input: なし / Output: なし / Side Effects: 音声再生要求
    public void OnClick()
    {
        // SE再生をグローバルに管理し、オブジェクト破棄時も音が途切れないようにするため
        if (clip != null)
        {
            ButtonSE.Instance.PlaySE(clip);
        }
        else
        {
            // 設定漏れによる無音状態を開発段階で早期に検知するため
            Debug.LogWarning("ButtonOnClick: AudioClipが設定されていません。");
        }
    }
}
