using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// チュートリアルの追加や変更を容易にするため、共通のインターフェースを提供します。
public class Tutorial : MonoBehaviour
{
    public int order;

    [TextArea(3, 10)]
    public string explanation;

    
    /// Input: なし
    /// Output: なし
    /// Side Effects: 自身をマネージャーのリストに登録します。
    /// 一元管理するため、生成時に自動登録を行います。
    void Awake()
    {
        TutorialManager.Instance.tutorials.Add(this);
    }

  
    /// Input: なし
    /// Output: なし
    /// Side Effects: 条件達成時にチュートリアルを完了します。
    /// 各チュートリアル固有のクリア条件を判定するため、オーバーライド用として用意します。
    public virtual void CheckIfHappening()
    {
    }
}
