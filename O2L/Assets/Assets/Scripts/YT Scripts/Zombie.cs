using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 各部位の攻撃力を統一するため、本体のステータスを管理し伝達します。
public class Zombie : MonoBehaviour
{
    // 各部位の攻撃力に反映させるため、手のスクリプトを参照します。
    public ZombieHand zombieHand;

    // ゲームバランス調整のため、インスペクターからダメージ量を設定します。
    public int zombieDamage;


    /// Input: なし
    /// Output: なし
    /// Side Effects: zombieHandのダメージ値が更新されます。
    /// 攻撃判定時に正しいダメージを与えるため、起動時に設定を同期します。
    private void Start()
    {
        zombieHand.damage = zombieDamage;
    }
}
