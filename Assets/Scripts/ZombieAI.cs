using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player;
    public float stoppingDistance = 1.6f;
    public float speed = 2.5f;
    public float angularSpeed = 300f;
    public float acceleration = 8f;

    [Header("Attack")]
    public float attackRange = 1.6f;
    public float attackCooldown = 1.0f;

    NavMeshAgent agent;
    Animator animator;
    ZombieStats stats;
    float nextAttackTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>() ?? gameObject.AddComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stats = GetComponent<ZombieStats>();

        if (animator) animator.applyRootMotion = false;

        agent.stoppingDistance = stoppingDistance;
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.isStopped = false;
    }

    void Update()
    {
        if (!player || !agent || !agent.isOnNavMesh) return;

        agent.SetDestination(player.position);

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRange)
        {
            agent.isStopped = true;

            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
                DealDamageToPlayer();
            }
        }
        else
        {
            agent.isStopped = false;
        }
    }

    void DealDamageToPlayer()
    {
        var hp = player.GetComponent<PlayerHealth>();
        if (!hp) return;

        int dmg = stats ? stats.damage : 5;
        hp.TakeDamage(dmg);
    }
}
