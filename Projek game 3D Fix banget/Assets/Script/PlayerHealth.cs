using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f; // Kesehatan awal pemain
    public Animation cameraAnimation; // Referensi ke komponen Animation kamera
    public Image healthImpact; // Efek gambar kesehatan
    public Image deathUI; // UI kematian
    private SpawnRestartPlayer spawnManager; // Referensi ke skrip SpawnRestartPlayer
    private bool isDead = false; // Flag untuk memastikan animasi Dead hanya dipanggil sekali

    void Start()
    {
        playerHealth = 100f;
        healthImpact.color = Color.red;

        // Cari skrip SpawnRestartPlayer di scene
        spawnManager = FindObjectOfType<SpawnRestartPlayer>();

        // Menyembunyikan UI death pada awal permainan
        if (deathUI != null)
        {
            deathUI.enabled = false; // Pastikan gambar UI kematian dimulai dalam keadaan tidak aktif
        }
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
        // Mengubah transparansi gambar healthImpact sesuai dengan health pemain
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
        if (isDead) return;
        isDead = true;
        Debug.Log("Player Died!");

        if (cameraAnimation != null)
        {
            // Tambahkan animasi jika belum ada
            if (cameraAnimation.GetClip("Cameradeath") == null)
            {
                cameraAnimation.AddClip(cameraAnimation.clip, "Cameradeath");
            }

            cameraAnimation.Play("Cameradeath");
        }

        if (deathUI != null)
        {
            deathUI.enabled = true;
            Debug.Log("Death UI enabled");
        }

        if (spawnManager != null)
        {
            StartCoroutine(WaitForDeadAnimation());
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
        yield return new WaitForSeconds(1.5f); // Ganti durasi animasi "Death" Anda

        // Setelah animasi selesai, respawn dan reset kesehatan
        spawnManager.RespawnPlayer();
        ResetPlayerHealth();
        HealthDamageImpact();

        if (deathUI != null)
        {
            deathUI.enabled = false; // Menyembunyikan gambar UI kematian
        }

        isDead = false; // Reset flag agar pemain bisa mati lagi nanti
    }
}
