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
    private Animator animator; // Animator untuk mengatur animasi
    private EnemyHealth enemyHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Ambil komponen Animator
        currentState = EnemyState.Patrol;
        currentPatrolIndex = 0;
        GoToNextPatrolPoint();
    }

    void Update()
    {

        if (enemyHealth != null && enemyHealth.IsDead)
        {
            // Jika musuh sudah mati, jangan lakukan gerakan apa pun
            return;
        }

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
            case EnemyState.Explode:
                Explode();
                break;
        }

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= detectionRadius || isTriggeredByShot)
        {
            currentState = EnemyState.Chase;
        }

        if (distanceToPlayer <= explodeDistance)
        {
            currentState = EnemyState.Explode;
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
        if (!hasExploded) // Cek apakah ledakan sudah terjadi
        {
            if (animator != null)
            {
                animator.SetBool("Dead", true); // Mainkan animasi mati
            }

            StartCoroutine(HandleExplosion()); // Menunggu animasi sebelum meledak
        }
    }

    private IEnumerator HandleExplosion()
    {
        // Tunggu beberapa detik sebelum meledak (sesuaikan dengan durasi animasi)
        yield return new WaitForSeconds(1f);

        if (explosionPrefab != null)
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
