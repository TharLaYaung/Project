using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;


/// プレイヤーのHP状態をUI（スライダーやエフェクト）に反映させるクラス

public class PlayerHpBar : MonoBehaviour
{
    // 親オブジェクトを保持するための変数（現状は未使用）
    private GameObject parentObject;

    // 操作対象となるプレイヤーのゲームオブジェクト
    [SerializeField] private GameObject Player;

    // プレイヤーにアタッチされている Playercontroller スクリプト
    Playercontroller Player_script;

    // プレイヤーの最大体力（UI表示用）
    [SerializeField] private int maxHP;

    // プレイヤーの現在体力（UI表示用）
    [SerializeField] private int hp;

    // HPを表示するためのUIスライダー
    [SerializeField] private Slider hpSlider;

    

    
    /// 初期化処理
  
    void Start()
    {
        // ヒエラルキー上の "Player" という名前のオブジェクトを検索して取得
        Player = GameObject.Find("Player");

        // プレイヤーから移動やステータスを管理しているスクリプトを取得
        Player_script = Player.GetComponent<Playercontroller>();

        // プレイヤースクリプトの変数から初期のHPと最大HPを取得
        hp = Player_script.PHp;
        maxHP = Player_script.MaxHp;

        // 【注意】元のコードでは maxHP = hp; となっていましたが、
        // 最大値を固定する場合は maxHP = Player_script.MaxHp; のままで保持します。

        // スライダーの見た目を満タン（1.0 = 100%）に設定
        hpSlider.value = 1f;
    }

   
    /// フレームごとの更新処理
   
    void Update()
    {
        // 常に最新のプレイヤー情報を参照するためにスクリプトを再取得（効率化のためにはStartでの取得のみが推奨されます）
        Player_script = Player.GetComponent<Playercontroller>();

        // プレイヤースクリプト内の現在のHP数値を同期
        hp = Player_script.PHp;
        maxHP = Player_script.MaxHp;

        // スライダーの値を更新。現在のHPを最大HPで割って 0.0 ～ 1.0 の範囲に変換する
        // (float) でキャストすることで、整数同士の計算による切り捨てを防止している
        hpSlider.value = (float)hp / maxHP;

        // HPが最大値を超えてしまった場合の補正処理
        if (hp > maxHP)
        {
            hp = maxHP;
        }
    }

   
    
}