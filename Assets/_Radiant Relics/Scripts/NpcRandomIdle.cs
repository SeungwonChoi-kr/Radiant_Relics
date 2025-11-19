using UnityEngine;

public class NpcRandomIdle : MonoBehaviour
{
    private Animator animator;

    private float npcSpeed;
    private readonly int _MoveSpeedHash = Animator.StringToHash("MoveSpeed");

    private int nextIdleIndex = 1;
    private readonly int _IdleRandomIndex = Animator.StringToHash("IdleRandomIndex");

    private bool isIdleSet = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null )
        {
            Debug.LogError("[NpcRandomIdle] 애니메이터 연결 안 됨");
        }

        RandomizeNextIdle();
    }

    void Update()
    {
        if (animator == null) return;

        npcSpeed = animator.GetFloat(_MoveSpeedHash);

        if (npcSpeed > 0.1f && !isIdleSet)
        {
            RandomizeNextIdle();
            isIdleSet = true;
        }
        else if (npcSpeed < 0.1f)
        {
            isIdleSet = false;
        }
    }

    private void RandomizeNextIdle()
    {
        nextIdleIndex = Random.Range(1, 3); // 1 아니면 2

        animator.SetInteger(_IdleRandomIndex, nextIdleIndex);
    }
}
