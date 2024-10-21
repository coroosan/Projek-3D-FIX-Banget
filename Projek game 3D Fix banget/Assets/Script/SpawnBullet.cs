using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab peluru
    public Transform spawnPoint; // Titik spawn peluru
    public float bulletSpeed = 20f;
    public int bulletDamage = 10; // Tambahkan ini untuk mengatur damage peluru

    public void FireBullet()
    {
        // Buat peluru di titik spawn
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

        // Atur kecepatan peluru
        bullet.GetComponent<BulletController>().speed = bulletSpeed;

        // Atur damage peluru
        bullet.GetComponent<BulletController>().SetDamage(bulletDamage);
    }
}
