using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [System.Serializable]
    public class SourceMine
    {
        public string resourceName; // 여기에 그냥 이름 (예: 금, 철, 은)
        public Transform mineLocation;  // 여기에 위치만 갖고 있는 광산 빈 오브젝트 넣고
        public List<GameObject> resourcePrefabs; // 여기에 프리팹들 다 때려 넣고 (ResourceToUse에 있는 프리팹들)
    }

    public Transform playerTransform;

    [Header("탐지기 설정")]
    [Tooltip("스폰 포인트를 발견할 기본 확률 (0.0 = 0%, 1.0 = 100%)")]
    [Range(0f, 1f)]
    public float globalDiscoveryChance = 0.7f;  // 검은색으로 변하지 않는 확률
    public float detectionRadius = 20f; // 나중에 업그레이드 요소

    [Header("자원 정보")]
    public List<SourceMine> sourceMines;

    [Header("스폰 포인트 생성 설정")]
    public GameObject spawnPointPrefab; // 기즈모 있는 프리팹 (Prefabs > Resource에 있음; 드래그 드랍으로 인스펙터 창에서 연결됨)
    public List<Terrain> allTerrains;
    public int numberOfSpawns = 300;
    public float depthBelowSurface = 1.0f; // 땅에 처 박을 깊이
    public float minDistanceBetweenSpawns = 40.0f;  // 최소 여유 간격
    public float minAltitude = 30.0f;   // 최소 고도 (이 밑에는 안 생김; 바다 때문에)

    private List<ResourceSpawnPoint> allSpawnPoints = new List<ResourceSpawnPoint>();   // Spawn Points 폴더 안에 실제로 저장되는 오브젝트들
    private Transform spawnPointParent; // 게임 시작 시 하이라키에 생기는 Spawn Points 폴더라고 보면 됨
    private float checkInterval = 0.5f; // 0.5초에 한 번씩 거리 계산
    private float timeSinceLastCheck = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (playerTransform == null)    // 플레이어 설정
        {
            Debug.LogError("[ResourceManager] 플레이어 미 할당");
            return;
        }

        if (allTerrains == null || allTerrains.Count == 0)  // 지형 설정
        {
            Debug.LogError("No terrains found in the scene.");
            return;
        }

        if (sourceMines == null || sourceMines.Count == 0)  // 광산 리스트 설정
        {
            Debug.LogError("[ResourceManager] 'Source Mines' 리스트가 비어있음");
            return;
        }

        foreach (var mine in sourceMines)
        {
            if (mine.mineLocation == null || mine.resourcePrefabs == null || mine.resourcePrefabs.Count == 0)   // 각 광산을 순회하면서 내부에 값이 할당이 되어 있는지 확인
            {
                Debug.LogError($"[ResourceManager] '{mine.resourceName}' 광산의 'Mine Location'이나 'Resource Prefabs' 리스트가 비어있습니다.");
            }
        }
    }

    private void Update()
    {
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck >= checkInterval)    // 자원까지의 거리 계산을 0.5초에 한 번씩 하도록 설정 (checkInterval로)
        {
            UpdateNearbySpawnPoints();
            timeSinceLastCheck = 0f;
        }
    }

    private void UpdateNearbySpawnPoints()
    {
        foreach (var sp in allSpawnPoints)
        {
            if (sp != null)
            {
                float sqrDistance = (playerTransform.position - sp.transform.position).sqrMagnitude;
                bool isPlayerNear = sqrDistance <= detectionRadius * detectionRadius;
                sp.SetProximity(isPlayerNear);  // ResourceSpawnPoint에 있는 함수로 플레이어가 근처에 있는지 확인하는 함수
            }
        }
    }

    public GameObject GetResourceToSpawn(Vector3 spawnPosition)
    {
        // 가중치 계산용 Dictionary (이름, 가중치)
        Dictionary<SourceMine, float> weights = new Dictionary<SourceMine, float>();
        float totalWeight = 0f;

        // sourceMines 리스트를 순회하며 거리 가중치 계산
        foreach (var mine in sourceMines)
        {
            float distance = Vector3.Distance(spawnPosition, mine.mineLocation.position);
            float weight = 1f / (distance + 0.1f); // (0으로 나누기 방지)

            weights[mine] = weight; // Key를 string(이름) 대신 SourceMine 객체 자체로 사용
            totalWeight += weight;
        }

        // 랜덤 값 추첨
        float randomValue = Random.Range(0, totalWeight);

        // 당첨된 광물 결정
        foreach (var mine in weights.Keys)
        {
            if (randomValue < weights[mine])
            {
                int randomIndex = Random.Range(0, mine.resourcePrefabs.Count);
                return mine.resourcePrefabs[randomIndex];
            }
            randomValue -= weights[mine];
        }

        return null;
    }

    public void GenerateSpawnPoints()
    {
        if (spawnPointParent != null)
            Destroy(spawnPointParent.gameObject);

        spawnPointParent = new GameObject("Spawn Points").transform;
        allSpawnPoints.Clear();

        List<Vector3> placedPositions = new List<Vector3>();    // 로컬 변수로 이 함수가 실행되는 동안 위치 값들과 비교만을 위해 사용

        for (int i = 0; i < numberOfSpawns; i++)
        {
            int attempts = 0;
            while (attempts < 30)   // 최대 30번까지 시도를 하면서 최소 간격을 만족하는 위치를 탐색 -> 없으면 그냥 패스
            {
                attempts++;
                Terrain randomTerrain = allTerrains[Random.Range(0, allTerrains.Count)];    // 4개 중 1개의 터레인을 선택하여
                Vector3 terrainPos = randomTerrain.transform.position;  
                Vector3 terrainSize = randomTerrain.terrainData.size;

                float randomX = Random.Range(terrainPos.x, terrainPos.x + terrainSize.x);   // 랜덤한 x 좌표와
                float randomZ = Random.Range(terrainPos.z, terrainPos.z + terrainSize.z);   // 랜덤한 z 좌표를 추출
                float terrainHeight = randomTerrain.SampleHeight(new Vector3(randomX, 0, randomZ)); // 해당 x, z 좌표에서의 터레인 높이를 구함

                if (terrainHeight < minAltitude)    // 만약 최소 고도보다 낮은 위치라면 패스
                    continue;

                Vector3 potentialPosition = new Vector3(randomX, (terrainHeight - depthBelowSurface), randomZ);   // 터레인에서 depthBelowSurface만큼 박은 위치 (x, y, z 좌표 모두 결정된 상태)

                bool isTooClose = placedPositions.Any(pos => Vector3.Distance(potentialPosition, pos) < minDistanceBetweenSpawns);  // 최소 간격보다 더 가까운 경우 isTooClose가 true로 활성화
                if (!isTooClose)    // 충분히 거리가 있을 때만
                {
                    placedPositions.Add(potentialPosition); // 추가
                    GameObject newSpawnObj = Instantiate(spawnPointPrefab, potentialPosition, Quaternion.identity, spawnPointParent);
                    allSpawnPoints.Add(newSpawnObj.GetComponent<ResourceSpawnPoint>());
                    break;
                }
            }
        }
        Debug.Log(placedPositions.Count + "개의 스폰 포인트를 생성했습니다.");
    }

    public void RespawnAllPointsForNewDay()
    {
        Debug.Log("새로운 날이 되어 모든 자원 스폰 포인트를 재배치합니다.");
        GenerateSpawnPoints();
    }
}
