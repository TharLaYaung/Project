using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// メインメニューのBGM再生状態を管理し、重複再生を防ぐクラス
public class Mainmenubgm : MonoBehaviour
{
    public static Mainmenubgm Instance { get; set; }

    public AudioSource bgmmainmenuChannel;

    // 入力: なし, 出力: なし, 副作用: インスタンスの登録、重複破棄
    private void Awake()
    {
        // メニュー画面への遷移を繰り返した際にBGMが多重再生されるのを防ぐため
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // 入力: なし, 出力: なし, 副作用: BGMの再生
    private void Start()
    {
        // ゲーム起動時にユーザーへタイトル画面であることを聴覚的に伝えるため
        if (bgmmainmenuChannel != null)
        {
            bgmmainmenuChannel.Play();
        }
    }
}
