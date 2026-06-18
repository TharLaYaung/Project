using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// プレイヤーが特定のキーまたはマウス入力を完了するまで待機するチュートリアル処理クラス
public class KeyTutorial : Tutorial
{
    private const int LEFT_CLICK = 0;
    private const int RIGHT_CLICK = 1;

    public List<string> keys = new List<string>();

    // 入力: なし, 出力: なし, 副作用: 入力リストの要素削除、チュートリアル完了通知
    public override void CheckIfHappening()
    {
        // プレイヤーが指定された全てのアクションを実行したか確認するため
        for (int i = 0; i < keys.Count; i++)
        {
            KeyCode code = (KeyCode)Enum.Parse(typeof(KeyCode), keys[i]);

            // プレイヤーが意図した操作（キーボードまたはマウス）を行ったかを判定するため
            if (Input.GetKeyDown(code))
            {
                keys.RemoveAt(i);
                break;
            }
            else if (Input.GetMouseButtonDown(LEFT_CLICK) || Input.GetMouseButtonDown(RIGHT_CLICK))
            {
                keys.RemoveAt(i);
                break;
            }
        }

        // 残りの要求操作がなくなった時点で次のチュートリアルへ進行させるため
        if (keys.Count == 0)
        {
            TutorialManager.Instance.CompleteTutorial();
        }
    }
}
