using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Boss Health Settings")]
    public int maxHealth = 500; // Total health boss
    private int currentHealth;

    [Header("Health Bar")]
    public Slider healthBar; // Drag UI Slider di inspector ke sini

    [Header("Explosion Effect")]
    public GameObject explosionPrefab; // Prefab untuk efek ledakan

    private Animator animator;

    public bool IsDead => currentHealth <= 0;

    void Start()
    {
        currentHealth = maxHealth; // Set health ke nilai maksimum saat game mulai
        animator = GetComponent<Animator>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    // Fungsi untuk menerima damage
    public void TakeDamage(int damage)
    {
        if (IsDead) return; // Jika boss sudah mati, tidak menerima damage lagi

        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    void Die()
    {
        // Memulai animasi mati
        animator.SetTrigger("Dead");

        // Menjalankan efek ledakan
        if (explosionPrefab != null)
        {
            InstantiateExplosion();
        }

        // Nonaktifkan collider agar tidak menerima damage lagi
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Menunggu sebentar untuk menampilkan animasi sebelum boss menghilang
        StartCoroutine(WaitForExplosion());
    }

    private void InstantiateExplosion()
    {
        // Instantiate efek ledakan di posisi boss
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        // Hancurkan efek ledakan setelah durasinya selesai
        Destroy(explosion, 1f); // Sesuaikan durasi sesuai kebutuhan
    }

    private IEnumerator WaitForExplosion()
    {
        // Tunggu durasi animasi kematian sebelum menghilangkan boss
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false); // Nonaktifkan boss
    }
}
