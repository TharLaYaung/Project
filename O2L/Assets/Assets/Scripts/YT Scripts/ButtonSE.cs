using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 音響管理（SE）をシングルトンで制御するクラス
/// シーンを跨いでも破棄されず、どこからでも音を鳴らせるようにします
public class ButtonSE : MonoBehaviour
{
    // どこからでも ButtonSE.Instance でアクセスできるようにするための静的変数
    public static ButtonSE Instance { get; set; }

    // 音を再生するための本体（コンポーネント）
    public AudioSource audioSource;

    
    /// スクリプトが読み込まれた瞬間に実行される
    /// シングルトンの確立と重複チェックを行います
    private void Awake()
    {
        // すでにインスタンスが存在し、かつ自分自身ではない場合
        if (Instance != null && Instance != this)
        {
            // 重複しているため自分自身を破棄する
            Destroy(gameObject);
        }
        else
        {
            // 自分自身を唯一のインスタンスとして登録する
            Instance = this;
        }

        // シーンを切り替えてもこのオブジェクトが消えないように設定
        DontDestroyOnLoad(this);
    }

   
    /// 外部（ButtonOnClickなど）から呼び出され、効果音を再生するメソッド
    
    public void PlaySE(AudioClip SE)
    {
        // PlayOneShotは、再生中の音を止めずに重ねて再生できる便利なメソッドです
        if (audioSource != null && SE != null)
        {
            audioSource.PlayOneShot(SE);
        }
    }
}