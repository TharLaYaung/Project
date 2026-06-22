using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


/// プレイヤーの体力管理、ダメージ、回復、および死亡時の挙動を管理するクラス

public class PLAYER : MonoBehaviour
{
    [SerializeField] private GameObject Player;     // プレイヤーオブジェクト本体
    [SerializeField] public int MaxHP;              // 最大体力
    [SerializeField] public int HP = 100;           // 現在の体力
    [SerializeField] private Slider hpSlider;       // 体力を表示するUIスライダー

    public GameObject bloodyScreen;                 // ダメージを受けた際の画面赤縁エフェクト

    public Quest quest;                             // 進行中のクエストへの参照
    public TextMeshProUGUI playerHealthUI;          // 体力数値を表示するテキストUI
    public GameObject gameOverUI;                   // ゲームオーバー画面のUI

    public bool isDead;                             // 死亡状態フラグ

    
    /// 開始時の初期化処理
    
    private void Start()
    {
        // UIの初期表示設定
        playerHealthUI.text = $"HP:{HP}%";
        hpSlider.value = 1;

        // インタラクションマネージャーに自分自身を登録
        InteractionManager.Instance.player = this;
    }

    
    /// ダメージを受ける処理
    
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        // 体力が0以下になったら死亡処理
        if (HP <= 0)
        {
            print("Player Dead");
            PlayerDead();
            isDead = true;
        }
        else
        {
            // 生存している場合はヒットエフェクトと音声を再生
            print("Player Hit");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"HP/{HP}";

            // ダメージ時のボイス再生
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

   
    /// 体力を回復する処理
   
    public void Heal(int healAmount)
    {
        // 固定で10回復するように設定されています
        healAmount = 10;
        HP += healAmount;

        // 最大体力を超えないように制限
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
    }

    
    /// 死亡時のシーン遷移やUI制御
    
    private void PlayerDead()
    {
        // ゲームオーバーシーンの読み込みとカーソル設定
        SceneManager.LoadScene("GameOver");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // ゲームオーバーBGMの再生
        Gameoverbgm.Instance.bgmgameoverChannel.clip = Gameoverbgm.Instance.gameoverMusic;
        Gameoverbgm.Instance.bgmgameoverChannel.PlayDelayed(0.2f);

        // 操作を無効化
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        // 死亡アニメーションなどのためにアニメーターを有効化
        GetComponentInChildren<Animator>().enabled = true;

        // 体力UIを非表示
        playerHealthUI.gameObject.SetActive(false);

        // 画面のフェードアウト開始
        GetComponent<ScreenFader>().StartFade();

        // 一定時間後にゲームオーバーUIを表示するコルーチンを開始
        StartCoroutine(ShowGameOverUI());
    }

    
    /// 遅延してゲームオーバーUIを表示する
    
    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
    }

    
    /// ダメージ時に画面を一瞬赤くし、徐々に透明にするエフェクト
    
    private IEnumerator BloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponent<RawImage>();

        // アルファ値を1（不透明）にセット
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 1f;
        float elapsedTime = 0f;

        // 指定時間かけてアルファ値を0にする（フェードアウト）
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }

    
    /// 敵（ゾンビの手など）の攻撃範囲に入った時の判定
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            // 生存中であれば、当たったオブジェクトからダメージ値を取得して適用
            if (isDead == false)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }

    
    /// 毎フレームのステータス更新処理
    
    void Update()
    {
        // UIテキストの更新
        playerHealthUI.text = $"{HP}%";

        // 体力スライダーの値を更新（割合計算）
        hpSlider.value = (float)HP / MaxHP;

        // 上限チェック
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }

        // クエストの目的達成チェック
        if (quest.isActive)
        {
            if (quest.goal.IsReached())
            {
                quest.Complete();
            }
        }
    }
}