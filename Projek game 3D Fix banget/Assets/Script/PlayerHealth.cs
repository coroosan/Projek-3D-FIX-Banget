using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f; // Kesehatan pemain

    // Fungsi untuk menerima damage
    public void TakeDamage(float amount)
    {
        health -= amount; // Kurangi kesehatan dengan jumlah damage
        Debug.Log("Damage taken: " + amount + ", Remaining health: " + health);

        // Cek apakah kesehatan pemain <= 0
        if (health <= 0)
        {
            Die();
        }
    }

    // Fungsi untuk menangani logika kematian
    private void Die()
    {
        Debug.Log("Player is dead!");
        // Tambahkan logika untuk menghapus pemain atau memainkan animasi kematian
        Destroy(gameObject); // Contoh: Menghancurkan objek pemain
    }
}