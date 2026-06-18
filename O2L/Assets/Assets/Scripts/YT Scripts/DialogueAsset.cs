using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 会話テキストを外部アセットとして切り出し、再利用やデータ管理を容易にするため
[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    // 長文や改行を含むテキストがUnityエディタ上で入力しやすくなるようにするため
    [TextArea]
    public string[] dialogue;
}
