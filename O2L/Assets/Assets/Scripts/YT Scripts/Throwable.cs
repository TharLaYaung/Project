using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Throwable : MonoBehaviour
{
    private const int GRENADE_DAMAGE = 100;

    [SerializeField] private float delay = 3f;
    [SerializeField] private float damageRadius = 2f;
    [SerializeField] private float explosionForce = 800f;

    private float countdown;

    private bool hasExploded = false;
    public bool hasBeenThrown = false;

    public Cameracontroller cameraController;
    private Animator animator;
    public enum ThrowableType
    {
        None,
        Grenade,
        Smoke_Grenade
    }

    public ThrowableType throwableType;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<Cameracontroller>();


        countdown = delay;
    }

    private void Update()
    {
        if(hasBeenThrown)
        {
            countdown -=Time.deltaTime;
            if(countdown <= 0f && !hasExploded )
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();
        cameraController.CameraShake();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
            case ThrowableType.Smoke_Grenade:
                SmokeGrenadeEffect();
                break;
        }
    }
    public void SmokeGrenadeEffect()
    {
        // 視覚的なフィードバックを与えるためエフェクトを生成する
        GameObject smokeEffect = GlobalReferences.Instance.smokeGrenadeEffect;
        GameObject instantiatedSmoke = Instantiate(smokeEffect, transform.position, transform.rotation);
        
        SmokeArea smokeArea = instantiatedSmoke.AddComponent<SmokeArea>();
        smokeArea.radius = damageRadius;

        // プレイヤーに動作完了を伝えるため音声を再生する
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
            }
        }
    }

   /* void OnDestroy()
    {

        switch (throwableType)
        {
           
            case ThrowableType.Smoke_Grenade:
                //animator.SetBool("isBlinding", false);
                break;
        }


    }*/


    private void GrenadeEffect()
    {
        // 爆発の規模を視覚的に表現するためエフェクトを生成する
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect,transform.position,transform.rotation);

        // 臨場感を高めるため爆発音を再生する
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);

        // 周囲のオブジェクトに影響を与えるため範囲内のコライダーを取得する
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {

            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            if (objectInRange.gameObject.GetComponent<Enemy>())
            {
                objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(GRENADE_DAMAGE);
            }
        }

       
    }
}

