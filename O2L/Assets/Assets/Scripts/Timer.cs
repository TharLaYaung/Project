using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


/// ゲームの制限時間を管理し、カウントダウンとUI表示、終了判定を行うクラス

public class Timer : MonoBehaviour
{
    // 時間を表示するためのUIテキスト
    [SerializeField] Text TimeText;

    // 残り時間（秒）。初期値は900秒（15分）
    [SerializeField] float RemainingTime = 900f;

   
    /// 毎フレーム実行される更新処理
    
    void Update()
    {
        // 残り時間が0より大きい場合、カウントダウンを継続
        if (RemainingTime > 0)
        {
            // 前のフレームからの経過時間を減算する
            RemainingTime -= Time.deltaTime;
        }
        // 残り時間が0を下回った場合（タイムアップ時）
        else if (RemainingTime < 0)
        {
            // マイナスにならないよう0で固定
            RemainingTime = 0;

            // 警告としてテキストの色を赤に変更
            TimeText.color = Color.red;

            // ゲームオーバーシーンへ遷移
            SceneManager.LoadScene("GameOver");
        }

        // 残り秒数から「分」を計算（60で割った整数値）
        int Minutes = Mathf.FloorToInt(RemainingTime / 60);

        // 残り秒数から「秒」を計算（60で割った余り）
        int Seconds = Mathf.FloorToInt(RemainingTime % 60);

        // UIテキストを「00:00」の形式でフォーマットして表示
        TimeText.text = string.Format("{0:00}:{1:00}", Minutes, Seconds);
    }
}