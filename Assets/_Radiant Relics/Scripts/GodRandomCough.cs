using UnityEngine;
using System.Collections;

public class GodRandomCough : MonoBehaviour
{
    private string triggerName = "doCough";

    public Animator animator;
    public float checkInterval = 20.0f;  // 확률 체크를 하는 주기
    public float coughProbability = 0.1f; // 10% 확률로 기침

    void Start()
    {
        StartCoroutine(CheckCoughRoutine());
    }

    IEnumerator CheckCoughRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval); // 주기만큼 대기

            if (Random.value <= coughProbability)
            {
                animator.SetTrigger(triggerName);

                yield return new WaitForSeconds(2.0f);  // 기침하는 동안은 대기
            }
        }
    }
}
