using UnityEngine;

public class Shovel : MonoBehaviour
{
    [Header("Digging Settings")]
    public float digDistance = 3f;      // 삽질이 유효한 최대 거리
    public LayerMask groundLayer;       // 땅 레이어 (삽질 유효성 확인용)
    public LayerMask resourceLayer;     // DiggableResource 오브젝트 레이어

    [Tooltip("XZ 좌표 비교 시 허용할 오차 범위 (작을수록 정밀함)")]
    public float xzTolerance = 0.7f;    // 70cm 오차 허용

    [Header("Performance Optimization")]
    [Tooltip("OverlapBox로 광물을 검색할 영역의 크기 (Y축은 깊이를 나타냄)")]
    public Vector3 searchBoxHalfExtents = new Vector3(10f, 10f, 10f); // X, Z축으로 10m, 깊이 10m 검색

    [Header("Action Cooldown")]
    [Tooltip("삽질 동작 간 최소 시간 간격 (초 단위)")]
    public float digCooldown = 0.5f; // 예시: 0.5초 딜레이
    private float nextDigTime = 0f;

    [Header("Audio Settings")]
    public AudioSource audioSource;    // 소리를 재생할 AudioSource 컴포넌트 연결
    public AudioClip digSoundClip;

    void Update()
    {
        if (Time.time < nextDigTime)
        {
            return;
        }

        // 마우스 좌클릭(Primary Action)이 발생했을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 2. 쿨다운 갱신: 삽질을 시작하면 다음 삽질 시간을 설정
            nextDigTime = Time.time + digCooldown;
            UsePrimary();
        }
    }

    // 플레이어가 삽질을 할 때 호출되는 함수
    public void UsePrimary()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, digDistance, groundLayer))
        {
            // Ray가 땅에 명중했습니다.
            DetectResourceAtHit(hit.point);
        }
        else
        {
            // Debug.Log("땅파기 불발: 유효하지 않은 목표");
        }
    }

    // 스폰 포인트(DiggableResource) 감지 및 이벤트 실행 (OverlapBox와 XZ 비교 조합)
    private void DetectResourceAtHit(Vector3 digPosition)
    {
        // 1. OverlapBox를 사용하여 플레이어 주변의 광물 Collider 목록을 가져옵니다.
        Vector3 searchCenter = transform.position;

        Collider[] nearbyResources = Physics.OverlapBox(
            searchCenter,
            searchBoxHalfExtents,
            Quaternion.identity,
            resourceLayer
        );

        bool resourceFound = false;

        // 2. 검색된 Collider 목록을 순회하며 XZ 좌표 일치 여부를 확인합니다.
        foreach (Collider collider in nearbyResources)
        {
            DiggableResource resource = collider.GetComponent<DiggableResource>();

            if (resource == null) continue; // DiggableResource 스크립트가 없으면 건너뜁니다.
            if (!resource.isCurrentlyDiggable)
            {
                Debug.Log("탐지되지 않았습니다.");
                continue; // 현재 삽질 가능한 상태가 아니면 건너뜁니다.
            }

            // 광물 오브젝트의 월드 좌표
            Vector3 resourcePos = resource.transform.position;

            // 땅을 찍은 XZ 좌표와 광물 오브젝트의 XZ 좌표를 비교 (Y좌표 무시)
            float xDiff = Mathf.Abs(digPosition.x - resourcePos.x);
            float zDiff = Mathf.Abs(digPosition.z - resourcePos.z);

            // X 및 Z 좌표 차이가 허용 오차 이내인지 확인
            if (xDiff <= xzTolerance && zDiff <= xzTolerance)
            {
                // XZ 좌표 일치 확인 (요구사항 충족)

                if (!resource.isFullyDug)
                {
                    resource.DigHit();
                    Debug.Log($"광물 발견! 삽질 ({resource.currentHits}/{resource.digRequiredHits})");
                    PlayDigSound();
                }
                else
                {
                    resource.CollectItem();
                    Debug.Log("이미 다 캔 광물입니다.");
                }

                resourceFound = true;
                break; // 정확한 위치의 광물을 찾았으므로 순회 중단
            }
        }

        if (!resourceFound)
        {
            // 정확한 위치에 스폰 포인트가 없어 땅파기 불발
            Debug.Log("땅파기 불발: 이 위치에는 광물이 묻혀 있지 않습니다.");
        }
    }
    private void PlayDigSound()
    {
        if (audioSource != null && digSoundClip != null)
        {
            // 한 번 재생이 완료될 때까지 다른 소리를 덮어쓰지 않고 재생
            audioSource.PlayOneShot(digSoundClip);
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 AudioClip이 Shovel 스크립트에 연결되지 않았습니다!");
        }
    }
}
