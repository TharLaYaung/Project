using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 設定画面（Settings）などの特定のシーンでBGMを再生・管理するクラス
/// シングルトンパターンを使用して、シーン内で唯一のインスタンスであることを保証します

public class Settingsbgm : MonoBehaviour
{
    // 他のクラスから Settingsbgm.Instance でアクセス可能にするための静的プロパティ
    public static Settingsbgm Instance { get; set; }

    // BGMを再生するためのオーディオソースコンポーネント
    public AudioSource bgmsettingsChannel;

    
    /// スクリプトのインスタンスがロードされた瞬間に実行される初期化処理
   
    private void Awake()
    {
        // インスタンスが既に存在しており、自分自身でない場合は重複を避けるために破棄する
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // このオブジェクトを唯一のインスタンスとして登録する
            Instance = this;
        }
    }


    /// オブジェクトが有効になった最初のフレームで実行される処理
  
    private void Start()
    {
        // 設定されたオーディオソースを使用して、BGMの再生を開始する
        // AudioSourceにクリップがセットされており、「Play On Awake」がオフの場合に有効です
        if (bgmsettingsChannel != null)
        {
            bgmsettingsChannel.Play();
        }
    }
}