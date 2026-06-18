using UnityEngine;
using System.Collections;

// 繝励Ξ繧､繝､繝ｼ縺ｫ繧｢繧､繝・Β縺ｮ蟄伜惠繧呈ｰ励▼縺九○繧九◆繧√∽ｸ贋ｸ九↓豬ｮ驕翫＆縺帙ｋ繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ繧定｡後≧
public class Itemfloater : MonoBehaviour
{
    private const string TAG_ITEMS = "Items";

    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();
    private GameObject items;
    private GameObject parentObject;

    // 豬ｮ驕翫い繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ縺ｮ蝓ｺ貅悶→縺ｪ繧句・譛滉ｽ咲ｽｮ繧定ｨ倬鹸縺吶ｋ
    void Start()
    {
        posOffset = transform.position;
        this.items = GameObject.FindWithTag(TAG_ITEMS);
    }

    // 繧｢繧､繝・Β縺瑚・辟ｶ縺ｫ貍ゅ≧讒伜ｭ舌ｒ陦ｨ迴ｾ縺吶ｋ縺溘ａ縲√し繧､繝ｳ豕｢縺ｧ菴咲ｽｮ繧呈峩譁ｰ縺吶ｋ
    void Update()
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}
