using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 攻撃命中時の視覚的フィードバックを提供し、プレイヤーの射撃感触を向上させる
public class HitMarker : MonoBehaviour
{
    [Header("表示設定")]
    // 連続ヒット時にUIがちらつかないよう、表示の遅延時間を調整可能にする
    public float showtime;

    // UIのオンオフを直接制御するためオブジェクトへの参照を保持する
    public GameObject hitmarker;

    void Start()
    {
        // 初期状態での誤表示を防ぐため、起動時に非表示状態を強制する
        hitmarker.SetActive(false);
    }

    // 被弾イベント発生時に呼び出され、即座にUIをアクティブ化する
    // Side Effects: 既存の非表示コルーチンをキャンセルし、新しくタイマーを開始する
    public void getHitmarker()
    {
        // 多段ヒット時にタイマーが重複して早期に消えるのを防ぐためリセットする
        StopCoroutine("showhitmarker");

        hitmarker.SetActive(true);

        StartCoroutine("showhitmarker");
    }

    // 指定時間経過後にマーカーを消去し、画面の視認性を回復させる
    // Side Effects: WaitForSeconds経過後、hitmarkerを非アクティブ化する
    public IEnumerator showhitmarker()
    {
        yield return new WaitForSeconds(showtime);

        hitmarker.SetActive(false);
    }
}
