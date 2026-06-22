using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


/// ゲームの設定（難易度、操作感度、マップ選択、音量）を管理し、
/// PlayerPrefsを使用してデータの保存と読み込みを行うクラス

public class SettingsManager : MonoBehaviour
{
    [Header("UI要素 - ボタン")]
    public Button normalDifficultyButton; // ノーマル難易度設定ボタン
    public Button hardDifficultyButton;   // ハード難易度設定ボタン
    public Button saveButton;             // 設定保存ボタン
    public Button map1Button;             // マップ1選択ボタン
    public Button map2Button;             // マップ2選択ボタン
    public Button playButton;             // ゲーム開始ボタン

    [Header("UI要素 - スライダー")]
    public Slider smoothSpeedSlider;      // 視点移動の滑らかさ調整
    public Slider sensitivitySlider;      // マウス感度調整

    [Header("オーディオ設定")]
    public AudioMixer audioMixer;         // 音量調整用のオーディオミキサー

    [Header("現在の設定値")]
    [SerializeField] public float smoothSpeed;
    [SerializeField] public float sensitivity;
    [SerializeField] public string selectedMap;
    [SerializeField] private int zombiePerWave;
    [SerializeField] private float delayperWave;

    
    /// 開始時にデフォルト値を設定し、保存された設定をロードしてボタンのイベントを登録する
    
    void Start()
    {
        // 1. まずデフォルトとして「ノーマル難易度」の数値を代入
        SetNormalDifficulty();

        // デフォルトのマップ名
        selectedMap = "GameScene";

        // スライダーの最小・最大値をスクリプトから制御
        smoothSpeedSlider.minValue = 1f;
        smoothSpeedSlider.maxValue = 10f;

        sensitivitySlider.minValue = 1f;
        sensitivitySlider.maxValue = 5f;

        // 感度などの初期値（難易度以外の項目）
        smoothSpeed = 10f;
        sensitivity = 2f;

        // 2. 保存されている設定があればロード（難易度設定が保存されていればここで上書きされます）
        LoadSettings();

        // 各ボタンをクリックした時の処理（リスナー）を登録
        normalDifficultyButton.onClick.AddListener(SetNormalDifficulty);
        hardDifficultyButton.onClick.AddListener(SetHardDifficulty);

        // ラムダ式を使用して引数付きのメソッドを登録
        map1Button.onClick.AddListener(() => SelectMap("GameScene"));
        map2Button.onClick.AddListener(() => SelectMap("GameScene2"));

        playButton.onClick.AddListener(LoadSelectedMap);
        saveButton.onClick.AddListener(SaveSettings);
    }

    
    /// PlayerPrefsから保存された値を読み込み、変数とUIに反映させる
    
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("DelayPerWave"))
        {
            delayperWave = PlayerPrefs.GetFloat("DelayPerWave");
        }
        if (PlayerPrefs.HasKey("ZombiePerWave"))
        {
            zombiePerWave = PlayerPrefs.GetInt("ZombiePerWave");
        }
        if (PlayerPrefs.HasKey("SmoothSpeed"))
        {
            smoothSpeed = PlayerPrefs.GetFloat("SmoothSpeed");
            smoothSpeedSlider.value = smoothSpeed; // UIに反映
        }
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensitivity = PlayerPrefs.GetFloat("Sensitivity");
            sensitivitySlider.value = sensitivity; // UIに反映
        }
        if (PlayerPrefs.HasKey("SelectedMap"))
        {
            selectedMap = PlayerPrefs.GetString("SelectedMap");
        }
        else
        {
            // キーがない場合はデフォルト値を保存
            selectedMap = "GameScene";
            PlayerPrefs.SetString("SelectedMap", selectedMap);
            PlayerPrefs.Save();
        }

        Debug.Log("Settings Loaded. Difficulty: " + (zombiePerWave == 3 ? "Normal" : "Hard"));
    }

    
    /// UI（スライダーなど）の値を現在の設定値としてPlayerPrefsに保存する
    
    public void SaveSettings()
    {
        // スライダーの現在値を反映
        smoothSpeed = smoothSpeedSlider.value;
        sensitivity = sensitivitySlider.value;

        // PlayerPrefsに各値をセット
        PlayerPrefs.SetFloat("DelayPerWave", delayperWave);
        PlayerPrefs.SetInt("ZombiePerWave", zombiePerWave);
        PlayerPrefs.SetFloat("SmoothSpeed", smoothSpeed);
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);

        // データの書き込みを確定
        PlayerPrefs.Save();

        Debug.Log("Settings Saved Successfully");
    }

    
    /// オーディオミキサーの音量を変更する
    
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    
    /// 難易度：ノーマルを設定
   
    private void SetNormalDifficulty()
    {
        delayperWave = 10.0f;
        zombiePerWave = 3;
        Debug.Log("Difficulty set to Normal");
    }

    
    /// 難易度：ハードを設定
    
    private void SetHardDifficulty()
    {
        delayperWave = 7.0f;
        zombiePerWave = 5;
        Debug.Log("Difficulty set to Hard");
    }

    
    /// 読み込むマップ名を切り替える
    
    private void SelectMap(string mapName)
    {
        selectedMap = mapName;
        PlayerPrefs.SetString("SelectedMap", selectedMap);
        Debug.Log("Map selected: " + selectedMap);
    }

    
    /// 選択されているマップシーンをロードする
    
    public void LoadSelectedMap()
    {
        if (string.IsNullOrEmpty(selectedMap))
        {
            selectedMap = PlayerPrefs.GetString("SelectedMap", "GameScene");
        }

        Debug.Log("Loading scene: " + selectedMap);
        SceneManager.LoadScene(selectedMap);
    }
}