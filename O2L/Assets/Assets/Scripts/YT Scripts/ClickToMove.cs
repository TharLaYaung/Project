using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// プレイヤーの操作入力を受け取り、ナビゲーションシステムを介して移動させる
public class ClickToMove : MonoBehaviour
{
    private const int LEFT_MOUSE_BUTTON = 0;

    private NavMeshAgent navAgent;

    // NavMeshAgentは移動制御の要であるため、初期化時に取得する
    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // ユーザーの移動指示を検知するため、毎フレームマウスクリックを確認する
    private void Update()
    {
        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            // クリックした画面座標を3D空間の地点に変換し、移動目標を決定する
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                navAgent.SetDestination(hit.point);
            }
        }
    }
}
