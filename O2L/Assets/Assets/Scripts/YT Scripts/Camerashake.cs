using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 画面揺れは臨場感を高めるための演出として機能する
// 揺れ処理中に元の位置がずれないように基準座標を保持する
public class Camerashake : MonoBehaviour
{
    private const float MIN_SHAKE_RANGE = -1f;
    private const float MAX_SHAKE_RANGE = 1f;

    private Vector3 originalPos;

    // 揺れ終了後に元の位置へ確実に戻すため、初期座標を記録する
    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    // Input: duration (揺れの持続時間), magnitude (揺れの強さ)
    // Output: なし
    // Side Effects: カメラのlocalPositionを変更するが、終了時には元の位置に戻る
    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        // 毎フレーム乱数でカメラを移動させ、不規則な揺れを表現するため
        while (elapsed < duration)
        {
            float x = Random.Range(MIN_SHAKE_RANGE, MAX_SHAKE_RANGE) * magnitude;
            float y = Random.Range(MIN_SHAKE_RANGE, MAX_SHAKE_RANGE) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            // フレームごとに更新を反映させるため待機する
            yield return null;
        }

        // 揺れによる位置のズレを蓄積させないよう、正確に初期位置へ戻すため
        transform.localPosition = originalPos;
    }
}
