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
    [SerializeField] private Transform bulletSpawnPoint1;
    [SerializeField] private Transform bulletSpawnPoint2;
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

        // Cek apakah NavMeshAgent valid
        if (agent != null && agent.isOnNavMesh)
        {
            GoToNextPatrolPoint();
        }
    }

    void Update()
    {
        if (hasExploded || player == null || agent == null || animator == null) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Cek apakah agent berada pada NavMesh sebelum melanjutkan
        if (agent.isOnNavMesh)
        {
            currentState = DetermineState(distanceToPlayer);
            UpdateState(distanceToPlayer);
        }
    }

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
        // Cek apakah agent valid dan berada di NavMesh
        if (agent == null || !agent.isOnNavMesh) return;

        if (patrolPoints.Length == 0) return;

        agent.speed = patrolSpeed;

        // Jika sudah dekat dengan titik patrol, pergi ke titik berikutnya
        if (!agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0 || agent == null || !agent.isOnNavMesh) return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Chase()
    {
        if (agent == null || !agent.isOnNavMesh) return;
        agent.speed = chaseSpeed;
        agent.destination = player.position;
    }

    void HandleShooting()
    {
        if (hasExploded || (enemyHealth != null && enemyHealth.IsDead))
        {
            animator.SetBool("Shoot", false);
            return;
        }

        RotateTowards(player.position);

        if (burstShotsFired < burstCount)
        {
            animator.SetBool("Shoot", true);
            if (Time.time >= nextBurstShotTime)
            {
                Shoot();
                burstShotsFired++;
                nextBurstShotTime = Time.time + 0.5f;
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
        if (hasExploded || (enemyHealth != null && enemyHealth.IsDead)) return;

        ShootFromPoint(bulletSpawnPoint1);
        ShootFromPoint(bulletSpawnPoint2);
    }

    void ShootFromPoint(Transform spawnPoint)
    {
        if (hasExploded || (enemyHealth != null && enemyHealth.IsDead)) return;

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = (player.position - spawnPoint.position).normalized * 20f;
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        animator.SetTrigger("Dead");
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, 1f);
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
