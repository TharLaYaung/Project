using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// ゲーム内の会話テキストをデータとして保持するためのスクリプタブルオブジェクト
// プロジェクトウィンドウの右クリックメニュー（Create）からこのファイルを生成できるようにします
[CreateAssetMenu]
public class DialogueAsset : ScriptableObject
{
    
    /// 会話文の配列
    // [TextArea] 属性をつけることで、Unityのエディタ上で入力枠が広くなり、
    // 長文や改行を含むテキストが入力しやすくなります。
    [TextArea]
    public string[] dialogue;
}