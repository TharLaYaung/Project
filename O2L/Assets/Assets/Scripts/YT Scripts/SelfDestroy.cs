using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 不要なオブジェクトによるメモリ圧迫を防ぐため、一定時間後に自身を破棄する
public class SelfDestroy : MonoBehaviour
{
    [Header("破壊までの時間設定")]
    public float timeForDestruction;

    // Input: なし, Output: なし, Side Effects: オブジェクト破棄のコルーチンを開始する
    private void Start()
    {
        StartCoroutine(DestroySelf(timeForDestruction));
    }

    // Input: 待機時間(float), Output: IEnumerator, Side Effects: 指定時間経過後にGameObjectを破棄する
    private IEnumerator DestroySelf(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
