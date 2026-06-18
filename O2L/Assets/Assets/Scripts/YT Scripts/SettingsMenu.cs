using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    // Input: なし, Output: なし, Side Effects: デバイス解像度を取得しドロップダウンを初期化する
    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Input: 解像度インデックス, Output: なし, Side Effects: 画面解像度を変更する
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Input: 音量(float), Output: なし, Side Effects: AudioMixerの設定を変更する
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    // Input: 品質インデックス, Output: なし, Side Effects: ゲームの描画品質レベルを変更する
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Input: フルスクリーンフラグ, Output: なし, Side Effects: フルスクリーンとウィンドウモードを切り替える
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
