using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f; // Kesehatan awal pemain
    public Image healthImpact; // Efek gambar kesehatan

    private SpawnRestartPlayer spawnManager; // Referensi ke skrip SpawnRestartPlayer
    private Animator animator; // Animator untuk animasi Dead

    private bool isDead = false; // Flag untuk memastikan animasi Dead hanya dipanggil sekali

    private void Start()
    {
        playerHealth = 100f;
        healthImpact.color = Color.red;

        // Cari skrip SpawnRestartPlayer di scene
        spawnManager = FindObjectOfType<SpawnRestartPlayer>();

        // Mendapatkan komponen Animator (jika ada)
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(10f);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Explode"))
        {
            TakeDamage(25f);
        }
    }

    private void HealthDamageImpact()
    {
        float transparency = 1f - (playerHealth / 100f);
        Color imageColor = Color.white;
        imageColor.a = transparency;
        healthImpact.color = imageColor;
    }

    public void TakeDamage(float damage)
    {
        if (playerHealth > 0 && !isDead)
        {
            playerHealth -= damage;
            playerHealth = Mathf.Max(playerHealth, 0);
            Debug.Log("Player Health: " + playerHealth);

            if (playerHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        if (isDead) return; // Cegah pemanggilan ganda
        isDead = true;
        Debug.Log("Player Died!");

        // Menjalankan animasi "Dead" jika ada
        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }

        // Mulai Coroutine untuk menunggu animasi selesai sebelum respawn
        if (spawnManager != null)
        {
            StartCoroutine(WaitForDeadAnimation());
        }
    }

    private void Update()
    {
        HealthDamageImpact();
    }

    public void ResetPlayerHealth()
    {
        playerHealth = 100f;
        Debug.Log("Player Health Reset to: " + playerHealth);
    }

    // Coroutine untuk menunggu animasi selesai sebelum respawn
    private IEnumerator WaitForDeadAnimation()
    {
        // Tunggu hingga animasi selesai (pastikan nama state benar sesuai dengan Animator Anda)
        yield return new WaitForSeconds(1.5f); // Ganti 1.5f dengan durasi animasi "Dead" Anda

        // Setelah animasi selesai, respawn dan reset kesehatan
        spawnManager.RespawnPlayer();
        ResetPlayerHealth();
        HealthDamageImpact();

        isDead = false; // Reset flag agar pemain bisa mati lagi nanti
    }
}
