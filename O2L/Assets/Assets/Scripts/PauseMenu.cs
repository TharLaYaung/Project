using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

// 繝励Ξ繧､繝､繝ｼ縺後＞縺､縺ｧ繧ゅご繝ｼ繝繧剃ｸｭ譁ｭ繝ｻ蜀埼幕縺ｧ縺阪ｋ繧医≧縺ｫ縺吶ｋ縺溘ａ縲√・繝ｼ繧ｺ讖溯・縺ｨUI繧堤ｮ｡逅・☆繧・
public class PauseMenu : MonoBehaviour
{
    private const float TIME_SCALE_NORMAL = 1f;
    private const float TIME_SCALE_PAUSED = 0f;

    public string mainMenuName = "MainMenu";
    public bool isPaused;
    public GameObject pauseScreen;
    public GameObject crosshair;
    public GameObject hitmark;

    public static PauseMenu instance;

    // 莉悶・繧ｹ繧ｯ繝ｪ繝励ヨ縺九ｉ繝昴・繧ｺ迥ｶ諷九ｒ蜿ら・繝ｻ謫堺ｽ懊＠繧・☆縺上☆繧九◆繧√∬・霄ｫ繧偵す繝ｳ繧ｰ繝ｫ繝医Φ縺ｨ縺励※逋ｻ骭ｲ縺吶ｋ
    private void Awake()
    {
        instance = this;
    }

    // 繧ｦ繧｣繝ｳ繝峨え螟悶・隱､謫堺ｽ懊ｒ髦ｲ縺弱▽縺､UI謫堺ｽ懊ｒ蜿ｯ閭ｽ縺ｫ縺吶ｋ縺溘ａ縲√き繝ｼ繧ｽ繝ｫ縺ｮ遘ｻ蜍慕ｯ・峇繧貞宛髯舌☆繧・
    public void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // 繝励Ξ繧､繝､繝ｼ縺九ｉ縺ｮ繝昴・繧ｺ蜈･蜉幄ｦ∵ｱゅｒ蜊ｳ蠎ｧ縺ｫ讀懃衍縺励※蜿肴丐縺輔○繧・
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnPause();
        }
    }

    // 繧ｲ繝ｼ繝縺ｮ荳ｭ譁ｭ繝ｻ蜀埼幕縺ｫ蜷医ｏ縺帙※縲∵凾髢馴ｲ陦後→UI縺ｮ陦ｨ遉ｺ迥ｶ諷九ｒ驕ｩ蛻・↓蜷梧悄縺輔○繧・
    public void PauseUnPause()
    {
        if (isPaused)
        {
            Cursor.visible = false;
            isPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = TIME_SCALE_NORMAL;

            if (crosshair != null) crosshair.SetActive(true);
            if (hitmark != null) hitmark.SetActive(true);
        }
        else
        {
            // 繝昴・繧ｺ繝｡繝九Η繝ｼ繧偵・繧ｦ繧ｹ縺ｧ謫堺ｽ懊〒縺阪ｋ繧医≧縺ｫ縺吶ｋ縺溘ａ縲√き繝ｼ繧ｽ繝ｫ繧定ｧ｣謾ｾ縺励※陦ｨ遉ｺ縺吶ｋ
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = TIME_SCALE_PAUSED;

            if (crosshair != null) crosshair.SetActive(false);
            if (hitmark != null) hitmark.SetActive(false);
        }
    }

    // 繧ｷ繝ｼ繝ｳ驕ｷ遘ｻ繧・い繝励Μ邨ゆｺ・凾縺ｫ諢丞峙縺帙★繧ｲ繝ｼ繝縺悟●豁｢縺励◆縺ｾ縺ｾ縺ｫ縺ｪ繧九・繧帝亟縺舌◆繧√∵凾髢薙ｒ豁｣蟶ｸ縺ｫ謌ｻ縺・
    public void Exit()
    {
        Time.timeScale = TIME_SCALE_NORMAL;

        if (!string.IsNullOrEmpty(mainMenuName))
        {
            SceneManager.LoadScene(mainMenuName);
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
