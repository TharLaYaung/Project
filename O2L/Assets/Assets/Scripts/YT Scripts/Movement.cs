using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// キャラクターの移動や姿勢（しゃがみ状態など）を制御するクラス

public class Movement : MonoBehaviour
{
    // 現在しゃがんでいるかどうかを判定するフラグ
    public bool isCrouching = false;

    // しゃがみ時の移動速度にかける倍率（例: 0.5を指定すると速度が半分になる）
    public float crouchingMultiplier;

    // キャラクターの物理的な衝突判定を制御するコンポーネントへの参照
    public CharacterController controller;

    // しゃがんでいる時のキャラクターの高さ（コライダの高さ）
    public float crouchingHeight = 1.25f;

    
    /// 毎フレーム実行される更新処理
    
    private void Update()
    {
        // しゃがみフラグが真（true）の場合
        if (isCrouching == true)
        {
            // ここで CharacterController の高さを変更する処理を記述します
            // controller.height = crouchingHeight;
            // これにより、低い通路を通れるようになったり、敵の弾を避けやすくなったりします
        }
    }
}