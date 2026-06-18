using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// プレイヤーの生存状態およびUI情報を一元管理するクラス
public class PLAYER : MonoBehaviour
{
    private const int DEFAULT_HP = 100;
    private const int HEAL_AMOUNT = 10;
    private const float BGM_DELAY = 0.2f;
    private const float GAME_OVER_UI_DELAY = 1f;
    private const float BLOODY_SCREEN_DURATION = 1f;

    [SerializeField] private GameObject playerObj;
    [SerializeField] public int maxHp;
    [SerializeField] public int hp = DEFAULT_HP;
    [SerializeField] private Slider hpSlider;

    public GameObject bloodyScreen;
    public Quest quest;
    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    public bool isDead;

    private void Start()
    {
        playerHealthUI.text = $"HP:{hp}%";
        hpSlider.value = 1;

        // 他のシステムがプレイヤー情報にアクセスできるようにシングルトンへ登録する
        InteractionManager.Instance.player = this;
    }

    /// 外部からの攻撃によるダメージを処理する
    /// 入力: damageAmount (受けるダメージ量)
    /// 副作用: HPの減少、死亡判定、および被弾エフェクトの再生
    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;

        if (hp <= 0)
        {
            print("Player Dead");
            PlayerDead();
            isDead = true;
        }
        else
        {
            print("Player Hit");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"HP/{hp}";

            // プレイヤーに被弾を気付かせるためボイスを再生する
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    /// 体力を回復し、上限を超えないように補正する
    /// 入力: healAmount (回復量。現在は定数で上書きされる)
    public void Heal(int healAmount)
    {
        healAmount = HEAL_AMOUNT;
        hp += healAmount;

        // 意図しない過剰回復によりゲームバランスが崩れるのを防ぐ
        if (hp > maxHp)
        {
            hp = maxHp;
        }
    }

    private void PlayerDead()
    {
        // プレイヤーにゲーム終了を提示し、UI操作ができるようにカーソルを解放する
        SceneManager.LoadScene("GameOver");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Gameoverbgm.Instance.bgmgameoverChannel.clip = Gameoverbgm.Instance.gameoverMusic;
        Gameoverbgm.Instance.bgmgameoverChannel.PlayDelayed(BGM_DELAY);

        // 死亡後にプレイヤーが移動や視点操作を行えないように制限する
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        GetComponentInChildren<Animator>().enabled = true;
        playerHealthUI.gameObject.SetActive(false);
        GetComponent<ScreenFader>().StartFade();

        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        // 演出の都合上、フェードアウトを待機してからUIを表示させる
        yield return new WaitForSeconds(GAME_OVER_UI_DELAY);
        gameOverUI.gameObject.SetActive(true);
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (!bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponent<RawImage>();

        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float elapsedTime = 0f;

        // 視界の妨げになり続けないよう、時間経過で徐々にエフェクトを消す
        while (elapsedTime < BLOODY_SCREEN_DURATION)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / BLOODY_SCREEN_DURATION);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            // 死亡後の追加ダメージを無効化し、不必要な処理を省く
            if (!isDead)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }

    void Update()
    {
        playerHealthUI.text = $"{hp}%";
        hpSlider.value = (float)hp / maxHp;

        if (hp > maxHp)
        {
            hp = maxHp;
        }

        if (quest.isActive)
        {
            if (quest.goal.IsReached())
            {
                quest.Complete();
            }
        }
    }
}
