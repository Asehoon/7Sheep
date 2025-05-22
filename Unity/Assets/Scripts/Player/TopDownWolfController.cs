using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TopDownWolfController : MonoBehaviour
{
    public float detectionRadius = 3f;
    public float patrolRadius = 10f;
    public Transform player;
    public TextMeshProUGUI Wolfdistance;

    private NavMeshAgent agent;
    private Animator anim;
    private bool isWaiting = false;
    private bool isPatrolling = false;
    public int patrolCount = 0;
    private bool playerDetected = false;
    public float speed = 3f;

    private float lastDistance = 0f;
    private float updateInterval = 0.2f;
    private float timeSinceLastUpdate = 0f;

    private enum State
    {
        Idle,
        Run,
        Chase,
        Attack,
        Dead
    }
    private State state;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.avoidancePriority = 40;

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
        UpdateAnimationDirection();
        anim.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);

        float currentDistance = Vector2.Distance(transform.position, player.position);
        UpdateWolfDistanceUI(currentDistance);

        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Chase:
                UpdateChase(currentDistance);
                break;
            case State.Attack:
                UpdateAttack(currentDistance);
                break;
        }
    }

    private void UpdateWolfDistanceUI(float distance)
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= updateInterval)
        {
            if (Mathf.Abs(distance - lastDistance) > 0.1f)
            {
                Wolfdistance.text = $"늑대와의 거리: {distance:F1}m";
                lastDistance = distance;
            }
            timeSinceLastUpdate = 0f;
        }
    }

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
            agent.speed = speed;
            anim.SetTrigger("Idle");
            patrolCount++;
            playerDetected = false;

            while (!agent.pathPending && agent.remainingDistance > 0.1f)
            {
                if (CanSeePlayer())
                {
                    state = State.Chase;
                    isPatrolling = false;
                    playerDetected = true;
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.5f);
        }

        if (playerDetected) yield break;
        isPatrolling = false;
    }

    private void UpdateChase(float distance)
    {
        if (!CanSeePlayer())
        {
            state = State.Idle;
            anim.SetTrigger("Idle");
            StartCoroutine(Patrol());
            return;
        }

        if (distance <= 2)
        {
            state = State.Attack;
        }
        else
        {
            agent.speed = speed;
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 stopPosition = player.position - direction * 1.5f;
            agent.SetDestination(stopPosition);
        }
    }

    private void UpdateAttack(float distance)
    {
        agent.speed = 0;

        if (!isWaiting)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(AttackDelay());
        }
    }

    private IEnumerator AttackDelay()
    {
        isWaiting = true;
        yield return new WaitForSeconds(0.5f);

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > 2)
        {
            state = State.Chase;
            anim.SetTrigger("Idle");
        }
        isWaiting = false;
    }

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer < detectionRadius;
    }

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

    private void UpdateAnimationDirection()
    {
        Vector3 velocity = agent.velocity;

        if (velocity.magnitude < 0.1f)
            return;

        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            anim.SetInteger("Direction", velocity.x > 0 ? 2 : 3);
        }
        else
        {
            anim.SetInteger("Direction", velocity.y > 0 ? 1 : 0);
        }
    }
}
