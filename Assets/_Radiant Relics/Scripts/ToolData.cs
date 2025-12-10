using UnityEngine;
using DevionGames;
using System.Collections.Generic;

public class ToolData : MonoBehaviour
{
    public string toolName = "Default Tool";
    public Sprite toolIcon;

    [Header("Upgrade Settings")]
    public int level = 0; // 초기 상태

    // 텍스처를 바꿀 대상 오브젝트의 Renderer
    public Renderer targetRenderer;

    // 레벨 4가 되었을 때 적용할 새 텍스처
    public Texture2D levelTexture;

    //바꿀 아이콘
    public Sprite levelIcon;


    public void Upgrade(int s)
    {
        // 1. 레벨 증가
        this.level = s;
        Debug.Log($"{toolName} 레벨이 {this.level}로 업그레이드되었습니다.");

        // 2. 레벨 4 도달 및 텍스처가 아직 바뀌지 않은 경우 확인
        if (this.level == 4)
        {
            FormChange();
        }
    }

    // 도구의 외형(텍스처 및 아이콘) 변경 메서드
    private void FormChange()
    {
        // 대상 Renderer와 텍스처가 모두 할당되었는지 확인
        if (targetRenderer != null)
        {
            if (levelTexture != null)
            {
                // Material의 _BaseMap 텍스처를 교체
                targetRenderer.material.SetTexture("_BaseMap", levelTexture);
                Debug.Log($"텍스쳐가 변경되었습니다.");
            }
            else
            {
                Debug.LogError("levelFourTexture가 ToolData 인스펙터에 할당되지 않았습니다.");
            }

            // 아이콘 교체
            if (levelIcon != null)
            {
                // ToolData의 public 변수 toolIcon 값을 새 Sprite로 업데이트합니다.
                this.toolIcon = levelIcon;
            }
            else
            {
                Debug.LogError("Level 4 아이콘(Sprite)이 ToolData에 할당되지 않았습니다.");
            }
        }
        //targetRender가 비어있는 경우는 Level up이 필요 없는 경우
    }
}
