using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// キャラクターの移動状態を監視し、コライダーの高さなどを動的に変更するクラス
public class Movement : MonoBehaviour
{
    public bool isCrouching = false;
    public float crouchingMultiplier;
    public CharacterController controller;
    public float crouchingHeight = 1.25f;

    // 入力: なし, 出力: なし, 副作用: コライダーの高さの更新（現在はコメントアウト）
    private void Update()
    {
        // プレイヤーが狭い場所を通行できるように、しゃがみ時は判定を小さくするため
        if (isCrouching)
        {
            // controller.height = crouchingHeight;
        }
    }
}
