using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ゲームクリア時のBGM再生を管理するクラス
/// シングルトンパターンを使用して、どこからでもアクセス可能にしています

public class Gameclearbgm : MonoBehaviour
{
    // 外部から Gameclearbgm.Instance でこのクラスの機能を使えるようにするための静的変数
    public static Gameclearbgm Instance { get; set; }

    // BGMを再生するためのオーディオソースコンポーネント
    public AudioSource bgmgameclearChannel;

    // 再生するゲームクリアBGMのオーディオデータ
    public AudioClip gameclearMusic;

    
    /// スクリプトが読み込まれた瞬間に実行される
    
    private void Awake()
    {
        // シングルトンの重複チェック
        // すでにインスタンスが存在している場合は、新しく作られた方を破棄する
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // インスタンスを自分自身に設定する
            Instance = this;
        }
    }

   
    /// オブジェクトが有効になった最初のフレームで実行される
    
    private void Start()
    {
        // ゲームクリアのBGMを一度だけ再生する
        // PlayOneShotを使用することで、他の音と重なっても再生可能です
        if (bgmgameclearChannel != null && gameclearMusic != null)
        {
            bgmgameclearChannel.PlayOneShot(gameclearMusic);
        }
    }
}