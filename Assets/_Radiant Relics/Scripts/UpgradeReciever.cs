using UnityEngine;
using DevionGames.InventorySystem;
using Unity.VisualScripting;

public class UpgradeReceiver : MonoBehaviour
{
    public int level = 0;
    public ToolData targetToolData;

    // 창이 닫혀 있어도 아이템을 검색하는 함수
    private bool HasModule(string moduleName)
    {
        int amount = ItemContainer.GetItemAmount("Inventory", moduleName);
        return amount > 0;
    }

    // 모듈 사용 후 소비까지 하고 싶다면 true로 변경
    public bool consumeModule = false;

    private void UseModule(string moduleName)
    {
        if (!consumeModule) return;
        Item module = ItemContainer.GetItem("Inventory", moduleName);
        if (module != null)
        {
            ItemContainer.RemoveItem("Inventory", module, 1);
        }
    }

    // 업그레이드 실행 함수
    public void TryUpgrade()
    {
        Debug.Log($"[업그레이드 시도] 현재 레벨: {level}");

        if (level == 0 && HasModule("Upgrade module 1"))
        {
            UpgradeTo(1);
            UseModule("Upgrade module 1");
        }
        else if (level == 1 && HasModule("Upgrade_module 2"))
        {
            UpgradeTo(2);
            UseModule("Upgrade_module 2");
        }
        else if (level == 2 && HasModule("Upgrade module 3"))
        {
            UpgradeTo(3);
            UseModule("Upgrade module 3");
        }
        else if (level == 3 && HasModule("Final Upgrade module"))
        {
            UpgradeTo(4);
            UseModule("Final Upgrade module");
        }
        else
        {
            Debug.LogWarning("업그레이드 실패: 필요한 모듈이 없습니다.");
        }
    }

    private void UpgradeTo(int nextLevel)
    {
        Debug.Log($"업그레이드 성공! {level} → {nextLevel}");
        level = nextLevel;
        if (targetToolData != null)
        {
            targetToolData.Upgrade(nextLevel);
        }
    }
}
