using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// 継続的なゲームプレイを提供するため、ウェーブ形式でゾンビを生成します。
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

 
    /// Input: なし
    /// Output: なし
    /// Side Effects: 設定値が読み込まれ、最初のウェーブが開始されます。
    /// 前回のプレイ設定を引き継ぐため、PlayerPrefsから値を読み込みます。
    private void Start()
    {
        // PlayerPrefs（保存された設定）からゾンビ数と生成遅延を取得
        initialZombiesPerWave = PlayerPrefs.GetInt("ZombiePerWave");
        currentZombiesPerWave = initialZombiesPerWave;
        spawnDelay = PlayerPrefs.GetFloat("DelayPerWave");

        // 最初のウェーブを開始
        StartNextWave();
    }

  
    /// Input: なし
    /// Output: なし
    /// Side Effects: currentWaveが増加し、SpawnWaveコルーチンが開始されます。
    /// 新しいウェーブを正しく開始するため、生存リストを初期化します。
    private void StartNextWave()
    {
        // 生存リストをクリア
        currentZombiesAlive.Clear();

        // ウェーブ数をカウントアップ
        currentWave++;

        // コルーチンを使用して時間差でゾンビを生成開始
        StartCoroutine(SpawnWave());
    }

    
    /// Input: なし
    /// Output: IEnumerator
    /// Side Effects: ゾンビが生成され、currentZombiesAliveに追加されます。
    /// 一度に大量のゾンビが出現するのを防ぐため、一定間隔で生成します。
    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie.GetComponent<Enemy>();
            enemyScript.hm = hitMarker;
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }

        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var zombie2 = Instantiate(zombiePrefab2, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie2.GetComponent<Enemy>();
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }

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

 
    /// Input: なし
    /// Output: なし
    /// Side Effects: 死亡したゾンビがリストから削除され、必要に応じて次のウェーブが開始されます。
    /// リアルタイムで戦況を把握するため、毎フレーム生存状態を確認します。
    private void Update()
    {
        List<Enemy> zombieToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombieToRemove.Add(zombie);
            }
        }

        foreach (Enemy zombie in zombieToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        zombieToRemove.Clear();

        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }
    }

   
    /// Input: なし
    /// Output: IEnumerator
    /// Side Effects: inCooldownが操作され、次のウェーブが準備されます。
    /// プレイヤーにインターバルを提供するため、一定時間の待機処理を行います。
    private IEnumerator WaveCooldown()
    {
        inCooldown = true;

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;

        currentZombiesPerWave *= 0;

        StartNextWave();
    }
}
