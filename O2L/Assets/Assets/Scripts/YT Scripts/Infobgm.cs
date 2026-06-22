using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 情報画面（Info）やメニュー画面などのBGM再生を管理するクラス
/// シングルトンパターンを使用して、二重生成を防ぎつつどこからでもアクセス可能にします
/// </summary>
public class Infobgm : MonoBehaviour
{
    // 静的変数（Instance）に自分自身を格納し、他のクラスから Infobgm.Instance でアクセスできるようにします
    public static Infobgm Instance { get; set; }

    // BGMを再生するためのオーディオソース
    public AudioSource bgminfoChannel;

    // 再生するBGMのオーディオデータ
    public AudioClip infoMusic;

    
    /// オブジェクトが生成された瞬間に実行される処理
    
    private void Awake()
    {
        // すで他にInstanceが存在しているか確認
        if (Instance != null && Instance != this)
        {
            // すでに存在していれば、この新しいオブジェクトを破棄して重複を防ぐ
            Destroy(gameObject);
        }
        else
        {
            // インスタンスとして自分自身を登録
            Instance = this;
        }
    }

    
    /// オブジェクトが有効になった最初のフレームで実行される処理
    
    private void Start()
    {
        // 指定されたオーディオソースで、指定されたBGMを一回再生する
        // PlayOneShotは再生中の他の音を止めずに重ねて再生できるメソッドです
        if (bgminfoChannel != null && infoMusic != null)
        {
            bgminfoChannel.PlayOneShot(infoMusic);
        }
    }
}