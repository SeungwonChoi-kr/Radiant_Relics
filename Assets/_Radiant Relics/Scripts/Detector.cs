using UnityEngine;
using UnityEngine.UI; // Unity UI 요소를 사용하기 위해 필요 (Image)
using System.Collections.Generic; // List와 같은 자료구조를 사용하기 위해 필요 (이 코드에서는 사용되지 않았지만 일반적)

/// <summary>
/// 금속 탐지기 스크립트.
/// 일정 범위 내에서 "metalLayer"에 해당하는 물체를 감지하고,
/// 가장 가까운 물체와의 거리에 따라 UI 신호 막대와 경고음을 조절합니다.
/// </summary>
public class MetalDetector : MonoBehaviour
{
    [Header("Sensor Settings")]
    public float initdetectionRange = 20; // 금속 탐지 최대 범위
    public LayerMask metalLayer;       // "금속"으로 감지할 오브젝트의 레이어 마스크

    [Header("UI Elements")]
    public Image[] signalBars; // 신호 세기를 표시할 3개의 UI 이미지 배열

    [Header("Audio Settings")]
    public AudioSource beepAudio;      // 경고음을 재생할 AudioSource 컴포넌트
    public float baseBeepInterval = 1.5f; // 기본 경고음 간격 (가장 멀리 있을 때)
    public float minBeepInterval = 0.5f;  // 최소 경고음 간격 (가장 가까이 있을 때)

    private ToolData parentToolData; // 레벨 업 정보를 받을 상위 ToolData 참조
    private float nextBeepTime = 0f; // 다음 경고음이 울릴 시간
    private Transform targetMetal;   // 현재 감지된 가장 가까운 금속 오브젝트의 Transform

    private DiggableResource currentDiggableResource; // 현재 탐지된 DiggableResource 참조
    private int treasureLayerIndex = -1;

    private void Awake()
    {
        parentToolData = GetComponentInParent<ToolData>();
        if (parentToolData == null)
        {
            Debug.LogError("Awake(): 상위 계층에서 ToolData 컴포넌트를 찾을 수 없습니다.");
        }
        treasureLayerIndex = LayerMask.NameToLayer("Treasure");
        if (treasureLayerIndex == -1)
        {
            Debug.LogWarning("레이어 'Treasure'를 찾을 수 없습니다. 레이어 이름을 확인해주세요.");
        }

    }

