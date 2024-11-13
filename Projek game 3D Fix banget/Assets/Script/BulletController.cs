using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 20f; // Kecepatan peluru, di-set oleh SpawnBullet
    public float lifespan = 5f; // Waktu hidup peluru
    private int damage = 10; // Damage default, akan di-set oleh SpawnBullet

    public GameObject hitEffect; // Efek saat peluru mengenai target

    void Start()
    {
        // Hancurkan peluru setelah waktu tertentu
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        // Gerakkan peluru ke depan
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Metode untuk mengatur damage peluru dari SpawnBullet
    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            // Jika yang terkena peluru adalah musuh
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Jika yang terkena peluru adalah boss
            BossHealth bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

}