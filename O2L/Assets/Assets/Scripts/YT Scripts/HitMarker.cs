using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// 敵に攻撃が当たった際のヒットマーカー表示を制御するクラス

public class HitMarker : MonoBehaviour
{
    // ヒットマーカーを表示し続ける時間（秒）
    [Header("表示設定")]
    public float showtime;

    // ヒットマーカーのUI画像（GameObject）
    public GameObject hitmarker;

    
    /// ゲーム開始時に実行される
    
    void Start()
    {
        // 最初はヒットマーカーを非表示にしておく
        hitmarker.SetActive(false);
    }

    
    /// 外部（Enemyクラスなど）から着弾時に呼び出されるメソッド
   
    public void getHitmarker()
    {
        // 連続でヒットした場合に備え、実行中の非表示タイマー（コルーチン）を一旦停止する
        StopCoroutine("showhitmarker");

        // ヒットマーカーを表示する
        hitmarker.SetActive(true);

        // 指定時間後に非表示にするタイマー（コルーチン）を開始する
        StartCoroutine("showhitmarker");
    }

    
    /// 指定された時間待機してからヒットマーカーを非表示にするコルーチン
    
    public IEnumerator showhitmarker()
    {
        // インスペクターで設定した時間（showtime）分だけ待機する
        yield return new WaitForSeconds(showtime);

        // ヒットマーカーを非表示にする
        hitmarker.SetActive(false);
    }
}