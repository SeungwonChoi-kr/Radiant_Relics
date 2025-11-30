using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName; // 디버깅용

    public void OnInteract()
    {
        if (itemName != null)
        {
            Debug.Log("[PickUpItem] " + itemName + " 획득");
        }

        // InventoryManager.Instance.AddItem(itemName, 1);  인벤토리 코드에서 추가하는 함수가 있는지 모르겠지만 이런 식으로 추가하면 될 듯

        // 오브젝트 삭제
        Destroy(gameObject);
    }
}
