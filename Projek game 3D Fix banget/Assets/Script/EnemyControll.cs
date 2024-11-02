using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControll : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Shoot, Explode }
    public EnemyState currentState;

    public Transform[] patrolPoints;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5f;
    public float detectionRadius = 20f;
    public float shootingDistance = 10f;
    public float explosionDistance = 2f;
    public float attackCooldown = 3f;
    public float burstInterval = 0.2f;
    public int burstCount = 3;
    public float flyingHeight = 10f;
    public float rotationSpeed = 5f;

    public EnemyHealth enemyHealth; // Tambahkan referensi ke EnemyHealth
    private Transform player;
    private int currentPatrolIndex;
    private float lastAttackTime;
    private bool isTriggeredByShot;
    public NavMeshAgent agent;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 20f;

    [SerializeField] private GameObject explosionPrefab; // Tambahkan referensi ke prefab ledakan

    private int burstShotsFired;
    private float nextBurstShotTime;
    private bool hasExploded = false; // Flag untuk mengecek apakah musuh sudah meledak

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>(); // Ambil komponen EnemyHealth
        currentState = EnemyState.Patrol;
        currentPatrolIndex = 0;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        // Cek apakah musuh sudah mati, jika ya, hentikan semua aktivitas.
        if (enemyHealth.health <= 0)
        {
            return; // Jika musuh sudah mati, tidak perlu melanjutkan Update.
        }

        Vector3 flyingPosition = new Vector3(transform.position.x, flyingHeight, transform.position.z);
        transform.position = flyingPosition;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Logika transisi state
        if (distanceToPlayer <= explosionDistance && !hasExploded) // Cek apakah belum meledak
        {
            currentState = EnemyState.Explode;
        }
        else if (distanceToPlayer <= detectionRadius || isTriggeredByShot)
        {
            currentState = EnemyState.Chase;
        }
        else if (distanceToPlayer <= shootingDistance && Time.time > lastAttackTime + attackCooldown)
        {
            burstShotsFired = 0;
            currentState = EnemyState.Shoot;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }

        // Eksekusi logika state yang sesuai
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Shoot:
                HandleShooting();
                break;
            case EnemyState.Explode:
                Explode();
                break;
        }
    }


    void Shoot()
    {
        RotateTowards(player.position);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        BulletControlEnemy bulletController = bullet.GetComponent<BulletControlEnemy>();

        Vector3 shootingDirection = (player.position - bulletSpawnPoint.position).normalized;
        bulletController.Initialize(shootingDirection, bulletSpeed, bulletDamage: 15f);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        MoveToTarget(targetPoint.position, patrolSpeed);

        if (Vector3.Distance(transform.position, targetPoint.position) < 1f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void Chase()
    {
        MoveToTarget(player.position, chaseSpeed);
    }

    void HandleShooting()
    {
        RotateTowards(player.position);

        if (burstShotsFired < burstCount)
        {
            if (Time.time >= nextBurstShotTime)
            {
                Shoot();
                burstShotsFired++;
                nextBurstShotTime = Time.time + burstInterval;
            }
        }
        else
        {
            lastAttackTime = Time.time;
            currentState = EnemyState.Chase;
            burstShotsFired = 0;
        }
    }

    void Explode()
    {
        if (!hasExploded)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            enemyHealth.Die(); // Tidak memanggil ledakan di Die()
            hasExploded = true;
        }
    }


    void MoveToTarget(Vector3 targetPosition, float speed)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        RotateTowards(targetPosition);
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void TakeDamage(int damageAmount)
    {
        enemyHealth.TakeDamage(damageAmount); // Panggil fungsi TakeDamage dari EnemyHealth
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionDistance);
    }
}
