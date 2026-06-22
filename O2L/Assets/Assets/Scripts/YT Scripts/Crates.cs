using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 木箱などのオブジェクトが粉砕する演出を制御するクラス

public class Crates : MonoBehaviour
{
    // 粉砕した時の破片（Rigidbodyを持つパーツ）のリスト
    // Unityのエディタ（インスペクター）からあらかじめパーツを登録しておきます
    public List<Rigidbody> allParts = new List<Rigidbody>();

    
    /// オブジェクトを粉砕させるメソッド
    public void Shatter()
    {
        // リストに登録されているすべてのパーツに対してループ処理を行う
        foreach (Rigidbody part in allParts)
        {
            // isKinematicを無効（false）にすることで、
            // そのパーツが物理演算（重力や衝撃）の影響を受けるようになり、バラバラに崩れ落ちます
            part.isKinematic = false;
        }
    }
}