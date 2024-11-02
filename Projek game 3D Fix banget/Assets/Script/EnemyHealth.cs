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
            animator.SetTrigger("Dead"); // Mainkan animasi mati
        }

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Ledakan tanpa delay
        }

        // Nonaktifkan collider untuk menghentikan interaksi
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Nonaktifkan gerakan (contoh untuk NavMeshAgent)
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Nonaktifkan renderer setelah animasi selesai untuk sembunyikan objek tanpa menghancurkannya
        StartCoroutine(HideAfterDelay(1.5f)); // 1.5 detik untuk memberi waktu animasi
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        // Optional: Nonaktifkan komponen script yang mengendalikan logika musuh
        EnemyWithNavMesh controller = GetComponent<EnemyWithNavMesh>();
        if (controller != null)
        {
            controller.enabled = false;
        }
    }
}