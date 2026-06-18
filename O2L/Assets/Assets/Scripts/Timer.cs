using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Timer : MonoBehaviour
{
    private const int SECONDS_PER_MINUTE = 60;

    [SerializeField] private Text timeText;
    [SerializeField] private float remainingTime = 900f;

    void Update()
    {
        // タイムアップ判定のため毎フレーム経過時間を減算する
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            // マイナス表示を防ぐため0で下限を固定する
            remainingTime = 0;

            // プレイヤーにタイムアップを視覚的に伝えるため赤色に変更する
            timeText.color = Color.red;

            SceneManager.LoadScene("GameOver");
        }

        int minutes = Mathf.FloorToInt(remainingTime / SECONDS_PER_MINUTE);
        int seconds = Mathf.FloorToInt(remainingTime % SECONDS_PER_MINUTE);

        // プレイヤーが直感的に残り時間を把握できるようにフォーマットする
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
