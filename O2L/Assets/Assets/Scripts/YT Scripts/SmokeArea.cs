using UnityEngine;

// 煙による視界不良を再現するため、範囲内の敵にブラインド効果を与える
public class SmokeArea : MonoBehaviour
{
    private const float DEFAULT_RADIUS = 5f;
    private const float DEFAULT_DURATION = 10f;

    public float radius = DEFAULT_RADIUS;
    public float duration = DEFAULT_DURATION;

    // Input: なし, Output: なし, Side Effects: スクリプトとオブジェクトが指定時間後に破棄されるようスケジュールする
    private void Start()
    {
        Destroy(gameObject, duration);
    }

    // Input: なし, Output: なし, Side Effects: 範囲内のEnemyコンポーネントを持つオブジェクトにブラインド処理を実行する
    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider objectInRange in colliders)
        {
            Enemy enemy = objectInRange.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Blind();
            }
        }
    }
}
