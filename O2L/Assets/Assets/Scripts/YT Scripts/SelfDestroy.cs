using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 生成された後、一定時間経過したら自動的に自分自身を削除するクラス
/// 弾着エフェクトや血飛沫などのパーティクル、一時的な演出オブジェクトに使用します

public class SelfDestroy : MonoBehaviour
{
    // オブジェクトが破棄されるまでの待機時間（秒）
    [Header("破壊までの時間設定")]
    public float timeForDestruction;

    
    /// オブジェクトが生成された最初のフレームで実行される処理

    void Start()
    {
        // 削除処理をコルーチンとして開始
        StartCoroutine(DestroySelf(timeForDestruction));
    }

   
    /// 指定された時間待機した後に、自分自身を破棄するコルーチン
   
    
    private IEnumerator DestroySelf(float timeForDestruction)
    {
        // 指定された秒数分、処理を中断して待機する
        yield return new WaitForSeconds(timeForDestruction);

        // 自分自身のゲームオブジェクトをシーンから削除する
        // これにより、メモリの節約やシーン内の整理が行われます
        Destroy(gameObject);
    }
}