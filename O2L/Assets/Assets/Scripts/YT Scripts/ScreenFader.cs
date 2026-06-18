using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// 視覚的な遷移を表現するため、画面全体のフェードアウト演出を行う
public class ScreenFader : MonoBehaviour
{
    private const float DEFAULT_FADE_DURATION = 1.0f;

    public Image fadeImage;
    public float fadeDuration = DEFAULT_FADE_DURATION;

    // Input: なし, Output: なし, Side Effects: フェードアウト処理を開始しUIを有効化する
    public void StartFade()
    {
        StartCoroutine(FadeOut());
        fadeImage.gameObject.SetActive(true);
    }

    // Input: なし, Output: IEnumerator, Side Effects: 時間経過に伴いUIのアルファ値を変更する
    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = Color.black;

        while (timer < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor;
    }
}
