using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

/// プレイヤーとの対話機能を実装するためのクラス
/// 接近時に会話UIを表示し、適切な音声とモーションを再生する
public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string[] lines;
    [SerializeField] private bool repeat = false;
    private int currentLine = 0;

    public TextMeshProUGUI dialogueText;
    public GameObject panel;
    public Image textBaseImage;

    void Awake()
    {
        // 意図せぬタイミングでUIが表示されるのを防ぐため初期状態で非表示にする
        dialogueText = GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.gameObject.SetActive(false);
        textBaseImage.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowDialogue();
            textBaseImage.gameObject.SetActive(true);

            // キャラクターに生命感を与えるためモーションを再生する
            animator.SetBool("Talk", true);

            // 音声が重複して不自然に聞こえないようにする
            if (!SoundManager.Instance.npcChannel.isPlaying)
            {
                SoundManager.Instance.npcChannel.PlayOneShot(SoundManager.Instance.npcTalk);
            }
        }
    }

    /// 会話テキストの進行を制御する
    /// 副作用: UIテキストの更新と進行状況のインクリメント
    void ShowDialogue()
    {
        if (currentLine < lines.Length)
        {
            dialogueText.text = lines[currentLine];
            dialogueText.gameObject.SetActive(true);
            currentLine++;
        }
        else if (repeat)
        {
            currentLine = 0;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーが離れたらUIが画面に残り続けないように非表示にする
            dialogueText.gameObject.SetActive(false);
            textBaseImage.gameObject.SetActive(false);

            animator.SetBool("Talk", false);
            SoundManager.Instance.npcChannel.Stop();
        }
    }
}
