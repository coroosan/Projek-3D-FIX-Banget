using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f; // Kesehatan awal pemain
    public Image healthImpact; // Efek gambar kesehatan
    public GameObject deathUI; // UI kematian
    private SpawnRestartPlayer spawnManager; // Referensi ke skrip SpawnRestartPlayer
    private bool isDead = false; // Flag untuk memastikan pemain mati hanya sekali
    private Animator childAnimator; // Animator pada child
    private FPSPlayerController fpsPlayerController;
    public GameObject player; 
                              // Referensi ke skrip FPSPlayerController
                              // Tidak perlu MouseLook lagi karena sudah diatur di FPSPlayerController

    void Start()
    {
        playerHealth = 100f;

        // Cari skrip SpawnRestartPlayer di scene
        spawnManager = FindObjectOfType<SpawnRestartPlayer>();

        // Mendapatkan referensi Animator pada child
        childAnimator = GetComponentInChildren<Animator>();
        if (childAnimator != null)
        {
            childAnimator.enabled = false; // Nonaktifkan animator di awal
        }

        // Mendapatkan referensi ke FPSPlayerController
        fpsPlayerController = GetComponent<FPSPlayerController>();

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

        // Matikan movement
        if (fpsPlayerController != null) fpsPlayerController.enabled = false;

        // Mengaktifkan animator pada child object
        if (childAnimator != null)
        {
            childAnimator.enabled = true;
            childAnimator.Play("CameraDeath"); // Ganti "Death" dengan nama animasi yang ingin diputar
        }

        // Mulai coroutine untuk menampilkan UI death setelah animasi selesai
        StartCoroutine(ShowDeathUIWithDelay());
    }

    private IEnumerator ShowDeathUIWithDelay()
    {
        // Tunggu 2 detik (atau sesuaikan sesuai durasi animasi) tanpa terpengaruh Time.timeScale
        yield return new WaitForSecondsRealtime(2f);

        if (deathUI != null)
        {
            deathUI.SetActive(true); // Tampilkan UI Death
            Time.timeScale = 0f; // Pause game

            // Aktifkan kursor, tapi tetap tidak bisa digerakkan
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

        // Pastikan animator tidak sedang memainkan animasi kematian dan tidak memainkan animasi apapun
        if (childAnimator != null)
        {
            childAnimator.enabled = false; // Nonaktifkan animator sementara
            childAnimator.enabled = true;  // Aktifkan kembali animator
        }

        if (deathUI != null)
        {
            deathUI.SetActive(false); // Sembunyikan UI Death
        }

        Time.timeScale = 1f; // Resume game

        // Kunci kembali kursor dan hidupkan kontrol
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Hidupkan kembali movement
        if (fpsPlayerController != null) fpsPlayerController.enabled = true;

        // Reset kondisi animasi ke kondisi normal (tidak ada animasi)
        if (childAnimator != null)
        {
            // Pastikan animator dalam keadaan idle atau tidak aktif tanpa animasi.
            childAnimator.enabled = false; // Nonaktifkan animator sepenuhnya
        }

        isDead = false; // Reset flag mati

        // Reset posisi seluruh pemain termasuk objek anak (bone, senjata, dll)
        ResetPlayerPosition();
    }

    private void ResetPlayerPosition()
    {
        if (spawnManager != null)
        {
            Vector3 startPosition = spawnManager.GetPlayerStartPosition();

            // Set posisi pemain utama
            player.transform.position = startPosition;

            // Set posisi semua anak objek (seperti senjata atau bone)
            foreach (Transform child in player.transform)
            {
                child.localPosition = Vector3.zero; // Reset posisi lokal anak objek
                child.localRotation = Quaternion.identity; // Reset rotasi lokal
            }
        }
    }

}
