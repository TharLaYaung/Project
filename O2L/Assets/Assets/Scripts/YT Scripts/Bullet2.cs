using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    // 弾のダメージ量
    public int bulletDamage;

    // 衝突したときに呼ばれる関数
    private void OnCollisionEnter(Collision objectWeHit)
    {
        // 壁に当たった場合
        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            // 弾痕エフェクトを生成
            CreatBulletImpactEffect(objectWeHit);
            // 弾を削除
            Destroy(gameObject);
        }

        // 木箱（Crates）に当たった場合
        if (objectWeHit.gameObject.CompareTag("Crates"))
        {
            print("Crate");

            // 自身または親オブジェクトからCratesコンポーネントを取得
            Crates crate = objectWeHit.gameObject.GetComponent<Crates>();
            if (crate == null) crate = objectWeHit.gameObject.GetComponentInParent<Crates>();

            if (crate != null)
            {
                crate.Shatter();
            }
        }

        // 敵に当たった場合
        if (objectWeHit.gameObject.CompareTag("Enemy"))
        {
            // 自身または親オブジェクトからEnemyコンポーネントを取得
            Enemy enemy = objectWeHit.gameObject.GetComponent<Enemy>();
            if (enemy == null) enemy = objectWeHit.gameObject.GetComponentInParent<Enemy>();

            if (enemy != null && enemy.isDead == false)
            {
                enemy.TakeDamage(bulletDamage);
            }

            // 血しぶきエフェクトを生成
            CreateBloodSprayEffect(objectWeHit);

            // 弾を削除
            Destroy(gameObject);
        }
    }

    // 血しぶきエフェクト生成
    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        // 衝突地点の情報を取得
        ContactPoint contact = objectWeHit.contacts[0];

        // 血しぶきエフェクトを生成
        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect, contact.point, // 衝突位置
            Quaternion.LookRotation(contact.normal)// 衝突面の向き
            );

        // 敵に追従するように親子関係を設定
        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
    }

    // 弾痕エフェクト生成
    void CreatBulletImpactEffect(Collision objectWeHit)
    {

        // 衝突地点の情報を取得
        ContactPoint contact = objectWeHit.contacts[0];

        // 弾痕エフェクトを生成
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,contact.point,// 衝突位置
            Quaternion.LookRotation(contact.normal)// 表面の向きに合わせる
            );

        // 壁などに貼り付けるため親に設定
        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
