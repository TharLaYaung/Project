using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// プレイヤーの当たり判定の大きさを動的に変更し、姿勢変化を実現するクラス
public class PlayerMovement : MonoBehaviour
{
    public float crouchHeight = 1f;
    public float standingHeight = 2f;

    private CapsuleCollider capsuleCollider;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // 狭い地形を通過できるようにするため、しゃがみ入力中はコライダーを縮小する
        if (Input.GetKey(KeyCode.LeftControl))
        {
            capsuleCollider.height = crouchHeight;
        }
        else
        {
            capsuleCollider.height = standingHeight;
        }
    }
}