    // 매 프레임마다 호출되는 Unity 생명주기 메서드
    void Update()
    {
        // 탐지 범위 계산 (업그레이드 레벨에 따라 증가)
        float detectionRange = initdetectionRange;
        if (parentToolData.level == 4)
        {
            detectionRange = 80;
            metalLayer = 1 << treasureLayerIndex;
            //Debug.Log($"[DEBUG] 레벨 4 감지: 레이어 변경 완료! ({metalLayer.value}) 이제 Treasure 탐지 가능합니다. (Index: {treasureLayerIndex})");
        }
        else
        {
            detectionRange += 5 * parentToolData.level;
        }


            // 가장 가까운 금속을 탐지
            DetectClosestMetal(detectionRange);

        // 감지된 금속(targetMetal)이 있다면
        if (targetMetal)
        {
            // 금속 탐지기(이 스크립트가 붙은 오브젝트)와 금속 사이의 거리 계산
            float distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(targetMetal.position.x, targetMetal.position.z)
        );

            // 강도 정규화: 0.7m 이내에서 1.0을 초과하도록 InverseLerp 사용
            float normalized = Mathf.InverseLerp(detectionRange, 0.7f, distance);
            // 0.0 ~ 1.0 사이로 클램핑
            float clampedNormalized = Mathf.Clamp01(normalized);

            // 정규화된 값(강도)을 기반으로 UI 신호 막대 업데이트
            UpdateSignalBars(clampedNormalized, detectionRange);
            // 정규화된 값(강도)을 기반으로 경고음 처리
            HandleBeepSound(clampedNormalized);

            //탐지 여부 확인
            if (currentDiggableResource != null)
            {
                // UI가 전부 켜지고 아직 캘 수 있는 상태가 아닐 때만 true로 설정합니다.
                if ((normalized >= 1.0f) && (!currentDiggableResource.isCurrentlyDiggable))
                {
                    Debug.Log($"자원 탐지 완료! ({targetMetal.name}) 이제 삽질 가능합니다.");
                    currentDiggableResource.SetDiggableStatus(true);
                }
            }
        }
        else // 감지된 금속이 없다면
        {
            // 신호 막대를 0으로 (모두 끔) 업데이트
            UpdateSignalBars(0, detectionRange);
        }
    }

    /// <summary>
    /// detectionRange 내의 'Layer'에 속하는 물체 중 가장 가까운 것을 찾습니다.
    /// </summary>
    void DetectClosestMetal(float range)
    {
        // 현재 위치를 중심으로 detectionRange 반경 내의 metalLayer에 속하는 모든 콜라이더를 감지
        Collider[] hits = Physics.OverlapSphere(transform.position, range, metalLayer);

        float closest = Mathf.Infinity; // 가장 가까운 거리를 저장할 변수 (초기값은 무한대)
        Transform newTargetMetal = null;

        // 감지된 모든 콜라이더를 순회
        foreach (Collider c in hits)
        {
            // 탐지기와의 거리 계산
            float dist = Vector3.Distance(transform.position, c.transform.position);

            // 현재 거리가 이전에 저장된 '가장 가까운 거리(closest)'보다 작다면
            if (dist < closest)
            {
                closest = dist;          // 가장 가까운 거리 갱신
                newTargetMetal = c.transform; // 가장 가까운 물체를 타겟으로 설정
            }
        }

        if (targetMetal != newTargetMetal)
        {
            targetMetal = newTargetMetal;
            currentDiggableResource = targetMetal != null ? targetMetal.GetComponent<DiggableResource>() : null;
        }
    }
    /// <summary>
    /// 신호 강도(intensity)에 따라 UI 신호 막대의 색상을 업데이트합니다.
    /// </summary>
    void UpdateSignalBars(float intensity, float currentRange)
    {
        if (signalBars.Length < 2) return; // UI 요소가 충분하지 않으면 종료

        // 금속이 범위 내에 있을 때 활성화
        bool signalOn = (intensity > 0);
        signalBars[0].color = signalOn ? Color.green : Color.darkGreen;

        // 탐색 진행 막대 (2.5m 단위로 쪼개서 채우기) ---
        if (signalBars[1] != null && signalBars[1].type == Image.Type.Filled)
        {
            if (signalOn) // 신호가 켜졌을 때만 처리
            {
                // 1. 전체 탐지 범위 R을 2.5m 간격으로 쪼개서 총 칸수(n) 계산
                float totalTiles = currentRange / 2.5f; // 예: 80m/2.5m = 32칸

                // 2. 현재 강도(intensity)에 따라 활성화된 칸수 계산
                // intensity는 distance가 R에서 0.7m로 가까워질 때 0에서 1로 변함.
                int activeTiles = Mathf.FloorToInt(intensity * totalTiles);

                // 3. fillAmount를 활성화된 칸수만큼 단계적으로 설정 (1/n 간격)
                float steppedFillAmount = (float)activeTiles / totalTiles;

                signalBars[1].fillAmount = steppedFillAmount;
            }
            else
            {
                signalBars[1].fillAmount = 0; // 신호가 없으면 0으로 초기화
            }
        }
    }

    /// <summary>
    /// 신호 강도(intensity)에 따라 경고음 간격과 피치(음높이)를 조절하고 재생합니다.
    /// </summary>
    void HandleBeepSound(float intensity)
    {
        // 강도에 따라 경고음 간격을 계산합니다. (Lerp: 선형 보간)
        // intensity가 0이면 baseBeepInterval(1.0초), 1이면 minBeepInterval(0.2초)
        float interval = Mathf.Lerp(baseBeepInterval, minBeepInterval, intensity);

        // 현재 시간이 다음 경고음 재생 시간(nextBeepTime)을 지났다면
        if (Time.time >= nextBeepTime)
        {
            // 강도에 따라 피치(음높이)를 조절 (0이면 0.8, 1이면 1.2)
            beepAudio.pitch = Mathf.Lerp(0.8f, 1.2f, intensity);
            // 경고음 재생
            beepAudio.Play();
            // 다음 경고음 재생 시간을 현재 시간 + 계산된 간격(interval)으로 설정
            nextBeepTime = Time.time + interval;
        }
    }

    /// <summary>
    /// Unity 에디터의 Scene 뷰에서만 호출됩니다. (디버깅용)
    /// 선택되었을 때 탐지 범위를 시각적으로 표시합니다.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (parentToolData == null)
        {
            // OnDrawGizmosSelected는 Awake()보다 먼저 호출될 수 있습니다.
            return;
        }
        // 탐지 범위 계산 (업그레이드 레벨에 따라 증가)
        float detectionRange = initdetectionRange;
        detectionRange += 5 * parentToolData.level;
        Gizmos.color = Color.cyan; // 기즈모 색상을 청록색으로 설정
        // 현재 위치를 중심으로 detectionRange 크기의 와이어프레임 구를 그립니다.
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
