using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bulletPrefab;   // Prefab peluru
    public Transform spawnPoint;      // Titik spawn peluru
    public float bulletSpeed = 20f;   // Kecepatan peluru
    public int bulletDamage = 10;     // Damage peluru
    public EnergyWeaponWithSlider energyWeapon;   // Referensi ke EnergyWeaponWithSlider
    private float fireCooldown = 0.5f;  // Waktu cooldown antara tembakan
    private float lastFireTime = 0f;    // Waktu tembakan terakhir

    void Update()
    {
        // Pastikan hanya menembak jika cooldown sudah habis dan energi cukup
        if (Input.GetButtonDown("Fire1") && Time.time > lastFireTime + fireCooldown)
        {
            // Periksa apakah ada energi untuk menembak
            if (energyWeapon != null && energyWeapon.canShoot)
            {
                FireBullet();  // Panggil fungsi untuk menembak
                lastFireTime = Time.time;  // Set waktu tembakan terakhir
            }
        }
    }

    public void FireBullet()
    {
        // Cek apakah energi cukup untuk menembak
        if (energyWeapon.currentEnergy >= energyWeapon.energyUsagePerShot)
        {
            // Buat peluru di titik spawn
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

            // Dapatkan Rigidbody dari peluru
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            // Nonaktifkan gravitasi pada peluru
            rb.useGravity = false;

            // Tambahkan gaya untuk menggerakkan peluru ke depan
            rb.AddForce(spawnPoint.forward * bulletSpeed, ForceMode.Impulse);

            // Atur damage peluru
            bullet.GetComponent<BulletController>().SetDamage(bulletDamage);

            // Kurangi energi setelah tembakan
            energyWeapon.Shoot();  // Hanya panggil sekali setelah tembakan berhasil
        }
        else
        {
            Debug.Log("Energi tidak cukup untuk menembak.");
        }
    }
}
