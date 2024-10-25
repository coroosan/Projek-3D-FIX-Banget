using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Tambahkan untuk menggunakan NavMesh

public class EnemyWithNavMesh : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }
    public EnemyState currentState;

    public Transform[] patrolPoints;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5f;
    public float attackDistance = 4f;
    public float detectionRadius = 10f;
    public float attackCooldown = 2f;
    public float flyingHeight = 10f; // Tinggi terbang musuh

    private Transform player;
    private int currentPatrolIndex;
    private float lastAttackTime;
    private bool isTriggeredByShot;

    // Tambahkan reference untuk bullet
    [SerializeField]
    private GameObject bulletPrefab;  // Prefab peluru
    [SerializeField]
    private Transform bulletSpawnPoint;  // Tempat spawn peluru

    // Reference untuk NavMeshAgent
    private NavMeshAgent agent;

    void Start()
    {
        // Mengambil referensi player dan NavMeshAgent
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        // Mulai dari keadaan patrol
        currentState = EnemyState.Patrol;
        currentPatrolIndex = 0;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        // Update posisi terbang musuh dengan ketinggian tetap
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
        }
    }

    void Patrol()
    {
        // Jika belum ada patrol points, return
        if (patrolPoints.Length == 0) return;

        // Set tujuan patrol menggunakan NavMeshAgent
        if (agent.remainingDistance < 0.5f) // Jika sudah dekat dengan patrol point
        {
            GoToNextPatrolPoint();
        }

        // Pastikan kecepatan patrol sesuai
        agent.speed = patrolSpeed;
    }

    void GoToNextPatrolPoint()
    {
        // Jika tidak ada patrol points, return
        if (patrolPoints.Length == 0) return;

        // Pilih patrol point berikutnya
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Chase()
    {
        // Mengejar pemain menggunakan NavMeshAgent
        agent.SetDestination(player.position);

        // Tingkatkan kecepatan untuk mengejar
        agent.speed = chaseSpeed;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= attackDistance && Time.time > lastAttackTime + attackCooldown)
        {
            currentState = EnemyState.Attack;
        }
    }

    void Attack()
    {
        // Musuh menghadap ke pemain dan menembak
        agent.SetDestination(transform.position); // Berhenti saat menyerang
        transform.LookAt(player);
        Shoot();
        lastAttackTime = Time.time;

        // Kembali ke chase setelah serangan
        currentState = EnemyState.Chase;
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
    }
}
