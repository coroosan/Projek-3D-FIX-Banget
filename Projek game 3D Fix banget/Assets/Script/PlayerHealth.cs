using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f; // Kesehatan awal pemain
    public Image healthImpact; // Efek gambar kesehatan
    public GameObject deathUI; // UI kematian
    private SpawnRestartPlayer spawnManager; // Referensi ke skrip SpawnRestartPlayer
    private bool isDead = false; // Flag untuk memastikan pemain mati hanya sekali

    void Start()
    {
        playerHealth = 100f;

        // Cari skrip SpawnRestartPlayer di scene
        spawnManager = FindObjectOfType<SpawnRestartPlayer>();

        // Pastikan UI Death dan Health Impact dalam keadaan awal
        if (deathUI != null)
        {
            deathUI.SetActive(false); // UI Death dimulai dalam keadaan tidak aktif
        }

        if (healthImpact != null)
        {
            healthImpact.color = new Color(1, 0, 0, 0); // Transparansi penuh
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

    private void UpdateHealthImpact()
    {
        // Mengubah transparansi healthImpact berdasarkan kesehatan pemain
        if (healthImpact != null)
        {
            float transparency = 1f - (playerHealth / 100f);
            healthImpact.color = new Color(1, 0, 0, transparency);
        }
    }

    public void TakeDamage(float damage)
    {
        if (playerHealth > 0 && !isDead)
        {
            playerHealth -= damage;
            playerHealth = Mathf.Max(playerHealth, 0); // Pastikan kesehatan tidak di bawah 0
            Debug.Log("Player Health: " + playerHealth);

            UpdateHealthImpact();

            if (playerHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Player Died!");

        if (deathUI != null)
        {
            deathUI.SetActive(true); // Tampilkan UI Death
            Time.timeScale = 0f; // Pause game

            // Aktifkan kursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OnRetry()
    {
        if (spawnManager != null)
        {
            spawnManager.RespawnPlayer(); // Respawn pemain
        }

        playerHealth = 100f; // Reset kesehatan pemain
        UpdateHealthImpact(); // Reset efek health impact

        if (deathUI != null)
        {
            deathUI.SetActive(false); // Sembunyikan UI Death
        }

        Time.timeScale = 1f; // Resume game

        // Kunci kembali kursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isDead = false; // Reset flag mati
    }

}
