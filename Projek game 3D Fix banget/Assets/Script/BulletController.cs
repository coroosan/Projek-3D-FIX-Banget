using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 20f;
    public float lifespan = 5f;
    private int damage = 10;

    public GameObject hitEffect; // Prefab efek saat peluru mengenai target

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

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Periksa jika peluru mengenai target dengan komponen EnemyHealth
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            // Aplikasikan damage ke musuh
            enemyHealth.TakeDamage(damage);

            // Buat efek saat peluru mengenai target
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // Hancurkan peluru setelah terkena target
            Destroy(gameObject);
        }
    }
}
