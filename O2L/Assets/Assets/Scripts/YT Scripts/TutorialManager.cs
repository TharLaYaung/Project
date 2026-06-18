using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// チュートリアルの進行状態を一元管理し、正しい順序で実行させるためのクラスです。
public class TutorialManager : MonoBehaviour
{
    public List<Tutorial> tutorials = new List<Tutorial>();

    [Header("UI要素の設定")]
    public TextMeshProUGUI expText;
    public TextMeshProUGUI panelText;
    public GameObject panel;
    public GameObject crosshair;
    public GameObject hitmark;
    public GameObject scope;

    private static TutorialManager instance;

   
    /// Input: なし
    /// Output: TutorialManager (シングルトンインスタンス)
    /// Side Effects: インスタンスが存在しない場合、検索して割り当てます。
    /// どこからでもチュートリアルの進行状況を更新できるようにするため、シングルトンにします。
    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindAnyObjectByType<TutorialManager>();

            return instance;
        }
    }

    private Tutorial currentTutorial;

   
    /// 起動時に最初のチュートリアル状態を初期化するため実行します。
    void Start()
    {
        SetNextTutorial(0);
    }

    /// 条件達成を即座に反映させるため、毎フレーム進行状況を確認します。
    void Update()
    {
        if (currentTutorial)
            currentTutorial.CheckIfHappening();
    }

   
    /// Input: なし
    /// Output: なし
    /// Side Effects: 次のチュートリアルがセットされます。
    /// 次の段階へ進行させるため、現在の順序に1を足して呼び出します。
    public void CompleteTutorial()
    {
        SetNextTutorial(currentTutorial.order + 1);
    }


    /// Input: currentOrder (次のチュートリアルの順序番号)
    /// Output: なし
    /// Side Effects: UIが更新されます。進行するチュートリアルがない場合は完了処理を行います。
    /// 適切な順番で処理を呼び出すため、指定された順序の要素を検索して設定します。
    public void SetNextTutorial(int currentOrder)
    {
        currentTutorial = GetTutorialByOrder(currentOrder);

        if (!currentTutorial)
        {
            CompleteAllTutorials();
            return;
        }

        expText.text = currentTutorial.explanation;
    }

 
    /// Input: なし
    /// Output: なし
    /// Side Effects: UIが切り替わり、カーソル操作が解放されます。
    /// プレイヤーにチュートリアルの終了を知らせるため、専用のUIを表示します。
    public void CompleteAllTutorials()
    {
        panel.SetActive(true);
        crosshair.SetActive(false);
        hitmark.SetActive(false);
        scope.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

   
    /// Input: order (検索したいチュートリアルの順序番号)
    /// Output: Tutorial (該当するチュートリアルオブジェクト、なければnull)
    /// 順序の整合性を保証するため、リスト内から特定の順番のチュートリアルを検索します。
    public Tutorial GetTutorialByOrder(int order)
    {
        for (int i = 0; i < tutorials.Count; i++)
        {
            if (tutorials[i].order == order)
                return tutorials[i];
        }
        return null;
    }
}
