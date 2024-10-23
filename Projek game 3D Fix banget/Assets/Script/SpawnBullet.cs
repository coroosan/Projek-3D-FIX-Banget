using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab peluru
    public Transform spawnPoint; // Titik spawn peluru
    public float bulletSpeed = 20f;
    public int bulletDamage = 10; // Tambahkan ini untuk mengatur damage peluru

    void Update()
    {
        // Cek apakah tombol Fire1 ditekan (biasanya tombol kiri mouse atau Ctrl)
        if (Input.GetButtonDown("Fire1"))
        {
            FireBullet();
        }
    }

    public void FireBullet()
    {
        // Buat peluru di titik spawn
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

        // Dapatkan Rigidbody dari peluru
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Nonaktifkan gravitasi jika tidak diinginkan
        rb.useGravity = false;

        // Tambahkan gaya untuk menggerakkan peluru ke depan
        rb.AddForce(spawnPoint.forward * bulletSpeed, ForceMode.Impulse);

        // Atur damage peluru
        bullet.GetComponent<BulletController>().SetDamage(bulletDamage);
    }
}