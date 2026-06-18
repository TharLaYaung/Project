using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 複数シーンでセーブ・ロード処理を跨いで利用するため、シングルトンとして管理する
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }

    // Input: なし, Output: なし, Side Effects: シングルトンの重複を破棄し、自身を永続化する
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }
}
