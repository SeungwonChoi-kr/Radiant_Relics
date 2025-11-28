using UnityEngine;
using System.Collections;

public class GodWakeUp : MonoBehaviour
{
    public Animator animator;

    private string nightBoolParameter = "isNight";

    void Update()
    {
        // 밤이 됐고 아직 일어나는 애니메이션을 실행하지 않았으면
        bool currentNightState = GameManager.Instance.isNight;
        animator.SetBool(nightBoolParameter, currentNightState);
    }
}
