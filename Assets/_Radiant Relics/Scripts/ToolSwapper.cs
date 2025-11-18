using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToolSwapper : MonoBehaviour
{
    // --- 도구 관리 ---
    [Header("Tool Management")]
    [Tooltip("순서대로 도구 GameObject들을 연결하세요 (ToolHolder의 자식들).")]
    public List<GameObject> toolObjects = new List<GameObject>();

    private int currentToolIndex = 0;
    private int maxTools;

    // --- UI 관리 ---
    [Header("UI References")]
    [Tooltip("화면 좌하단에 도구 아이콘을 표시할 UI Image를 연결하세요.")]
    public Image currentToolIconImage;

    // --- 쿨다운 설정 ---
    [Header("Swap Cooldown")]
    [Tooltip("도구 스왑 간 최소 시간 간격 (초 단위)")]
    public float swapCooldown = 0.25f; // 기본값 0.25초
    private float nextSwapTime = 0f;

    void Start()
    {
        maxTools = toolObjects.Count;

        if (maxTools > 0)
        {
            // 초기화: 모든 도구 비활성화
            foreach (GameObject tool in toolObjects)
            {
                tool.SetActive(false);
            }
            // 첫 번째 도구 활성화 및 UI 업데이트
            SwapTool(currentToolIndex);
        }
        else
        {
            Debug.LogError("Tool list is empty! Check the ToolSwapper on the Player object.");
        }
    }

    void Update()
    {
        HandleScrollInput();
    }

    private void HandleScrollInput()
    {
        // 쿨다운 검사: 다음 스왑 시간 이전이면 입력 무시
        if (Time.time < nextSwapTime)
        {
            return;
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f && maxTools > 0)
        {
            // 스왑을 허용하면 다음 스왑 시간을 설정
            nextSwapTime = Time.time + swapCooldown;

            int newIndex = currentToolIndex;

            if (scrollInput > 0f) // 휠 업
            {
                newIndex++;
            }
            else if (scrollInput < 0f) // 휠 다운
            {
                newIndex--;
            }

            // 인덱스 순환 처리
            if (newIndex < 0)
            {
                newIndex = maxTools - 1;
            }
            else if (newIndex >= maxTools)
            {
                newIndex = 0;
            }

            if (newIndex != currentToolIndex)
            {
                currentToolIndex = newIndex;
                SwapTool(currentToolIndex);
            }
        }
    }

    // --- 핵심 스왑 및 UI 로직 ---

    private void SwapTool(int newIndex)
    {
        for (int i = 0; i < maxTools; i++)
        {
            // 개별 도구의 SetActive 상태 변경 (기능 스크립트 실행/중지 제어)
            toolObjects[i].SetActive(i == newIndex);
        }

        // UI 업데이트
        UpdateToolUI(toolObjects[newIndex]);
    }

    private void UpdateToolUI(GameObject selectedTool)
    {
        ToolData toolData = selectedTool.GetComponent<ToolData>();

        if (toolData != null && currentToolIconImage != null)
        {
            // 아이콘 변경 및 UI 활성화
            currentToolIconImage.sprite = toolData.toolIcon;
            currentToolIconImage.enabled = true;
        }
        else if (currentToolIconImage != null)
        {
            // 도구가 없거나 ToolData 누락 시 UI 숨김
            currentToolIconImage.sprite = null;
            currentToolIconImage.enabled = false;
        }
    }
}
