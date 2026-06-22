using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 特定のエリア（トリガー）に侵入したことを検知してチュートリアルを完了させるクラス
/// Tutorialクラスを継承しており、TutorialManagerによって制御されます

public class TriggerTutorial : Tutorial
{
    // このチュートリアルが現在アクティブ（実行中）かどうかを判定するフラグ
    private bool isCurrentTutorial = false;

    // 侵入を検知したい対象のTransform（通常はプレイヤーのTransformをインスペクターで設定）
    public Transform HitTransform;

  
    /// チュートリアルマネージャーから「このチュートリアルを開始してほしい」と呼ばれた時に実行される
 
    public override void CheckIfHappening()
    {
        // チュートリアルを開始状態にする
        isCurrentTutorial = true;
    }


    /// Unityの物理演算によるトリガー侵入検知
   
    public void OnTriggerEnter(Collider other)
    {
        // このチュートリアルがアクティブでない場合は、何かが入ってきても無視する
        if (!isCurrentTutorial)
            return;

        // 侵入してきたオブジェクトが、指定したHitTransform（プレイヤーなど）と一致するか確認
        if (other.transform == HitTransform)
        {
            // 条件を満たしたため、チュートリアルマネージャーに完了を通知
            TutorialManager.Instance.CompleteTutorial();

            // 二重に完了処理が走らないよう、フラグをオフにする
            isCurrentTutorial = false;
        }
    }
}