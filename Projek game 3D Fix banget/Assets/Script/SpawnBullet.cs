using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bulletPrefab;   // Prefab peluru
    public Transform spawnPoint;      // Titik spawn peluru
    public float bulletSpeed = 20f;   // Kecepatan peluru
    public float fireCooldown = 0.5f; // Cooldown antar tembakan
    public int bulletDamage = 10;     // Damage peluru
    public EnergyWeaponWithSlider energyWeapon; // Referensi ke sistem energi
    public AudioClip shootSound; // Suara tembakan
    private AudioSource audioSource; // AudioSource untuk memainkan suara
    private float lastFireTime = 0f; // Waktu terakhir menembak

    private bool isPaused = false; // Untuk mengecek apakah game sedang pause

    void Update()
    {
        // Cek status pause dan nonaktifkan shooting saat game dipause
        if (Time.timeScale == 0f)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }

        if (!isPaused)
        {
            HandleShooting();  // Handle shooting hanya jika tidak dalam keadaan pause
        }
    }

    void Awake()
    {
        // Mendapatkan referensi AudioSource pada GameObject yang sama
        audioSource = GetComponent<AudioSource>();
    }

    void HandleShooting()
    {
        // Input menembak dan cek apakah cooldown sudah selesai
        if (Input.GetButton("Fire1") && Time.time > lastFireTime + fireCooldown)
        {
            if (energyWeapon != null && energyWeapon.canShoot)
            {
                FireBullet(); // Panggil fungsi tembak
                lastFireTime = Time.time; // Atur waktu tembakan terakhir
            }
        }
    }

    void FireBullet()
    {
        if (energyWeapon.currentEnergy >= energyWeapon.energyUsagePerShot)
        {
            // Buat peluru di titik spawn
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

            // Tambahkan kecepatan ke peluru
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false; // Nonaktifkan gravitasi pada peluru
                rb.velocity = spawnPoint.forward * bulletSpeed;
            }

            // Set damage peluru jika ada skrip BulletController
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (bulletController != null)
            {
                bulletController.SetDamage(bulletDamage);
            }

            // Kurangi energi setelah menembak
            energyWeapon.Shoot();
            PlayShootSound();
        }
    }

    void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound); // Mainkan suara tembakan
        }
        else
        {
            Debug.LogWarning("AudioSource atau AudioClip belum diatur"); // Pesan peringatan jika AudioSource atau AudioClip belum diatur
        }
    }

    // Fungsi untuk Pause
    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause game
        // Tidak perlu menonaktifkan shooting di sini karena sudah dikelola di Update
    }

    // Fungsi untuk Resume
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume game
    }
}
