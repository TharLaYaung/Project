using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// 繧ｲ繝ｼ繝�蜈ｨ菴薙・繧ｷ繝ｼ繝ｳ驕ｷ遘ｻ縲√き繝ｼ繧ｽ繝ｫ縺ｮ陦ｨ遉ｺ險ｭ螳壹∝推逕ｻ髱｢縺ｮBGM蜀咲函繧堤ｮ｡逅・☆繧九け繝ｩ繧ｹ

public class SceneDirector : MonoBehaviour
{
    // 繝励Ξ繧､繝､繝ｼ縺ｮ蜿ら・・育樟迥ｶ縺ｯ譛ｪ菴ｿ逕ｨ・・
    private GameObject player;

    // 繧ｲ繝ｼ繝�縺御ｸ譎ょ●豁｢荳ｭ縺九←縺・°繧貞愛螳壹☆繧矩撕逧・､画焚
    public static bool isPaused = false;

    
    /// 繧ｲ繝ｼ繝�譛ｬ邱ｨ・・oading繧ｷ繝ｼ繝ｳ・峨ｒ髢句ｧ九☆繧・
    
    public void Play()
    {
        // 繧ｿ繧､繝�繧ｹ繧ｱ繝ｼ繝ｫ繧偵Μ繧ｻ繝・ヨ縺励※繝輔Μ繝ｼ繧ｺ繧帝亟縺・
        Time.timeScale = 1f;
        // Loading繧ｷ繝ｼ繝ｳ縺ｸ驕ｷ遘ｻ
        SceneManager.LoadScene("Loading");

        // 繝励Ξ繧､荳ｭ縺ｫ驍ｪ鬲斐↓縺ｪ繧峨↑縺・ｈ縺・き繝ｼ繧ｽ繝ｫ繧帝撼陦ｨ遉ｺ縺ｫ縺励∽ｸｭ螟ｮ縺ｫ繝ｭ繝・け縺吶ｋ
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    /// 繝√Η繝ｼ繝医Μ繧｢繝ｫ繧ｷ繝ｼ繝ｳ繧帝幕蟋九☆繧・
   
    public void Tutorial()
    {
        // 繧ｿ繧､繝�繧ｹ繧ｱ繝ｼ繝ｫ繧偵Μ繧ｻ繝・ヨ縺励※繝輔Μ繝ｼ繧ｺ繧帝亟縺・
        Time.timeScale = 1f;
        // Tutorial繧ｷ繝ｼ繝ｳ縺ｸ驕ｷ遘ｻ
        SceneManager.LoadScene("Tutorial");

        // 繧ｫ繝ｼ繧ｽ繝ｫ繧帝撼陦ｨ遉ｺ縺ｫ縺励※繝ｭ繝・け縺吶ｋ
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    /// 險ｭ螳夂判髱｢・・ettings・峨ｒ髢九￥
    
    public void Settings()
    {
        // Settings繧ｷ繝ｼ繝ｳ縺ｸ驕ｷ遘ｻ
        SceneManager.LoadScene("Settings");

        // 繧ｷ繝ｳ繧ｰ繝ｫ繝医Φ・・nstance・臥ｵ檎罰縺ｧ險ｭ螳夂判髱｢逕ｨ縺ｮBGM繧貞・逕・

        // UI謫堺ｽ懊′蠢・ｦ√↑縺溘ａ繧ｫ繝ｼ繧ｽ繝ｫ繧定｡ｨ遉ｺ縺励√え繧｣繝ｳ繝峨え螟悶↓蜃ｺ縺ｪ縺・ｈ縺・宛髯舌☆繧・
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

   
    /// 繧｢繝励Μ繧ｱ繝ｼ繧ｷ繝ｧ繝ｳ繧堤ｵゆｺ・☆繧・
    
    public void Exit()
    {
        // 繧ｨ繝・ぅ繧ｿ荳翫〒縺ｮ螳溯｡御ｸｭ縺ｯ縲∝・逕溘Δ繝ｼ繝峨ｒ蛛懈ｭ｢縺輔○繧・
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // 繝薙Ν繝峨＆繧後◆繧｢繝励Μ繧ｱ繝ｼ繧ｷ繝ｧ繝ｳ繧堤ｵゆｺ・☆繧・
        Application.Quit();
    }

    
    /// 迴ｾ蝨ｨ驕ｸ謚槭＆繧後※縺・ｋ繝槭ャ繝励ｒ蜀崎ｪｭ縺ｿ霎ｼ縺ｿ縺励※蜀埼幕縺吶ｋ
    
    public void Restart()
    {
        // 繧ｿ繧､繝繧ｹ繧ｱ繝ｼ繝ｫ繧偵Μ繧ｻ繝・ヨ縺励※繝輔Μ繝ｼ繧ｺ繧帝亟縺・
        Time.timeScale = 1f;
        // PlayerPrefs縺ｫ菫晏ｭ倥＆繧後※縺・ｋ縲郡electedMap縲阪・蜷榊燕縺ｮ繧ｷ繝ｼ繝ｳ繧定ｪｭ縺ｿ霎ｼ繧€
        SceneManager.LoadScene(PlayerPrefs.GetString("SelectedMap", "GameScene"));

        // 繧ｫ繝ｼ繧ｽ繝ｫ繧帝撼陦ｨ遉ｺ縺ｫ縺励※繝ｭ繝・け縺吶ｋ
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    /// 繝｡繧､繝ｳ繝｡繝九Η繝ｼ・医ち繧､繝医Ν逕ｻ髱｢・峨↓謌ｻ繧・
    
    public void Menu()
    {
        // 繧ｿ繧､繝�繧ｹ繧ｱ繝ｼ繝ｫ繧偵Μ繧ｻ繝・ヨ縺励※繝輔Μ繝ｼ繧ｺ繧帝亟縺・
        Time.timeScale = 1f;
        // MainMenu繧ｷ繝ｼ繝ｳ縺ｸ驕ｷ遘ｻ
        SceneManager.LoadScene("MainMenu");

        // 繝｡繧､繝ｳ繝｡繝九Η繝ｼ逕ｨ縺ｮBGM繧貞・逕・

        // 繧ｫ繝ｼ繧ｽ繝ｫ繧定｡ｨ遉ｺ縺励√え繧｣繝ｳ繝峨え蜀・↓蛻ｶ髯舌☆繧・
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    
    /// 繧ｭ繝ｼ蜑ｲ繧雁ｽ薙※險ｭ螳夂判髱｢・・eySettings・峨ｒ髢九￥
   
    public void KeySettings()
    {
        // KeySettings繧ｷ繝ｼ繝ｳ縺ｸ驕ｷ遘ｻ
        SceneManager.LoadScene("KeySettings");

        // 繧ｭ繝ｼ險ｭ螳夂判髱｢逕ｨ縺ｮBGM繧ｯ繝ｪ繝・・繧偵そ繝・ヨ縺励※蜀咲函

        // 繧ｫ繝ｼ繧ｽ繝ｫ繧定｡ｨ遉ｺ縺励√え繧｣繝ｳ繝峨え蜀・↓蛻ｶ髯舌☆繧・
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    
    /// 諠・�ｱ繝ｻ繧ｯ繝ｬ繧ｸ繝・ヨ逕ｻ髱｢・・nfo・峨ｒ髢九￥
    
    public void Info()
    {
        // Info繧ｷ繝ｼ繝ｳ縺ｸ驕ｷ遘ｻ
        SceneManager.LoadScene("Info");

        // 諠・�ｱ逕ｻ髱｢逕ｨ縺ｮBGM繧ｯ繝ｪ繝・・繧偵そ繝・ヨ縺励※蜀咲函

        // 繧ｫ繝ｼ繧ｽ繝ｫ繧定｡ｨ遉ｺ縺励√え繧｣繝ｳ繝峨え蜀・↓蛻ｶ髯舌☆繧・
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
