using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRadius = 3f; // 감지 범위
    public float patrolRadius = 10f; // 탐색 범위
    public Transform player; // 플레이어

    private NavMeshAgent agent;
    private Animator anim;
    private bool isWaiting = false;
    private bool isPatrolling = false;
    public int patrolCount = 0; // 탐색 횟수 카운트
    private bool playerDetected = false;


    private enum State
    {
        Idle,   // 정지 상태 (2초 대기)
        Run,    // 탐색 상태 (랜덤 이동)
        Chase,  // 추적 상태 (플레이어 감지 시)
        Attack, // 공격 상태
        Dead    // 죽은 상태
    }
    private State state;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.avoidancePriority = 40;  // **회피 우선순위 설정**

        if (agent == null || anim == null || player == null)
        {
            Debug.LogError("필수 컴포넌트가 할당되지 않았습니다.");
            return;
        }

        state = State.Run;
        anim.SetTrigger("Idle");

        StartCoroutine(Patrol());
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Chase:
                UpdateChase();
                break;
            case State.Attack:
                UpdateAttack();
                break;
        }
    }

    // **대기 상태 (2초 멈춤)**
    private void UpdateIdle()
    {
        agent.speed = 0;
        if (!isWaiting)
        {
            StartCoroutine(IdleWait());
        }
    }

    private IEnumerator IdleWait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(2f);
        patrolCount = 0;
        state = State.Run;
        anim.SetTrigger("Idle");
        isWaiting = false;
        isPatrolling = false;
        StartCoroutine(Patrol());
    }

    // **탐색 상태 (수시로 플레이어 체크)**
    private IEnumerator Patrol()
    {
        if (isPatrolling) yield break;

        isPatrolling = true;

        while (state == State.Run)
        {
            if (patrolCount >= 8)
            {
                state = State.Idle;
                anim.SetTrigger("Idle");
                isPatrolling = false;
                yield break;
            }

            Vector3 randomPoint = GetRandomNavMeshPoint(transform.position, patrolRadius);
            agent.SetDestination(randomPoint);
            agent.speed = 3f;
            anim.SetTrigger("Idle");
            patrolCount++;
            playerDetected= false; // 탐지 확인

            // **도착할 때까지 반복 확인**
            while (!agent.pathPending && agent.remainingDistance > 0.1f)
            {
                if (CanSeePlayer())  // **플레이어 감지 시 즉시 추적 모드로 변경**
                {
                    state = State.Chase;
                    isPatrolling = false;
                    playerDetected= true;
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);  // **0.1초마다 체크**
            }
            
            yield return new WaitForSeconds(0.5f); // **0.5초 쉬고 다음 탐색**
        }
        if (playerDetected) yield break;  // **플레이어를 감지하면 즉시 종료**
        isPatrolling = false;
    }

    // **추적 상태**
    private void UpdateChase()
    {
        if (!CanSeePlayer())
        {
            state = State.Idle;
            anim.SetTrigger("Idle");
            StartCoroutine(Patrol());
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= 2)
            state = State.Attack;
        else
        {
            agent.speed = 3f;
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 stopPosition = player.position - direction * 1.5f;
            agent.SetDestination(stopPosition);
        }
    }

    // **공격 상태**
    private void UpdateAttack()
    {
        agent.speed = 0;


        // 플레이어 HP 감소 (추가 필요)

        // player.hp --;

        if (!isWaiting)  // 딜레이 중복 실행 방지
        {
            anim.SetTrigger("Attack"); // 애니메이션 먼저 실행
            StartCoroutine(AttackDelay());
        }
    }

    // **공격 후 딜레이**
    private IEnumerator AttackDelay()
    {
        isWaiting = true;
        yield return new WaitForSeconds(0.5f); // 0.5초 대기

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > 2)
        {
            state = State.Chase;
            anim.SetTrigger("Idle");
        }
        isWaiting = false;
    }
    // **플레이어 감지 로직**
    private bool CanSeePlayer()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer >= detectionRadius) return false;

        else return true;
    }

    // **순찰 지역 랜덤 이동**
    private Vector3 GetRandomNavMeshPoint(Vector3 origin, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += origin;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return origin;
    }
}
