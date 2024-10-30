using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerakanMeriam : MonoBehaviour
{
    public Transform player; // Referensi ke objek player
    public float rotationSpeed = 5f; // Kecepatan rotasi meriam
    public GameObject projectilePrefab; // Prefab peluru
    public Transform firePoint; // Titik di mana peluru keluar
    public float shootingInterval = 2f; // Interval waktu antar tembakan
    private float shootingTimer; // Timer internal untuk menghitung interval tembakan

    void Update()
    {
        if (player != null)
        {
            // Panggil fungsi untuk mengarahkan meriam ke player
            RotateTowardsPlayer();

            // Hitung waktu untuk menembak
            shootingTimer += Time.deltaTime;
            if (shootingTimer >= shootingInterval)
            {
                Shoot();
                shootingTimer = 0f; // Reset timer setelah menembak
            }
        }
    }

    void RotateTowardsPlayer()
    {
        // Hitung arah ke player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Membatasi rotasi hanya pada sumbu horizontal

        // Hitung rotasi yang dibutuhkan untuk mengarah ke player
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotasi meriam secara bertahap ke arah targetRotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            // Munculkan peluru di firePoint dengan rotasi meriam saat ini
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Debug.Log("Meriam menembak!");
        }
    }
}