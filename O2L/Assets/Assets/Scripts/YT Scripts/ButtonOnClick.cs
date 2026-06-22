using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ボタンがクリックされた時に特定のSE（効果音）を再生するためのクラス
public class ButtonOnClick : MonoBehaviour
{
    // 再生したいオーディオクリップをインスペクターからアサインします
    [Header("再生する効果音の設定")]
    public AudioClip clip;

    
    /// ボタンのOnClickイベントから呼び出される公開メソッド
    public void OnClick()
    {
        // シングルトンパターン（ButtonSE.Instance）を使用して、
        // 効果音管理クラスのPlaySEメソッドを実行し、指定したクリップを再生します。
        
        if (clip != null)
        {
            ButtonSE.Instance.PlaySE(clip);
        }
        else
        {
            // クリップが設定されていない場合の警告
            Debug.LogWarning("ButtonOnClick: AudioClipが設定されていません。");
        }
    }
}