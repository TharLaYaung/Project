using UnityEngine;
using System.Collections;


/// アイテムを回転させながら上下にふわふわと浮遊させるスクリプト
/// プレイヤーが接触した際の回復・消去処理（コメントアウト部分）も含みます

public class Itemfloater : MonoBehaviour
{
    // --- ユーザー設定項目（インスペクターで調整可能） ---

    // 1秒間に回転する度数
    public float degreesPerSecond = 15.0f;

    // 上下浮遊の振幅（どれくらい上下に動くか）
    public float amplitude = 0.5f;

    // 浮遊の周期（速さ）
    public float frequency = 1f;

    // --- 内部計算用の変数 ---

    // オブジェクトの初期位置を保存する変数
    Vector3 posOffset = new Vector3();

    // 計算後の新しい位置を一時的に保持する変数
    Vector3 tempPos = new Vector3();

    // シーン内の「Items」タグが付いたオブジェクト（現在は参照取得のみ）
    private GameObject Items;

    // 親オブジェクトへの参照用変数
    private GameObject parentObject;

    
    /// 初期化処理
    
    void Start()
    {
        // 開始時の位置をオフセットとして記録（これを基準に上下に動かす）
        posOffset = transform.position;

        // シーン内から "Items" タグを持つオブジェクトを検索
        this.Items = GameObject.FindWithTag("Items");
    }

    
    /// フレームごとの更新（アニメーション処理）
    
    void Update()
    {
        // 1. Y軸を中心にオブジェクトを回転させる
        // Space.World を指定することで、親の向きに関わらず世界軸基準で回転
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // 2. サイン波（Sin）を用いて上下の位置を計算する
        // 初期位置（posOffset）をベースにする
        tempPos = posOffset;

        // Sin(時間 * パイ * 周波数) * 振幅 をY座標に加算
        // これにより、滑らかな往復運動（浮遊）が実現される
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        // 計算した座標を実際のオブジェクトの位置に適用
        transform.position = tempPos;
    }
}