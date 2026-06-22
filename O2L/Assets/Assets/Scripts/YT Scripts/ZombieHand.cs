using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ゾンビの攻撃判定（手など）にアタッチし、プレイヤーに与えるダメージ量を保持するクラス

public class ZombieHand : MonoBehaviour
{
    // プレイヤーに接触した際に減らす体力の値
    // Unityのインスペクターから、ゾンビの種類ごとに異なるダメージ値を設定できます
    public int damage;
}