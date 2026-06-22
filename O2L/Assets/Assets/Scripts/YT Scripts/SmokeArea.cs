using UnityEngine;

public class SmokeArea : MonoBehaviour
{
    public float radius = 5f;
    public float duration = 10f;

    void Start()
    {
        // Ensure the smoke area script doesn't run forever if the visual effect isn't destroyed
        Destroy(gameObject, duration);
    }

    void Update()
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
