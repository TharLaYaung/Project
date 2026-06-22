using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ゲームのセーブおよびロード機能を一括管理するクラス
/// シングルトンパターンにより、ゲーム全体で一つのインスタンスのみが存在することを保証します

public class SaveLoadManager : MonoBehaviour
{
    // 静的プロパティ：他のスクリプトから SaveLoadManager.Instance でアクセス可能です
    public static SaveLoadManager Instance { get; set; }

    
    /// スクリプトのインスタンスがロードされたときに実行される初期化処理
    
    private void Awake()
    {
        // すでにインスタンスが存在しており、それが自分自身でない場合
        if (Instance != null && Instance != this)
        {
            // 重複した管理オブジェクトが作られないよう、新しい方を破棄します
            Destroy(gameObject);
        }
        else
        {
            // このオブジェクトを唯一の正規インスタンスとして登録します
            Instance = this;
        }

        // シーンを切り替えてもこのオブジェクトが破棄されないように設定します
        // これにより、タイトル画面からゲーム画面へ移動してもセーブデータの管理を継続できます
        DontDestroyOnLoad(this);
    }
}