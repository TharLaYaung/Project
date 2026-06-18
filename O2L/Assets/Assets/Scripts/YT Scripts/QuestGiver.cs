using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// プレイヤーに対してクエストを提示し、受注の処理を行うクラス
public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public PLAYER player;

    [Header("UI Elements")]
    public GameObject questWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rewardText;

    void Awake()
    {
        // 意図せぬタイミングでUIが表示されるのを防ぐため初期状態で非表示にする
        if (questWindow != null)
        {
            questWindow.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenQuestWindow();
        }
    }

    /// クエストUIに必要な情報を反映させて画面に表示する
    public void OpenQuestWindow()
    {
        // UIコンポーネントの未割り当てによるNullReferenceExceptionを防ぐ
        if (quest == null || titleText == null || descriptionText == null || scoreText == null || questWindow == null)
        {
            return;
        }

        titleText.text = quest.title;
        descriptionText.text = quest.description;
        scoreText.text = quest.score.ToString();
        // rewardText.text = quest.reward.ToString();

        // プレイヤーがUIのボタンをクリックできるようにカーソルを解放する
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // 既に受注済みのクエストを再度受けられないようにする
        if (quest.isActive)
        {
            questWindow.SetActive(false);
        }
        else
        {
            questWindow.SetActive(true);
        }
    }

    /// 繧ｯ繧ｨ繧ｹ繝亥女豕ｨ繝懊ち繝ｳ縺梧款縺輔ｌ縺滓凾縺ｮ蜃ｦ逅・
    public void AcceptQuest()
    {
        // 繝励Ξ繧､繝､繝ｼ縺ｮ蜿ら・縺梧ｼ上ｌ縺ｦ縺・ｋ蝣ｴ蜷医ｂ繧ｨ繝ｩ繝ｼ繧貞・縺励※蜃ｦ逅・ｒ荳ｭ譁ｭ縺励∪縺・
        if (player == null)
        {
            return;
        }

        // 繧ｦ繧｣繝ｳ繝峨え繧帝哩縺倥ｋ
        // 窶ｻ Unity縺ｮ莉墓ｧ倅ｸ翫√が繝悶ず繧ｧ繧ｯ繝育ｴ譽・凾縺ｮ謖吝虚縺ｮ驛ｽ蜷医〒縲・.縲阪ｈ繧翫ｂ縲・= null縲阪・譁ｹ縺悟ｮ牙・縺ｧ縺・
        if (questWindow != null)
        {
            questWindow.SetActive(false);
        }

        // 繧ｯ繧ｨ繧ｹ繝医ｒ縲碁ｲ陦御ｸｭ縲咲憾諷九↓縺吶ｋ
        quest.isActive = true;

        // 繝励Ξ繧､繝､繝ｼ蛛ｴ縺ｫ縺薙・繧ｯ繧ｨ繧ｹ繝医ｒ逋ｻ骭ｲ縺励∬ｿｽ霍｡繧帝幕蟋九＆縺帙ｋ
        player.quest = quest;
    }

    /// 繝励Ξ繧､繝､繝ｼ縺碁屬繧後◆繧芽・蜍慕噪縺ｫ繧ｦ繧｣繝ｳ繝峨え繧帝哩縺倥ｋ
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (questWindow != null)
            {
                questWindow.SetActive(false);
            }
        }
    }

    /// 髢峨§繧九・繧ｿ繝ｳ縺ｪ縺ｩ縺ｧ謇句虚縺ｧ繧ｦ繧｣繝ｳ繝峨え繧帝哩縺倥ｋ蜃ｦ逅・
    public void Close()
    {
        if (questWindow != null)
        {
            questWindow.SetActive(false);
        }
    }
}
