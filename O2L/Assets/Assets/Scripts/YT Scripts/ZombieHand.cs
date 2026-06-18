using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// プレイヤーとの接触判定時に、固定のダメージ量を伝達します。
public class ZombieHand : MonoBehaviour
{
    // 各ゾンビごとに固有のダメージを割り当てるため、インスペクターから設定可能にします。
    public int damage;
}
