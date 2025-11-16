using UnityEngine;
using TMPro; // UI를 쓴다면 필요

public class ShopNpc : MonoBehaviour
{
    [Header("UI & Interaction")]
    public GameObject UI_Message; // 'F키' UI 텍스트

    // 애니메이션 종료 후 위치 오차 보정을 위한 변수
    [HideInInspector]
    public Vector3 initialPosition; // 처음 위치
    [HideInInspector]
    public Quaternion initialRotation; // 처음 방향

    private Animator animator;
    private bool playerInRange = false;     // [Parameter] 상점에 들어왔는지
    private bool isSittingNow = false;   // 'ShopNPC_Sit_Down' State에 진입했는지 확인하기 위한 플래그

    // 'ShopNPC_Sit_Down' State가 시작될 때의 (오차가 누적된) 위치와 방향
    private Vector3 driftedPosition;
    private Quaternion driftedRotation;

    // 문자열 비교보다 더 빠르게 비교할 수 있도록 State의 이름을 해시로 변환
    private readonly int hashStateSitDown = Animator.StringToHash("ShopNPC_Sit_Down");

    void Start()
    {
        animator = GetComponent<Animator>();

        // 시작과 동시에 위치와 방향을 저장
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // 'F키' 상호작용 UI 끄기 (아직 가까이 안 갔기 때문에)
        if (UI_Message != null)
        {
            UI_Message.SetActive(false);
        }
        else
        {
            // Debug.LogError("[ShopNPC] 'F키' 상호작용 UI 미할당");
        }
    }

    void Update()
    {
        // 플레이어가 범위 안에 있고 'F키'를 누르면
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            HandleInteraction();
        }
    }

    // 플레이어가 Trigger로 만들어진 Box Collidar 범위에 들어왔을 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[ShopNPC] 상점 입장");
            playerInRange = true;

            // Animator의 'PlayerInRange' 값을 true로 설정
            animator.SetBool("PlayerInRange", true);

            // 'F키' 상호작용 UI 켜기
            if (UI_Message != null)
            {
                UI_Message.SetActive(true);
            }
        }
    }

    // 플레이어가 Trigger로 만들어진 Box Collidar 범위에서 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[ShopNPC] 상점 퇴장");
            playerInRange = false;

            // Animator에게 'PlayerInRange' 값을 false로 설정
            animator.SetBool("PlayerInRange", false);

            // 'F키' 상호작용 UI 끄기
            if (UI_Message != null)
            {
                UI_Message.SetActive(false);
            }
        }
    }

    // 'F키'를 눌렀을 때
    private void HandleInteraction()
    {
        Debug.Log("[ShopNPC] Interact (F key Pressed)");

        int talkIndex = Random.Range(1, 3); // 1번과 2번 Talk 애니메이션 중 하나를 선택하기 위한 랜덤값

        animator.SetInteger("TalkIndex", talkIndex);    // 해당 값으로 애니메이션 분기
        animator.SetTrigger("DoInteract");              // DoInteract도 있어야 분기가 되므로 Trigger 활성화

        // 상호작용 중에는 UI 끄기
        if (UI_Message != null)
        {
            UI_Message.SetActive(false);
        }
    }

    // Animator에 'Apply Root Motion'이 체크되어 있다면 매 프레임마다 호출
    void OnAnimatorMove()
    {
        if (animator == null) return;

        // 현재 애니메이터의 State 정보 가져옴
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // 현재 State가 'ShopNPC_Sit_Down'이 맞는지 확인 (앉는 동작의 애니메이션임)
        if (currentState.shortNameHash == hashStateSitDown)
        {
            if (!isSittingNow)
            {
                isSittingNow = true;    // 앉는 중 플래그 ON
                driftedPosition = transform.position;   // 일어서는 과정에서 왼쪽으로 밀린 위치 오차를 한 번만 저장
                driftedRotation = transform.rotation;   // 일어서는 과정에서 생겼을지도 모르는 방향 오차를 한 번만 저장
                                                        // Animation Clip들에서 이미 Root Transform Rotation을 통해 애니메이션 중 회전을 고려하지 않게 설정했기 때문에 신경 쓰지 않아도 되긴 함
            }

            // 애니메이션 진행도에 따라 보정하기 위한 변수
            float progress = currentState.normalizedTime;

            // 'transform.position'을 직접 조작하여 'Apply Root Motion'의 활성화로 인해 생긴 오차를 덮어씀
            transform.position = Vector3.Lerp(driftedPosition, initialPosition, progress);
            transform.rotation = Quaternion.Slerp(driftedRotation, initialRotation, progress);
        }
        else
        {
            animator.ApplyBuiltinRootMotion();  // 'ShopNPC_Sit_Down' State가 아니라면 Animator에 만든대로 동작

            isSittingNow = false;   // 앉는 중 플래그는 다시 앉을 때를 위해 끔
        }
    }
}
