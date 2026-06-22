using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// すべての個別チュートリアル（移動、射撃、エリア到達など）の親となるクラス。
/// 共通のプロパティを提供し、マネージャーへの自動登録を行います。

public class Tutorial : MonoBehaviour
{
    // チュートリアルが実行される順番（0, 1, 2...）
    public int Order;

    // インスペクターから入力する、プレイヤーに表示する説明文
    [TextArea(3, 10)]
    public string Explanation;

    
    /// オブジェクトが生成された際に実行されます。
    /// シーン内の各チュートリアルオブジェクトが、自分自身を管理クラスへ登録します。
   
    void Awake()
    {
        // TutorialManagerが持つリストに、このコンポーネントを追加
        // これにより、マネージャー側で全チュートリアルの把握が可能になります。
        TutorialManager.Instance.Tutorials.Add(this);
    }

  
    /// チュートリアルのクリア条件を判定するための仮想メソッド。
    /// 派生クラス（例：KeyTutorial, TriggerTutorial）で override して具体的な処理を書きます。
    
    public virtual void CheckIfHappening()
    {
        // 基底クラスでは中身は空です。
    }
}