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
    public int health = 100; // Kesehatan awal
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip engineSoundClip; // AudioClip untuk suara mesin
    private Animator animator;
    private bool isDead = false;
    public bool IsDead => isDead;
    private GameObject engineSoundObject;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Pastikan suara mesin dimainkan jika HP lebih dari 0
        if (engineSoundClip != null && health > 0)
        {
            // Buat GameObject baru untuk suara mesin agar bisa dihentikan nanti
            engineSoundObject = new GameObject("EngineSound");
            engineSoundObject.transform.position = transform.position;

            // Tambahkan AudioSource ke engineSoundObject untuk memainkan AudioClip
            var audioSource = engineSoundObject.AddComponent<AudioSource>();
            audioSource.clip = engineSoundClip;
            audioSource.loop = true; // Looping suara mesin
            audioSource.spatialBlend = 1.0f; // Atur agar suara 3D
            audioSource.Play();
        }
    }

    // Fungsi untuk mengambil damage
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

    // Fungsi untuk mengatur ulang health saat respawn
    public void ResetHealth(int newHealth)
    {
        health = newHealth;  // Reset kesehatan ke newHealth (misalnya 100)
        isDead = false;
        Debug.Log("Enemy health has been reset.");
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Enemy has died.");

        // Hentikan suara mesin jika musuh mati
        if (engineSoundObject != null)
        {
            Destroy(engineSoundObject); // Hancurkan GameObject suara mesin untuk menghentikannya
        }

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
            animator.SetBool("Shoot", true); // Mulai animasi shoot
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
