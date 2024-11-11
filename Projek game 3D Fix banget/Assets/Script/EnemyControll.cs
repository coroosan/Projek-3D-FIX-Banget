using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Shoot, Explode }
    public EnemyState currentState;

    public Transform[] patrolPoints;
    public float patrolSpeed = 3.5f, chaseSpeed = 5f, detectionRadius = 20f, shootingDistance = 10f, explosionDistance = 2f;
    public int burstCount = 3;

    private Transform player;
    private int currentPatrolIndex;
    private float lastAttackTime, nextBurstShotTime;
    private bool hasExploded = false;
    private int burstShotsFired;

    public NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject explosionPrefab;

    private EnemyHealth enemyHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyHealth = GetComponent<EnemyHealth>();
        currentState = EnemyState.Patrol;
        currentPatrolIndex = 0;

        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (hasExploded || player == null || agent == null || animator == null) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        currentState = DetermineState(distanceToPlayer);
        UpdateState(distanceToPlayer);
    }

    // Tambahkan fungsi DetermineState di sini
    EnemyState DetermineState(float distanceToPlayer)
    {
        if (distanceToPlayer <= explosionDistance) return EnemyState.Explode;
        if (distanceToPlayer <= shootingDistance) return EnemyState.Shoot;
        if (distanceToPlayer <= detectionRadius) return EnemyState.Chase;
        return EnemyState.Patrol;
    }

    void UpdateState(float distanceToPlayer)
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                animator.SetBool("Chase", false);
                Patrol();
                break;
            case EnemyState.Chase:
                animator.SetBool("Chase", true);
                Chase();
                break;
            case EnemyState.Shoot:
                animator.SetBool("Chase", false);
                HandleShooting();
                break;
            case EnemyState.Explode:
                Explode();
                break;
        }
    }

    void Patrol()
    {
        if (agent == null || !agent.isOnNavMesh) return; // Pastikan agent ada dan berada di NavMesh

        if (patrolPoints.Length == 0) return;
        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 1f)
            GoToNextPatrolPoint();
    }

    void Chase()
    {
        agent.speed = chaseSpeed;
        agent.destination = player.position;
    }

    void HandleShooting()
    {
        RotateTowards(player.position);

        if (burstShotsFired < burstCount)
        {
            animator.SetBool("Shoot", true);
            if (Time.time >= nextBurstShotTime)
            {
                Shoot();
                burstShotsFired++;
                nextBurstShotTime = Time.time + 0.5f; // Delay per shot
            }
        }
        else
        {
            animator.SetBool("Shoot", false);
            burstShotsFired = 0;
            lastAttackTime = Time.time;
            currentState = EnemyState.Chase;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = (player.position - bulletSpawnPoint.position).normalized * 20f;
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        animator.SetTrigger("Dead");
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, 1f); // Destroy after explosion
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void GoToNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.destination = patrolPoints[currentPatrolIndex].position;
    }
}
