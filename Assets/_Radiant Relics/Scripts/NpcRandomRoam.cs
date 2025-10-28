using UnityEngine;
using UnityEngine.AI; // NavMeshAgent를 사용하려면 이게 필수!

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NpcRandomRoam : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("배회 설정")]
    public float roamRadius = 15f;    // NPC가 이 반경 안에서만 배회합니다.
    public float minWaitTime = 3f;  // 목적지 도착 후 최소 대기 시간
    public float maxWaitTime = 7f;  // 목적지 도착 후 최대 대기 시간

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // NPC가 씬에 배치된 자신의 위치를 기준으로 배회하도록 설정
        agent.autoBraking = true; // 목적지에 가까워지면 속도 줄임

        // 첫 번째 랜덤 목적지로 이동 시작
        SetNewRandomDestination();
    }

    void Update()
    {
        // 1. 애니메이터에 현재 속도 전달
        float currentSpeed = agent.velocity.magnitude;
        animator.SetFloat("MoveSpeed", currentSpeed);

        // 2. 목적지에 도착했는지 확인
        // (!agent.pathPending: 경로 계산 중이 아닐 때)
        // (agent.remainingDistance <= agent.stoppingDistance: 남은 거리가 멈추는 거리보다 작을 때)
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // 3. 도착했다면, 잠시 대기 후 다음 목적지 설정
            // (agent.isStopped == false 일때만 실행해서 중복 호출 방지)
            if (!agent.isStopped)
            {
                agent.isStopped = true; // 이동 멈춤 (애니메이션은 'Idle'로 자동 전환됨)
                float waitTime = Random.Range(minWaitTime, maxWaitTime);
                Invoke(nameof(SetNewRandomDestination), waitTime); // n초 뒤에 SetNewRandomDestination 함수 호출
            }
        }
    }

    void SetNewRandomDestination()
    {
        // roamRadius 반경 내의 랜덤한 지점 계산
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position; // NPC의 현재 위치 기준

        // 계산된 랜덤 위치가 NavMesh 위에서 "갈 수 있는" 위치인지 확인
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            // "갈 수 있는" 유효한 위치라면, 그곳을 새 목적지로 설정
            agent.SetDestination(hit.position);
            agent.isStopped = false; // 다시 이동 시작 (애니메이션은 'Walking'으로 자동 전환됨)
        }
        else
        {
            // 혹시라도 유효한 위치를 못 찾으면, 1초 뒤에 다시 시도
            Invoke(nameof(SetNewRandomDestination), 1f);
        }
    }
}
