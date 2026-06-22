using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ゲームオーバー時のBGM再生を管理するクラス
/// シングルトンパターンを使用して、他のスクリプトから簡単にアクセスできるようにしています

public class Gameoverbgm : MonoBehaviour
{
    // 外部から Gameoverbgm.Instance を通じてアクセスするための静的変数
    public static Gameoverbgm Instance { get; set; }

    // BGMを再生するためのオーディオソースコンポーネント
    public AudioSource bgmgameoverChannel;

    // 再生するゲームオーバー用のオーディオクリップ（BGM）
    public AudioClip gameoverMusic;

    
    /// スクリプトのインスタンスが読み込まれた時に実行される処理
    
    private void Awake()
    {
        // シングルトンの重複チェック
        // すでにインスタンスが存在している場合、新しいオブジェクトを破棄して二重生成を防ぐ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // 自分自身をインスタンスとして登録
            Instance = this;
        }
    }

    
    /// オブジェクトが有効になった直後のフレームで実行される処理
    
    private void Start()
    {
        // ゲームオーバーシーンの開始時にBGMを一度だけ再生する
        // PlayOneShotは、再生中の音があっても重ねて再生できるメソッドです
        if (bgmgameoverChannel != null && gameoverMusic != null)
        {
            bgmgameoverChannel.PlayOneShot(gameoverMusic);
        }
    }
}