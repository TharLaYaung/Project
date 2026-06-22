using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// カメラを揺らす（シェイク）演出を制御するクラス

public class Camerashake : MonoBehaviour
{
    // カメラの本来の位置を保持するための変数
    Vector3 originalPos;

   
    /// スクリプトが読み込まれた時に実行される
    void Awake()
    {
        // 揺れが終わった後に戻せるよう、初期状態のローカル座標を保存しておく
        originalPos = transform.localPosition;
    }

    
    /// カメラを揺らすコルーチン
    
    public IEnumerator Shake(float duration, float magnitude)
    {
        // 経過時間を記録する変数
        float elapsed = 0.0f;

        // 指定された時間（duration）が経過するまでループを繰り返す
        while (elapsed < duration)
        {
            // 指定した強さの範囲内でランダムな座標（x, y）を計算する
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            // カメラの座標を、元の位置からランダムな値だけずらした座標に更新する
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            // 前のフレームからの経過時間を加算する
            elapsed += Time.deltaTime;

            // 1フレーム待機して、次のループへ進む
            yield return null;
        }

        // 揺れが終了した後、カメラの位置を確実に元の位置に戻す
        transform.localPosition = originalPos;
    }
}