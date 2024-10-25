using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControll : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }
    public EnemyState currentState;

    public Transform[] patrolPoints;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5f;
    public float attackDistance = 4f;
    public float detectionRadius = 10f;
    public float shootReactionRadius = 15f;
    public float attackCooldown = 2f;
    public float flyingHeight = 10f; // Tinggi terbang musuh

    private NavMeshAgent agent;
    private Transform player;
    private int currentPatrolIndex;
    private float lastAttackTime;
    private bool isTriggeredByShot;

    // Tambahkan reference untuk bullet
    [SerializeField]
    private GameObject bulletPrefab;  // Prefab peluru
    [SerializeField]
    private Transform bulletSpawnPoint;  // Tempat spawn peluru

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = EnemyState.Patrol;
        agent.speed = patrolSpeed;
        currentPatrolIndex = 0;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        // Perbarui posisi terbang musuh dengan ketinggian tetap
        Vector3 flyingPosition = new Vector3(transform.position.x, flyingHeight, transform.position.z);
        transform.position = flyingPosition;

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }

        // Trigger jika player menembak atau dalam radius tertentu
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= detectionRadius || isTriggeredByShot)
        {
            currentState = EnemyState.Chase;
            agent.speed = chaseSpeed;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 3f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void Chase()
    {
        agent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= attackDistance && Time.time > lastAttackTime + attackCooldown)
        {
            currentState = EnemyState.Attack;
        }
    }

    void Attack()
    {
        agent.isStopped = true; // Berhenti saat menyerang
        transform.LookAt(player);

        // Memanggil fungsi untuk menembakkan peluru
        Shoot();

        lastAttackTime = Time.time;
        currentState = EnemyState.Chase; // Kembali ke keadaan mengejar setelah menyerang
        agent.isStopped = false;
    }

    void Shoot()
    {
        // Spawn bullet dari spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        BulletControlEnemy bulletController = bullet.GetComponent<BulletControlEnemy>();

        // Arahkan peluru ke arah pemain
        Vector3 shootingDirection = (player.position - bulletSpawnPoint.position).normalized;
        bulletController.Initialize(shootingDirection);
    }

    public void OnShot()
    {
        isTriggeredByShot = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootReactionRadius);
    }
}
