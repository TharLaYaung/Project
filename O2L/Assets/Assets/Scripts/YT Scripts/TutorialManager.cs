using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// ゲーム全体のチュートリアル進行を管理するマネージャークラス
/// 個別のTutorialオブジェクトをリストで保持し、順番に実行・完了を制御します

public class TutorialManager : MonoBehaviour
{
    // シーン内に存在するすべてのチュートリアル項目のリスト
    public List<Tutorial> Tutorials = new List<Tutorial>();

    [Header("UI要素の設定")]
    public TextMeshProUGUI expText;    // 現在のチュートリアルの説明文を表示するテキスト
    public TextMeshProUGUI panelText;  // パネル上に表示するテキスト（必要に応じて使用）
    public GameObject Panel;          // 全チュートリアル完了時に表示するパネル
    public GameObject crosshair;      // 画面中央のレティクル（完了時に非表示にする用）
    public GameObject hitmark;        // ヒットマーカー（完了時に非表示にする用）
    public GameObject scope;          // スコープUI（完了時に非表示にする用）

    private static TutorialManager instance;

   
    /// シングルトンプロパティ。どこからでも TutorialManager.Instance でアクセス可能
   
    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindAnyObjectByType<TutorialManager>();

            if (instance == null)
                Debug.Log("TutorialManagerが見つかりません");

            return instance;
        }
    }

    // 現在進行中のチュートリアル項目
    private Tutorial currentTutorial;

   
    /// 開始時に最初のチュートリアル（Order 0）を設定
  
    void Start()
    {
        SetNextTutorial(0);
    }

   
    /// 毎フレーム、現在アクティブなチュートリアルのクリア条件をチェック
   
    void Update()
    {
        if (currentTutorial)
            currentTutorial.CheckIfHappening();
    }

   
    /// 現在のチュートリアルを完了し、次の番号（Order）のチュートリアルへ進む
    
    public void CompleteTutorial()
    {
        SetNextTutorial(currentTutorial.Order + 1);
    }


    /// 指定された順番（Order）のチュートリアルを開始する
   
    public void SetNextTutorial(int currentOrder)
    {
        // 指定されたOrderを持つチュートリアルを検索
        currentTutorial = GetTutorialByOrder(currentOrder);

        // 次のチュートリアルが存在しない場合は、全工程完了とみなす
        if (!currentTutorial)
        {
            CompleteAllTutorials();
            return;
        }

        // 画面上の説明テキストを更新
        expText.text = currentTutorial.Explanation;
    }

 
    /// すべてのチュートリアルが終了した時の処理
    /// UIの表示切り替えやカーソルのロック解除を行います

    public void CompleteAllTutorials()
    {
        Panel.SetActive(true);        // 完了パネルを表示
        crosshair.SetActive(false);   // ゲームプレイ用UIを非表示
        hitmark.SetActive(false);
        scope.SetActive(false);

        // マウスカーソルを表示し、ウィンドウ内に留める設定に変更
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

   
    /// リストの中から指定されたOrder番号を持つTutorialオブジェクトを検索して返す
   
    public Tutorial GetTutorialByOrder(int Order)
    {
        for (int i = 0; i < Tutorials.Count; i++)
        {
            if (Tutorials[i].Order == Order)
                return Tutorials[i];
        }
        return null;
    }
}