using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// メインメニュー画面のBGM再生を一括管理するクラス
/// シングルトンパターンを使用して、二重再生の防止と他クラスからのアクセスを容易にします

public class Mainmenubgm : MonoBehaviour
{
    // 静的変数：Mainmenubgm.Instance でどこからでもこのオブジェクトを参照できます
    public static Mainmenubgm Instance { get; set; }

    // BGMを流すためのオーディオソースコンポーネント
    public AudioSource bgmmainmenuChannel;

   
    /// スクリプトが読み込まれた際に実行される初期化処理
    
    private void Awake()
    {
        // シングルトンの実装：
        // すでにインスタンスが存在しており、それが自分自身でない場合は、
        // 重複してBGMが流れないように新しいオブジェクトを破棄します
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // このオブジェクトを唯一の有効なインスタンスとして登録します
            Instance = this;
        }
    }

    
    /// オブジェクトが有効になった最初のフレームで実行される処理
    
    private void Start()
    {
        // メインメニューのBGM再生を開始します
        // PlayOneShotではなくPlayを使用することで、ループ設定などが有効になります
        if (bgmmainmenuChannel != null)
        {
            bgmmainmenuChannel.Play();
        }
    }
}