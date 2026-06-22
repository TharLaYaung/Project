using UnityEngine;


/// プレイヤーが回転しても、ミニマップ上のアイコン（ドット）の向きを固定するスクリプト。

public class MinimapIcon : MonoBehaviour
{
    [Header("アイコンの向き設定")]
    [Tooltip("ワールド座標における固定角度（通常はデフォルトのままでOK）。")]
    public Vector3 fixedRotation = new Vector3(90f, 0f, 0f);

    private void LateUpdate()
    {
        // 親（プレイヤー）の回転を無視して、常に指定したワールド回転を維持
        // これにより、ミニマップ上でドットが常に同じ向きを表示します
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}