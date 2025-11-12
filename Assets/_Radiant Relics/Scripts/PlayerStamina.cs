using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerStamina : MonoBehaviour
{
    [Header("스테미너 설정")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public float staminaDrainRate = 25f; // 초당 달리기 소모
    public float staminaRegenRate = 15f;  // 초당 회복

    [Header("점프 스테미너 설정")]
    public float jumpCost = 20f; // 점프 시 소모 스테미너

    [Header("UI 설정")]
    public Image staminaFillImage;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        currentStamina = maxStamina;

        if (playerMovement == null)
        {
            Debug.LogError("[PlayerStamina] PlayerMovement 컴포넌트를 찾지 못했습니다!");
        }
        else
        {
            Debug.Log("[PlayerStamina] PlayerMovement 연결 완료");
        }
    }

    void Update()
    {
        HandleStamina();
        UpdateStaminaUI();
    }

    private void HandleStamina()
    {
        // PlayerMovement 상태 확인
        if (playerMovement == null)
        {
            Debug.LogError("[PlayerStamina] playerMovement가 null입니다. Start()에서 연결이 실패한 것 같습니다.");
            return;
        }

        // 1. 달리기 스테미너 감소
        if (playerMovement.isRunning)
        {
            float before = currentStamina;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

            Debug.Log($"[PlayerStamina] 달리는 중 → 스테미너 감소: {before:F2} → {currentStamina:F2}");

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                Debug.Log("[PlayerStamina] 스테미너가 0이 되어 달리기 강제 중단");
                playerMovement.ForceStopRunning();
            }
        }
        else
        {
            // 달리지 않을 때 회복
            float before = currentStamina;
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

            Debug.Log($"[PlayerStamina] 회복 중: {before:F2} → {currentStamina:F2}");
        }
    }

    private void UpdateStaminaUI()
    {
        if (staminaFillImage != null)
        {
            staminaFillImage.fillAmount = currentStamina / maxStamina;
        }
        else
        {
            Debug.LogWarning("[PlayerStamina] staminaFillImage가 설정되어 있지 않습니다.");
        }
    }

    // PlayerMovement에서 점프 시 스테미너 차감
    public void jumpReduceStamina(float amount)
    {
        float before = currentStamina;
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        Debug.Log($"[PlayerStamina] 점프 스테미너 차감: {before:F2} → {currentStamina:F2}");
    }

    // 외부에서 현재 스테미너 확인용
    public bool HasStamina(float amount)
    {
        bool result = currentStamina >= amount;
        Debug.Log($"[PlayerStamina] 점프 가능 여부 확인: {result} (현재: {currentStamina:F2}, 필요: {amount})");
        return result;
    }
}
