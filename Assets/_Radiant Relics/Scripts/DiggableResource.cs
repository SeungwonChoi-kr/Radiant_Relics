using UnityEngine;

public class DiggableResource : MonoBehaviour
{
    [Header("Resource State")]
    public int digRequiredHits = 5; // 완전히 캐는 데 필요한 총 횟수
    [HideInInspector] public int currentHits = 0;
    [HideInInspector] public bool isFullyDug = false;

    [Header("Animation & Rewards")]
    public float maxRiseHeight = 2f; // 광물이 땅에서 최대로 올라올 높이

    // 광물이 솟아나기 시작할 기준 위치
    [HideInInspector] public Vector3 initialPosition;

    [Tooltip("금속 탐지기로부터 완전히 감지되어 현재 삽질 가능한 상태")]
    public bool isCurrentlyDiggable { get; private set; } = false;

    // 이 스크립트가 붙은 GameObject의 Transform을 직접 사용합니다.
    void Start()
    {
        initialPosition = this.transform.localPosition;
    }
    public void SetDiggableStatus(bool status)
    {
        isCurrentlyDiggable = status;
    }

    // 삽질 이벤트가 발생할 때마다 호출됨 (Shovel.cs에서 호출됨)
    public void DigHit()
    {
        if (isFullyDug) return;

        currentHits++;

        // 2. 광물이 조금씩 올라오는 로직
        float progress = (float)currentHits / digRequiredHits;
        float currentHeight = progress * maxRiseHeight;

        // 광물 오브젝트의 로컬 위치를 업데이트합니다.
        // Start()에서 설정된 initialPosition을 기준으로 위로 올라갑니다.
        this.transform.localPosition = initialPosition + Vector3.up * currentHeight;

        if (currentHits >= digRequiredHits)
        {
            isFullyDug = true;
            Debug.Log("광물 채굴 완료! 아이템을 수거할 수 있습니다.");
            OnDigComplete();
        }
    }

    private void OnDigComplete()
    {
        // 채굴 완료 시 실행되는 이벤트 (예: 이펙트)
    }

    // 플레이어가 채굴 완료된 광물과 상호작용하여 아이템을 얻는 함수
    public void CollectItem()
    {
        if (isFullyDug)
        {
            // 인벤토리 로직 실행

            Destroy(this.gameObject); // 광물 오브젝트 파괴
        }
    }
}
