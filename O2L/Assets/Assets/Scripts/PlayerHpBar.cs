using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 繝励Ξ繧､繝､繝ｼ縺ｮ逕溷ｭ倡憾豕√ｒ隕冶ｦ夂噪縺ｫ莨昴∴繧九◆繧√？P繧旦I縺ｫ蜷梧悄縺輔○繧・
public class PlayerHpBar : MonoBehaviour
{
    private const string NAME_PLAYER = "Player";
    private const float SLIDER_MAX_VALUE = 1f;

    private GameObject parentObject;

    [SerializeField] private GameObject player;
    private Playercontroller playerScript;

    [SerializeField] private int maxHP;
    [SerializeField] private int hp;
    [SerializeField] private Slider hpSlider;

    // 繧ｲ繝ｼ繝髢句ｧ区凾縺ｫ繧ｹ繝・・繧ｿ繧ｹ縺ｮ蝓ｺ貅門､繧定ｨｭ螳壹☆繧九◆繧√√・繝ｬ繧､繝､繝ｼ諠・ｱ繧貞叙蠕励☆繧・
    void Start()
    {
        player = GameObject.Find(NAME_PLAYER);
        playerScript = player.GetComponent<Playercontroller>();

        hp = playerScript.pHp;
        maxHP = playerScript.maxHp;

        hpSlider.value = SLIDER_MAX_VALUE;
    }

    // 繝励Ξ繧､繝､繝ｼ縺ｮ陲ｫ繝繝｡繝ｼ繧ｸ繧・屓蠕ｩ繧貞叉蠎ｧ縺ｫUI縺ｸ蜿肴丐縺輔○繧九◆繧√∵ｯ弱ヵ繝ｬ繝ｼ繝HP繧貞酔譛溘☆繧・
    void Update()
    {
        // 螳溯｡御ｸｭ縺ｫ繝励Ξ繧､繝､繝ｼ繧ｹ繧ｯ繝ｪ繝励ヨ縺悟・逕滓・縺輔ｌ縺溷ｴ蜷医↓繧ょｯｾ蠢懊☆繧九◆繧√・・蠎ｦ蜿ら・繧貞叙蠕励☆繧・
        playerScript = player.GetComponent<Playercontroller>();

        hp = playerScript.pHp;
        maxHP = playerScript.maxHp;

        // 謨ｴ謨ｰ蜷悟｣ｫ縺ｮ蜑ｲ繧顔ｮ励↓繧医ｋ繧ｼ繝ｭ荳ｸ繧√ｒ髦ｲ縺弱∵ｭ｣遒ｺ縺ｪ蜑ｲ蜷医ｒ繧ｹ繝ｩ繧､繝繝ｼ縺ｫ驕ｩ逕ｨ縺吶ｋ縺溘ａfloat縺ｧ繧ｭ繝｣繧ｹ繝医☆繧・
        hpSlider.value = (float)hp / maxHP;

        // 蝗槫ｾｩ蜃ｦ逅・↑縺ｩ縺ｧHP縺梧怙螟ｧ蛟､繧剃ｸ雁屓縺｣縺ｦ荳崎・辟ｶ縺ｪ陦ｨ遉ｺ縺ｫ縺ｪ繧九・繧帝亟縺舌◆繧√∽ｸ企剞繧定ｨｭ縺代ｋ
        if (hp > maxHP)
        {
            hp = maxHP;
        }
    }
}
