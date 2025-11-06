using UnityEngine;
using UnityEngine.UI;
//using System.Collections;

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
        playerMovement = GetComponent<PlayerMovement>();
        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleStamina();
        UpdateStaminaUI();
    }

    private void HandleStamina()
    {
        // 1. 달리기 스테미너 감소
        if (playerMovement.isRunning)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                playerMovement.ForceStopRunning();
            }
        }
        else
        {
            // 달리지 않을 때 회복
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }

        // Clamp 항상 유지
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    private void UpdateStaminaUI()
    {
        if (staminaFillImage != null)
            staminaFillImage.fillAmount = currentStamina / maxStamina;
    }

    //PlayerMovement에서 점프 시 스테미너 차감
    public void jumpReduceStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }


    // 외부에서 현재 스테미너 확인용
    public bool HasStamina(float amount)
    {
        return currentStamina >= amount;
    }
}
