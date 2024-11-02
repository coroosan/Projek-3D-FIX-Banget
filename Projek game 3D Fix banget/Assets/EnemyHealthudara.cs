using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthUdara : MonoBehaviour
{
    // Variabel kesehatan musuh
    public int health = 100;
    [SerializeField] private GameObject explosionPrefab;
    private Animator animator;

    // Menandakan apakah musuh sudah mati
    private bool isDead = false;

    void Start()
    {
        // Mengambil komponen Animator dari objek ini
        animator = GetComponent<Animator>();
    }

    // Fungsi untuk mengurangi kesehatan musuh
    public void TakeDamage(int damageAmount)
    {
        // Jika musuh sudah mati, tidak melakukan apa-apa
        if (isDead) return;

        // Mengurangi kesehatan musuh
        health -= damageAmount;

        // Jika kesehatan <= 0, panggil fungsi mati
        if (health <= 0)
        {
            Die();
        }
    }

    // Fungsi untuk menangani kematian musuh
    public void Die()
    {
        // Jika sudah mati, tidak melakukan apa-apa
        if (isDead) return;

        isDead = true; // Tandai musuh sudah mati

        Debug.Log("Enemy has died.");

        // Memainkan animasi mati
        if (animator != null)
        {
            animator.SetBool("dead", true); // Set animasi mati
        }

        // Memicu efek ledakan tanpa delay
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Menghancurkan objek ini setelah 1 detik
        Destroy(gameObject, 1f);
    }
}
