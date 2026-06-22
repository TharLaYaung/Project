using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;


/// NPCとの会話（ダイアログ）を管理するクラス
/// プレイヤーが接近した際にテキストを表示し、アニメーションや音声を再生します

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private Animator animator;     // NPCのアニメーター
    [SerializeField] private string[] lines;        // 会話文の配列
    [SerializeField] private bool repeat = false;    // 会話が終わった後に最初から繰り返すか
    private int currentLine = 0;                   // 現在表示している行のインデックス

    public TextMeshProUGUI dialogueText;            // 会話を表示するテキストUI
    public GameObject Panel;                       // 会話用パネル（必要に応じて使用）
    public Image textBaseImage;                    // テキストの背景画像

    
    /// インスタンス生成時の初期化処理
   
    void Awake()
    {
        // 子要素からテキストコンポーネントを取得し、初期状態では非表示にする
        dialogueText = GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.gameObject.SetActive(false);
        textBaseImage.gameObject.SetActive(false);
    }

    
    /// プレイヤーがNPCのコライダー（トリガー）に入った時の処理
    
    void OnTriggerEnter(Collider other)
    {
        // 接触したオブジェクトのタグが "Player" であるか確認
        if (other.CompareTag("Player"))
        {
            // 会話を表示し、背景画像を有効にする
            ShowDialogue();
            textBaseImage.gameObject.SetActive(true);

            // アニメーターの "Talk" パラメータを真にして会話モーションを開始
            animator.SetBool("Talk", true);

            // SoundManagerを通じてNPCのボイスを再生（二重再生を防止）
            if (SoundManager.Instance.npcChannel.isPlaying == false)
            {
                SoundManager.Instance.npcChannel.PlayOneShot(SoundManager.Instance.npcTalk);
            }
        }
    }

    
    /// 会話テキストを表示し、インデックスを進めるメソッド
    
    void ShowDialogue()
    {
        // まだ表示すべき行が残っている場合
        if (currentLine < lines.Length)
        {
            dialogueText.text = lines[currentLine];
            dialogueText.gameObject.SetActive(true);
            currentLine++; // 次に備えてインデックスを加算
        }
        // すべて読み終えた後、繰り返し設定が有効であればリセット
        else if (repeat)
        {
            currentLine = 0;
        }
    }

    
    /// プレイヤーがNPCから離れた時の処理
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // テキストと背景を非表示にする
            dialogueText.gameObject.SetActive(false);
            textBaseImage.gameObject.SetActive(false);

            // 会話アニメーションを停止し、音声を止める
            animator.SetBool("Talk", false);
            SoundManager.Instance.npcChannel.Stop();
        }
    }
}