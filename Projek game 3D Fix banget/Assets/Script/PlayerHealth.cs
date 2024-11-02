using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f; // Kesehatan pemain
    private Animator animator; // Referensi ke komponen Animator

    private void Start()
    {
        // Dapatkan komponen Animator di objek pemain
        animator = GetComponent<Animator>();
    }

    // Fungsi untuk menerima damage
    public void TakeDamage(float amount)
    {
        // Jika animasi "Die" sedang aktif, abaikan damage lebih lanjut
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            Debug.Log("Player is already dead, ignoring damage.");
            return;
        }

        // Kurangi kesehatan pemain
        health -= amount;
        Debug.Log("Damage taken: " + amount + ", Remaining health: " + health);

        // Jika kesehatan mencapai 0 atau kurang, panggil Die
        if (health <= 0)
        {
            health = 0; // Setel kesehatan ke 0 agar tidak negatif
            Die();
        }
    }

    // Fungsi untuk menangani logika kematian
    private void Die()
    {
        Debug.Log("Player is dead!");

        // Aktifkan animasi kematian menggunakan Trigger
        if (animator != null)
        {
            animator.SetTrigger("Die");
            Debug.Log("Trigger 'Die' set in Animator.");
        }
        else
        {
            Debug.LogWarning("Animator is not assigned!");
        }

        // Tambahkan logika lain, seperti menonaktifkan kontrol pemain
    }
}
