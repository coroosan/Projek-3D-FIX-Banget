using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int maxHealth = 100; // Health maksimum musuh
    public int explosionDamage = 50; // Damage yang diberikan pada pemain saat meledak

    private int health;
    private Transform player;
    private int currentPatrolIndex;
    private float lastAttackTime;
    private bool isTriggeredByShot;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private float bulletSpeed = 20f;

    private int burstShotsFired;
    private float nextBurstShotTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = EnemyState.Patrol;
        currentPatrolIndex = 0;
        health = maxHealth; // Set health awal musuh
        GoToNextPatrolPoint();
    }

    void Update()
    {
        Vector3 flyingPosition = new Vector3(transform.position.x, flyingHeight, transform.position.z);
        transform.position = flyingPosition;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Logika transisi state
        if (health <= 20 && distanceToPlayer <= explosionDistance)
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
        // Tampilkan efek ledakan
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        // Periksa jarak ke pemain dan beri damage jika dalam radius ledakan
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= explosionDistance)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage);
            }
        }

        // Hancurkan musuh
        Destroy(gameObject);
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
        health -= damageAmount;

        if (health <= 20)
        {
            currentState = EnemyState.Explode;
        }
        else
        {
            isTriggeredByShot = true;
            currentState = EnemyState.Chase;
        }
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
