using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// プレイヤーの環境に応じたゲーム体験を提供するため、各種設定の保持と永続化を行う
public class SettingsManager : MonoBehaviour
{
    private const string PREF_DELAY_PER_WAVE = "DelayPerWave";
    private const string PREF_ZOMBIES_PER_WAVE = "ZombiePerWave";
    private const string PREF_SMOOTH_SPEED = "SmoothSpeed";
    private const string PREF_SENSITIVITY = "Sensitivity";
    private const string PREF_SELECTED_MAP = "SelectedMap";

    private const string MAP_1 = "GameScene";
    private const string MAP_2 = "GameScene2";

    private const float NORMAL_DELAY = 10.0f;
    private const int NORMAL_ZOMBIES = 3;
    private const float HARD_DELAY = 7.0f;
    private const int HARD_ZOMBIES = 5;

    private const float MIN_SMOOTH_SPEED = 1f;
    private const float MAX_SMOOTH_SPEED = 10f;
    private const float MIN_SENSITIVITY = 1f;
    private const float MAX_SENSITIVITY = 5f;

    private const float DEFAULT_SMOOTH_SPEED = 10f;
    private const float DEFAULT_SENSITIVITY = 2f;

    [Header("UI要素 - ボタン")]
    public Button normalDifficultyButton;
    public Button hardDifficultyButton;
    public Button saveButton;
    public Button map1Button;
    public Button map2Button;
    public Button playButton;

    [Header("UI要素 - スライダー")]
    public Slider smoothSpeedSlider;
    public Slider sensitivitySlider;

    [Header("オーディオ設定")]
    public AudioMixer audioMixer;

    [Header("現在の設定値")]
    public float smoothSpeed;
    public float sensitivity;
    public string selectedMap;
    [SerializeField] private int zombiesPerWave;
    [SerializeField] private float delayPerWave;

    // Input: なし, Output: なし, Side Effects: UI初期化とリスナー登録、保存済み設定の読み込みを行う
    private void Start()
    {
        SetNormalDifficulty();
        selectedMap = MAP_1;

        smoothSpeedSlider.minValue = MIN_SMOOTH_SPEED;
        smoothSpeedSlider.maxValue = MAX_SMOOTH_SPEED;

        sensitivitySlider.minValue = MIN_SENSITIVITY;
        sensitivitySlider.maxValue = MAX_SENSITIVITY;

        smoothSpeed = DEFAULT_SMOOTH_SPEED;
        sensitivity = DEFAULT_SENSITIVITY;

        LoadSettings();

        normalDifficultyButton.onClick.AddListener(SetNormalDifficulty);
        hardDifficultyButton.onClick.AddListener(SetHardDifficulty);

        map1Button.onClick.AddListener(() => SelectMap(MAP_1));
        map2Button.onClick.AddListener(() => SelectMap(MAP_2));

        playButton.onClick.AddListener(LoadSelectedMap);
        saveButton.onClick.AddListener(SaveSettings);
    }

    // Input: なし, Output: なし, Side Effects: PlayerPrefsから値をロードしUIと変数に適用する
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey(PREF_DELAY_PER_WAVE))
        {
            delayPerWave = PlayerPrefs.GetFloat(PREF_DELAY_PER_WAVE);
        }
        if (PlayerPrefs.HasKey(PREF_ZOMBIES_PER_WAVE))
        {
            zombiesPerWave = PlayerPrefs.GetInt(PREF_ZOMBIES_PER_WAVE);
        }
        if (PlayerPrefs.HasKey(PREF_SMOOTH_SPEED))
        {
            smoothSpeed = PlayerPrefs.GetFloat(PREF_SMOOTH_SPEED);
            smoothSpeedSlider.value = smoothSpeed;
        }
        if (PlayerPrefs.HasKey(PREF_SENSITIVITY))
        {
            sensitivity = PlayerPrefs.GetFloat(PREF_SENSITIVITY);
            sensitivitySlider.value = sensitivity;
        }
        if (PlayerPrefs.HasKey(PREF_SELECTED_MAP))
        {
            selectedMap = PlayerPrefs.GetString(PREF_SELECTED_MAP);
        }
        else
        {
            selectedMap = MAP_1;
            PlayerPrefs.SetString(PREF_SELECTED_MAP, selectedMap);
            PlayerPrefs.Save();
        }

        Debug.Log("Settings Loaded. Difficulty: " + (zombiesPerWave == NORMAL_ZOMBIES ? "Normal" : "Hard"));
    }

    // Input: なし, Output: なし, Side Effects: UIの値を設定としてPlayerPrefsに保存する
    public void SaveSettings()
    {
        smoothSpeed = smoothSpeedSlider.value;
        sensitivity = sensitivitySlider.value;

        PlayerPrefs.SetFloat(PREF_DELAY_PER_WAVE, delayPerWave);
        PlayerPrefs.SetInt(PREF_ZOMBIES_PER_WAVE, zombiesPerWave);
        PlayerPrefs.SetFloat(PREF_SMOOTH_SPEED, smoothSpeed);
        PlayerPrefs.SetFloat(PREF_SENSITIVITY, sensitivity);
        PlayerPrefs.Save();

        Debug.Log("Settings Saved Successfully");
    }

    // Input: float (音量), Output: なし, Side Effects: AudioMixerのボリュームを変更する
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    // Input: なし, Output: なし, Side Effects: 難易度をノーマルに変更する
    private void SetNormalDifficulty()
    {
        delayPerWave = NORMAL_DELAY;
        zombiesPerWave = NORMAL_ZOMBIES;
        Debug.Log("Difficulty set to Normal");
    }

    // Input: なし, Output: なし, Side Effects: 難易度をハードに変更する
    private void SetHardDifficulty()
    {
        delayPerWave = HARD_DELAY;
        zombiesPerWave = HARD_ZOMBIES;
        Debug.Log("Difficulty set to Hard");
    }

    // Input: string (マップ名), Output: なし, Side Effects: 選択中のマップを更新し保存する
    private void SelectMap(string mapName)
    {
        selectedMap = mapName;
        PlayerPrefs.SetString(PREF_SELECTED_MAP, selectedMap);
        Debug.Log("Map selected: " + selectedMap);
    }

    // Input: なし, Output: なし, Side Effects: 選択中のマップシーンをロードする
    public void LoadSelectedMap()
    {
        if (string.IsNullOrEmpty(selectedMap))
        {
            selectedMap = PlayerPrefs.GetString(PREF_SELECTED_MAP, MAP_1);
        }

        Debug.Log("Loading scene: " + selectedMap);
        SceneManager.LoadScene(selectedMap);
    }
}
