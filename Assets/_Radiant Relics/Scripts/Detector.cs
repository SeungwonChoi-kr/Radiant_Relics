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
    public float detectionRange = 20f; // 금속 탐지 최대 범위
    public LayerMask metalLayer;       // "금속"으로 감지할 오브젝트의 레이어 마스크

    [Header("UI Elements")]
    public Image[] signalBars; // 신호 세기를 표시할 9개의 UI 이미지 배열

    [Header("Audio Settings")]
    public AudioSource beepAudio;      // 경고음을 재생할 AudioSource 컴포넌트
    public float baseBeepInterval = 1.5f; // 기본 경고음 간격 (가장 멀리 있을 때)
    public float minBeepInterval = 0.5f;  // 최소 경고음 간격 (가장 가까이 있을 때)

    private float nextBeepTime = 0f; // 다음 경고음이 울릴 시간
    private Transform targetMetal;   // 현재 감지된 가장 가까운 금속 오브젝트의 Transform

    // 매 프레임마다 호출되는 Unity 생명주기 메서드
    void Update()
    {
        // 가장 가까운 금속을 탐지
        DetectClosestMetal();

        // 감지된 금속(targetMetal)이 있다면
        if (targetMetal)
        {
            // 금속 탐지기(이 스크립트가 붙은 오브젝트)와 금속 사이의 거리 계산
            float distance = Vector3.Distance(transform.position, targetMetal.position);

            // 거리를 0(멀리 있음) ~ 1(가까이 있음) 사이의 값으로 정규화합니다.
            // InverseLerp: detectionRange일 때 0, 0일 때 1을 반환합니다.
            float normalized = Mathf.InverseLerp(detectionRange, 0, distance);

            // 정규화된 값(강도)을 기반으로 UI 신호 막대 업데이트
            UpdateSignalBars(normalized);
            // 정규화된 값(강도)을 기반으로 경고음 처리
            HandleBeepSound(normalized);
        }
        else // 감지된 금속이 없다면
        {
            // 신호 막대를 0으로 (모두 끔) 업데이트
            UpdateSignalBars(0);
        }
    }

    /// <summary>
    /// detectionRange 내의 'metalLayer'에 속하는 물체 중 가장 가까운 것을 찾습니다.
    /// </summary>
    void DetectClosestMetal()
    {
        // 현재 위치를 중심으로 detectionRange 반경 내의 metalLayer에 속하는 모든 콜라이더를 감지
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, metalLayer);

        float closest = Mathf.Infinity; // 가장 가까운 거리를 저장할 변수 (초기값은 무한대)
        targetMetal = null; // 매번 탐지 시작 전 타겟을 초기화

        // 감지된 모든 콜라이더를 순회
        foreach (Collider c in hits)
        {
            // 탐지기와의 거리 계산
            float dist = Vector3.Distance(transform.position, c.transform.position);

            // 현재 거리가 이전에 저장된 '가장 가까운 거리(closest)'보다 작다면
            if (dist < closest)
            {
                closest = dist;          // 가장 가까운 거리 갱신
                targetMetal = c.transform; // 가장 가까운 물체를 타겟으로 설정
            }
        }
    }

    /// <summary>
    /// 신호 강도(intensity)에 따라 UI 신호 막대의 색상을 업데이트합니다.
    /// </summary>
    /// <param name="intensity">0.0 (약함) ~ 1.0 (강함) 사이의 신호 강도</param>
    void UpdateSignalBars(float intensity)
    {
        signalBars[8].color = new Color(0, 0.3f, 0);//탐지 범위 내에 없으면 꺼짐
        // 강도(0~1)에 총 막대 수(스위치 제외)를 곱하여 활성화할 막대의 개수를 계산 (반올림)
        int activeBars = Mathf.RoundToInt(intensity * (signalBars.Length -1));
        if (intensity > 0)
        {
            signalBars[8].color = Color.green;//탐지 범위내에 있으면 켜짐, index == 8은 스위치 역할
        }
        // 스위치(8번)를 제외한 모든 신호 막대를 순회
        for (int i = 0; i < (signalBars.Length -1); i++)
        {
            // 현재 막대의 인덱스(i)가 활성화할 개수(activeBars)보다 작으면
            if (i < activeBars)
            {
                signalBars[i].color = Color.green; // 활성 색상 (초록색)으로 변경
            }
            else
            {
                signalBars[i].color = new Color(0, 0.3f, 0); // 비활성 색상 (어두운 초록색)으로 변경
            }

            // 위 7줄의 if-else 구문은 아래 한 줄의 삼항 연산자로 대체할 수 있습니다:
            // signalBars[i].color = i < activeBars ? Color.green : new Color(0, 0.3f, 0);
        }
    }

    /// <summary>
    /// 신호 강도(intensity)에 따라 경고음 간격과 피치(음높이)를 조절하고 재생합니다.
    /// </summary>
    /// <param name="intensity">0.0 (약함) ~ 1.0 (강함) 사이의 신호 강도</param>
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
        Gizmos.color = Color.cyan; // 기즈모 색상을 청록색으로 설정
        // 현재 위치를 중심으로 detectionRange 크기의 와이어프레임 구를 그립니다.
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
