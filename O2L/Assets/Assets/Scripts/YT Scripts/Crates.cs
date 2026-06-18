using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 破壊可能なオブジェクトが壊れた際に、破片を物理演算に任せて自然な見た目にするため
public class Crates : MonoBehaviour
{
    public List<Rigidbody> allParts = new List<Rigidbody>();

    // Input: なし
    // Output: なし
    // Side Effects: allParts内の全RigidbodyのisKinematicをfalseにする
    public void Shatter()
    {
        // 破片が重力や衝撃の影響を受けるようにし、崩れ落ちる演出を作るため
        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = false;
        }
    }
}
