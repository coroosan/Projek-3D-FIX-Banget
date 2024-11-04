using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageExplode : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;
    public LayerMask damageableLayer; // Hanya objek pada layer ini yang menerima damage

    private void Start()
    {
        // Panggil metode untuk memberikan damage segera setelah ledakan diaktifkan
        DealDamage();
    }

    void DealDamage()
    {
        // Dapatkan semua objek di sekitar dalam radius ledakan
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, damageableLayer);

        foreach (Collider hit in hitColliders)
        {
            // Cek apakah objek memiliki komponen PlayerHealth
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage); // Berikan damage ke player
            }

            // Jika ada objek lain yang perlu menerima damage, tambahkan logika serupa di sini
        }

        // Hancurkan objek ledakan setelah damage diberikan (opsional)
        Destroy(gameObject, 2f); // Menghancurkan partikel setelah 2 detik
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}