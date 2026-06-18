using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// プレイヤーの現在地に基づいた文脈的指導を行うため、特定エリアへの到達を検知します。
public class TriggerTutorial : Tutorial
{
    // 誤検出を防ぐため、チュートリアル中のみ判定を有効にします。
    private bool isCurrentTutorial = false;

    // 特定の対象のみを検知するため、対象のTransformを指定します。
    public Transform hitTransform;

  
    /// Input: なし
    /// Output: なし
    /// Side Effects: トリガー判定が有効化されます。
    /// 指定された条件でのみ完了処理を行うため、状態をアクティブにします。
    public override void CheckIfHappening()
    {
        isCurrentTutorial = true;
    }


    /// Input: other (侵入したCollider)
    /// Output: なし
    /// Side Effects: 条件一致時、チュートリアルを完了します。
    /// 意図した対象が到達したかを判別し、進行を管理します。
    public void OnTriggerEnter(Collider other)
    {
        if (!isCurrentTutorial)
            return;

        if (other.transform == hitTransform)
        {
            TutorialManager.Instance.CompleteTutorial();
            isCurrentTutorial = false;
        }
    }
}
