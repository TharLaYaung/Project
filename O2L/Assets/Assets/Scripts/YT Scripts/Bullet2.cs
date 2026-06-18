using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 弾の当たり判定と各種オブジェクトへの影響を管理するため
public class Bullet2 : MonoBehaviour
{
    private const int FIRST_CONTACT_INDEX = 0;

    // 敵に与えるダメージ量をインスペクタから調整するため
    public int bulletDamage;

    // Input: Collision objectWeHit / Output: なし / Side Effects: エフェクト生成、ダメージ処理、自身の破棄
    private void OnCollisionEnter(Collision objectWeHit)
    {
        // 貫通せずにその場で弾を消滅させるため
        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            CreatBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }

        // 破壊可能な障害物を壊して進行ルートを確保するため
        if (objectWeHit.gameObject.CompareTag("Crates"))
        {
            print("Crate");

            // 親子構造のどこにコンポーネントがあっても確実に取得するため
            Crates crate = objectWeHit.gameObject.GetComponent<Crates>();
            if (crate == null) crate = objectWeHit.gameObject.GetComponentInParent<Crates>();

            if (crate != null)
            {
                crate.Shatter();
            }
        }

        // 敵へのダメージ判定を行い、倒すため
        if (objectWeHit.gameObject.CompareTag("Enemy"))
        {
            // 当たり判定が子オブジェクトにあっても本体へダメージを与えるため
            Enemy enemy = objectWeHit.gameObject.GetComponent<Enemy>();
            if (enemy == null) enemy = objectWeHit.gameObject.GetComponentInParent<Enemy>();

            if (enemy != null && enemy.isDead == false)
            {
                enemy.TakeDamage(bulletDamage);
            }

            // 被弾箇所を視覚的にわかりやすくするため
            CreateBloodSprayEffect(objectWeHit);

            Destroy(gameObject);
        }
    }

    // Input: Collision objectWeHit / Output: なし / Side Effects: 血飛沫エフェクトの生成
    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        // 弾が正確に当たった位置と角度にエフェクトを発生させるため
        ContactPoint contact = objectWeHit.contacts[FIRST_CONTACT_INDEX];

        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect, contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        // 敵が動いた際にエフェクトも追従して不自然さをなくすため
        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
    }

    // Input: Collision objectWeHit / Output: なし / Side Effects: 弾痕エフェクトの生成
    void CreatBulletImpactEffect(Collision objectWeHit)
    {
        // 衝突した表面の向きに合わせて弾痕を貼り付けるため
        ContactPoint contact = objectWeHit.contacts[FIRST_CONTACT_INDEX];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab, contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        // 動く壁などに当たった場合でも弾痕が置いてけぼりにならないようにするため
        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
