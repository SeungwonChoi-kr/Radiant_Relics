using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [Header("Settings")]
    public float interactDistance = 3.0f; // 아이템을 주울 수 있는 거리
    public LayerMask pickupLayer; // 아이템 레이어 (DropItem으로 함)
    public GameObject handObject;

    private Camera playerCam;

    void Start()
    {
        playerCam = GetComponent<Camera>();
        // 만약 카메라에 스크립트를 안 붙였다면 자동으로 메인 카메라를 찾음
        if (playerCam == null) playerCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))    // 'F' 키 누르면 줍기
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        // 손은 있으니까 패스 && 손이 활성화되어있을 때만 줍기
        if (handObject != null && !handObject.activeInHierarchy)
        {
            Debug.Log("[PlayerPickUp] 맨손으로 주으십쇼");
            return;
        }

        // 레이캐스트 발사
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, pickupLayer))
        {
            // 부딪힌 물체가 PickupItem 스크립트를 가지고 있는지 확인
            PickupItem item = hit.collider.GetComponent<PickupItem>();

            if (item != null)
            {
                item.OnInteract(); // 아이템 획득
            }
        }
    }
}
