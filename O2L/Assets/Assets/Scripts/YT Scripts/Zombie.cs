using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ゾンビの基本ステータスを管理し、攻撃部位（手など）に情報を伝達するクラス

public class Zombie : MonoBehaviour
{
    // ゾンビの攻撃判定を持つ部位（手など）のスクリプトへの参照
    public ZombieHand zombieHand;

    // このゾンビ個体がプレイヤーに与えるダメージ量
    public int zombieDamage;


    private void Start()
    {
        // ゲーム開始時に、ゾンビ本体の設定ダメージを攻撃部位（手）の変数に代入する
        // これにより、手がプレイヤーに触れた際に正しいダメージが適用されるようになる
        zombieHand.damage = zombieDamage;
    }
}