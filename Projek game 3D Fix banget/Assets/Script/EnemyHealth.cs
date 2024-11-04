using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Fungsi untuk menerima damage
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Fungsi untuk mengelola kematian musuh
    void Die()
    {
        // Logika untuk menghancurkan atau menonaktifkan musuh
        Destroy(gameObject);
    }
}
