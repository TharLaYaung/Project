using UnityEngine;

/// ミニマップ上のアイコンがプレイヤーの向いている方向に関わらず常に一定の方向を向くようにするクラス
public class MinimapIcon : MonoBehaviour
{
    [Header("アイコンの向き設定")]
    [Tooltip("ワールド座標における固定角度")]
    public Vector3 fixedRotation = new Vector3(90f, 0f, 0f);

    // 入力: なし, 出力: なし, 副作用: アイコンの回転の固定
    private void LateUpdate()
    {
        // プレイヤーの回転に依存せず、ミニマップ上で常に同じ方角を向かせて視認性を高めるため
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
