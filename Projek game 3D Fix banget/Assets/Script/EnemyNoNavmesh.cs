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
        // Update posisi terbang musuh pada ketinggian tetap
        Vector3 flyingPosition = new Vector3(transform.position.x, flyingHeight, transform.position.z);
        transform.position = flyingPosition;

        // Ganti status sesuai keadaan
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Explode:
                Explode();
                break;
        }

        // Cek jarak ke pemain untuk pindah ke Chase atau Explode
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= detectionRadius || isTriggeredByShot)
        {
            currentState = EnemyState.Chase;
        }
        
        // Pindah ke explode jika dalam jarak ledakan
        if (distanceToPlayer <= explodeDistance)
        {
            currentState = EnemyState.Explode;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (agent.remainingDistance < 0.5f) // Jika sudah dekat dengan patrol point
        {
            GoToNextPatrolPoint();
        }

        agent.speed = patrolSpeed;
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Chase()
    {
        agent.SetDestination(player.position);
        agent.speed = chaseSpeed;
    }

    void Explode()
    {
        // Logika untuk efek ledakan
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Hancurkan musuh setelah meledak
        Destroy(gameObject);
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
