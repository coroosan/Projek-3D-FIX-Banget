using UnityEngine;
using System.Collections;

public enum EnemyType
{
    TypeA,
    TypeB
}

public class EnemyHealth : MonoBehaviour
{
    public EnemyType enemyType;
    public int health = 100;
    [SerializeField] private GameObject explosionPrefab;
    private Animator animator;
    private bool isDead = false;
    public bool IsDead => isDead;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;
        Debug.Log($"Enemy took damage: {damageAmount}. Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Enemy has died.");

        if (animator != null)
        {
            animator.SetTrigger("Dead"); // Animasi mati
        }

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Nonaktifkan collider dan NavMeshAgent
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        StartCoroutine(HideAfterDelay(1.5f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        EnemyWithNavMesh controller = GetComponent<EnemyWithNavMesh>();
        if (controller != null)
        {
            controller.enabled = false;
        }
    }

    public void StartChase()
    {
        if (enemyType == EnemyType.TypeB && !isDead)
        {
            animator.SetBool("Chase", true); // Mengaktifkan animasi chase
        }
    }

    public void StopChase()
    {
        if (enemyType == EnemyType.TypeB)
        {
            animator.SetBool("Chase", false);
        }
    }

    public void StartShoot()
    {
        if (enemyType == EnemyType.TypeB && !isDead)
        {
            animator.SetBool("Shoot", true); // Mulai animasi shoots
        }
    }

    public void StopShoot()
    {
        if (enemyType == EnemyType.TypeB)
        {
            animator.SetBool("Shoot", false); // Hentikan animasi shoot
        }
    }
}