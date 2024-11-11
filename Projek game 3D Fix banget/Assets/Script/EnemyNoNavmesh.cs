using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWithNavMesh : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Explode }
    public EnemyState currentState;

    public Transform[] patrolPoints;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5f;
    public float detectionRadius = 10f;
    public float explodeDistance = 3f; // Jarak untuk meledak
    public float flyingHeight = 10f; // Tinggi terbang musuh

    private Transform player;
    private int currentPatrolIndex;
    private bool isTriggeredByShot;
    private NavMeshAgent agent;

    [SerializeField]
    private GameObject explosionPrefab; // Prefab untuk efek ledakan
    private bool hasExploded = false; // Menandakan apakah sudah meledak
    private EnemyHealth enemyHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrol;
        currentPatrolIndex = 0;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            return; // Jangan lakukan apa pun jika musuh sudah mati
        }

        // Pastikan posisi musuh tetap di ketinggian terbang
        Vector3 flyingPosition = new Vector3(transform.position.x, flyingHeight, transform.position.z);
        transform.position = flyingPosition;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Logika eksplosi ketika mendekati pemain tanpa animasi tambahan
        if (distanceToPlayer <= explodeDistance && !hasExploded)
        {
            Explode();
            return;
        }

        // Periksa status saat ini untuk menentukan aksi
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
        }

        // Beralih ke "Chase" jika pemain berada dalam jarak deteksi atau ditembak
        if (distanceToPlayer <= detectionRadius || isTriggeredByShot)
        {
            currentState = EnemyState.Chase;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }

        agent.speed = patrolSpeed;
        Vector3 targetPosition = patrolPoints[currentPatrolIndex].position;
        targetPosition.y = flyingHeight; // Atur ketinggian ke flyingHeight
        agent.SetDestination(targetPosition);
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        Vector3 targetPosition = patrolPoints[currentPatrolIndex].position;
        targetPosition.y = flyingHeight; // Atur ketinggian ke flyingHeight
        agent.SetDestination(targetPosition);
    }

    void Chase()
    {
        Vector3 targetPosition = player.position;
        targetPosition.y = flyingHeight; // Atur ketinggian ke flyingHeight
        agent.SetDestination(targetPosition);
        agent.speed = chaseSpeed;
    }

    void Explode()
    {
        if (explosionPrefab != null && !hasExploded)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // Hancurkan objek ledakan setelah 2 detik
        }

        hasExploded = true; // Tandai bahwa ledakan sudah terjadi
        Destroy(gameObject); // Hancurkan musuh setelah meledak
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
        Gizmos.DrawWireSphere(transform.position, explodeDistance);
    }
}
