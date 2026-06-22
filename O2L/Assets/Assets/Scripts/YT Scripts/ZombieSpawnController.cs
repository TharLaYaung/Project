using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// ゾンビのウェーブ制スポーン（生成）を管理するクラス
/// 全てのゾンビが倒されるとクールダウンを経て次のウェーブを開始します

public class ZombieSpawnController : MonoBehaviour
{
    // ウェーブ開始時の初期ゾンビ数
    public int initialZombiesPerWave = 3;
    // 現在のウェーブで生成すべきゾンビ数
    public int currentZombiesPerWave;

    // 1体ごとの生成間隔（秒）
    public float spawnDelay = 0.5f;

    // 現在のウェーブ数
    public int currentWave = 0;
    // ウェーブ間の休憩時間
    public float waveCooldown = 10.0f;

    // クールダウン中かどうかのフラグ
    public bool inCooldown;
    // クールダウンの残り時間を表示するためのカウンター
    public float cooldownCounter = 0;

    // 現在生存しているゾンビ（Enemyスクリプト）のリスト
    public List<Enemy> currentZombiesAlive;

    // スポーンさせるゾンビの種類（プレハブ）
    public GameObject zombiePrefab;
    public GameObject zombiePrefab2;
    public GameObject zombiePrefab3;

    // ゾンビに割り当てるヒットマーカー（UI演出用）
    public HitMarker hitMarker;

 
    /// ゲーム開始時に呼ばれる初期化処理
    
    private void Start()
    {
        // PlayerPrefs（保存された設定）からゾンビ数と生成遅延を取得
        initialZombiesPerWave = PlayerPrefs.GetInt("ZombiePerWave");
        currentZombiesPerWave = initialZombiesPerWave;
        spawnDelay = PlayerPrefs.GetFloat("DelayPerWave");

        // 最初のウェーブを開始
        StartNextWave();
    }

  
    /// 次のウェーブの準備を行う

    private void StartNextWave()
    {
        // 生存リストをクリア
        currentZombiesAlive.Clear();

        // ウェーブ数をカウントアップ
        currentWave++;

        // コルーチンを使用して時間差でゾンビを生成開始
        StartCoroutine(SpawnWave());
    }

    
    /// ゾンビを時間差で生成するコルーチン
    /// 3種類のプレハブを順番に生成します

    private IEnumerator SpawnWave()
    {
        // --- プレハブ1の生成ループ ---
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            // 生成位置にランダムな微調整を加える
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            // プレハブを実体化
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            // Enemyコンポーネントを取得し、初期設定を行ってリストに追加
            Enemy enemyScript = zombie.GetComponent<Enemy>();
            enemyScript.hm = hitMarker;
            currentZombiesAlive.Add(enemyScript);

            // 次の生成まで待機
            yield return new WaitForSeconds(spawnDelay);
        }

        // --- プレハブ2の生成ループ ---
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var zombie2 = Instantiate(zombiePrefab2, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie2.GetComponent<Enemy>();
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }

        // --- プレハブ3の生成ループ ---
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var zombie3 = Instantiate(zombiePrefab3, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie3.GetComponent<Enemy>();
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

 
    /// 毎フレームの更新処理
    /// ゾンビの死亡チェックとウェーブ終了判定を行います
 
    private void Update()
    {
        // 死亡したゾンビをリストから除外するための準備
        List<Enemy> zombieToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            // Enemyスクリプトの死亡フラグを確認
            if (zombie.isDead)
            {
                zombieToRemove.Add(zombie);
            }
        }

        // 実際にリストから削除
        foreach (Enemy zombie in zombieToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        zombieToRemove.Clear();

        // 全てのゾンビが倒され、かつクールダウン中でないなら次のウェーブへのカウントダウン開始
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        // クールダウン中のタイマー減算処理
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            // クールダウン中でなければタイマーを初期値にリセット
            cooldownCounter = waveCooldown;
        }
    }

   
    /// ウェーブ終了後の休憩時間を管理するコルーチン
    
    private IEnumerator WaveCooldown()
    {
        inCooldown = true;

        // 設定されたクールダウン時間だけ待機
        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;

        // 注意：現在のコードではここでゾンビ数が0にリセットされています
        // ウェーブごとに増やしたい場合は「currentZombiesPerWave += 増加数」などの処理が必要です
        currentZombiesPerWave *= 0;

        // 次のウェーブを開始
        StartNextWave();
    }
}