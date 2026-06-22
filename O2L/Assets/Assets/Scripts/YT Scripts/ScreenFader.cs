using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// ゲームオーバー時などに画面を徐々に暗くする（フェードアウト）演出を管理するクラス

public class ScreenFader : MonoBehaviour
{
    // フェードに使用する全画面表示のUI画像（通常は真っ黒な画像）
    public Image fadeImage;

    // フェードにかける時間（秒）
    public float fadeDuration = 1.0f;

    
    /// 外部（PLAYERクラスなど）からフェードを開始するためのメソッド
    
    public void StartFade()
    {
        // フェードアウトのコルーチンを開始
        StartCoroutine(FadeOut());

        // 画像オブジェクト自体を有効にする
        fadeImage.gameObject.SetActive(true);
    }

    
    /// 時間経過とともに画像のアルファ値を変化させるコルーチン
    
    private IEnumerator FadeOut()
    {
        float timer = 0f;

        // 開始時の色（現在の色）を取得
        Color startColor = fadeImage.color;

        // 最終的な色を設定（黒色、かつ不透明度1.0）
        Color endColor = new Color(0f, 0f, 0f, 1f);

        // 設定した時間（fadeDuration）が経過するまでループ
        while (timer < fadeDuration)
        {
            // Color.Lerp を使用して、開始色と終了色の間を時間に応じて補間する
            // timer / fadeDuration は 0.0 から 1.0 に向かって進む割合
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);

            // 前のフレームからの経過時間を加算
            timer += Time.deltaTime;

            // 1フレーム待機
            yield return null;
        }

        // ループ終了後、確実に最終的な色（真っ黒）に設定する
        fadeImage.color = endColor;
    }
}