using UnityEngine;

public class DiggableResource : MonoBehaviour
{
    [Header("Resource State")]
    public int digRequiredHits = 5; // 완전히 캐는 데 필요한 총 횟수
    [HideInInspector] public int currentHits = 0;
    [HideInInspector] public bool isFullyDug = false;

    [Header("Animation & Rewards")]
    public float maxRiseHeight = 2f; // 광물이 땅에서 최대로 올라올 높이
    [HideInInspector] public Vector3 initialPosition;   // 광물이 솟아나기 시작할 기준 위치

    [Header("Drop Items")]
    [Tooltip("Resource > Plate 프리팹들")]
    public GameObject dropItemPrefab;   // 아직 뭘 드랍할지는 연결 안 됨

    public int minDropCount = 3;
    public int maxDropCount = 5;    // 이건 탐지기 레벨에 따라 바뀌도록 탐지기의 변수를 가져와야 함

    public float popForce = 3.0f;   // 퐁 튀어나오는 힘
    public bool isCurrentlyDiggable { get; private set; } = false;



    void Start()
    {
        initialPosition = this.transform.localPosition;
    }

    public void SetDiggableStatus(bool status)
    {
        isCurrentlyDiggable = status;
    }

    // Shovel.cs에서 삽질 이벤트가 발생할 때마다 호출됨
    public void DigHit()
    {
        if (isFullyDug) return;

        currentHits++;

        // 광물이 조금씩 올라오는 로직
        float progress = (float)currentHits / digRequiredHits;
        float currentHeight = progress * maxRiseHeight;
        this.transform.localPosition = initialPosition + Vector3.up * currentHeight;    // 수직 위로 조금씩 위치 이동

        if (currentHits >= digRequiredHits)
        {
            isFullyDug = true;

            BreakAndDropItem();
        }
    }

    private void BreakAndDropItem()
    {
        ResourceSpawnPoint parentPoint = GetComponentInParent<ResourceSpawnPoint>();
        if (parentPoint != null)
        {
            parentPoint.OnMined();  // 자기 부모 오브젝트인 ResourceSpawnPoint의 OnMined 함수를 호출하여 검은색으로 변경 & 자원 오브젝트 파괴
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Treasure"))
        {
            GameManager.Instance.AddTreasureCount();

            Destroy(gameObject);
        }

        if (dropItemPrefab != null)
        {
            int dropCount = Random.Range(minDropCount, maxDropCount + 1);

            Transform parentTransfrom = parentPoint.transform;

            for (int i = 0; i < dropCount; i++)
            {
                Vector3 randomPos = Random.insideUnitSphere * 0.5f;
                randomPos.y = 0.5f;

                GameObject droppedItem = Instantiate(dropItemPrefab, transform.position + randomPos, Quaternion.identity, parentTransfrom);
                droppedItem.transform.localScale = Vector3.one * 0.08f;

                Rigidbody rigidbody = droppedItem.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    Vector3 forceDirection = (Vector3.up + Random.insideUnitSphere).normalized;
                    rigidbody.AddForce(forceDirection * popForce, ForceMode.Impulse);
                }
            }
        }

        Destroy(this.gameObject);
    }

    // 플레이어가 채굴 완료된 광물과 상호작용하여 아이템을 얻는 함수
    public void CollectItem()
    {
        if (isFullyDug)
        {
            Destroy(this.gameObject); // 광물 오브젝트 파괴
        }
    }
}
