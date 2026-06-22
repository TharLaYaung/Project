using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


/// 特定のキー入力やマウス操作を検知してチュートリアルを進行させるクラス
/// Tutorialクラスを継承しています

public class KeyTutorial : Tutorial
{
    // チュートリアルで入力が必要なキーの名前（string）を保持するリスト
    // Unityのエディタから "W", "A", "S", "D" などを登録します
    public List<string> Keys = new List<string>();

    
    /// チュートリアルの進行条件を毎フレームチェックするメソッド
    
    public override void CheckIfHappening()
    {
        // 登録されているキーの数だけループして入力を確認する
        for (int i = 0; i < Keys.Count; i++)
        {
            // 文字列（string）をUnityのKeyCode型に変換する
            KeyCode code = (KeyCode)Enum.Parse(typeof(KeyCode), Keys[i]);

            // 指定されたキーが押されたか、またはマウスの左右クリックが押されたかを判定
            if (Input.GetKeyDown(code))
            {
                // 条件を満たしたキーをリストから削除する
                Keys.RemoveAt(i);
                break;
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                // マウス操作（左クリック:0, 右クリック:1）があった場合もリストから削除
                Keys.RemoveAt(i);
                break;
            }
        }

        // リスト内のすべてのキー（操作）が完了（カウントが0）した場合
        if (Keys.Count == 0)
        {
            // チュートリアルマネージャーに完了を通知する
            TutorialManager.Instance.CompleteTutorial();
        }
    }
}