using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f; // Kesehatan pemain awal
    public Image healthImpact; // Gambar untuk menampilkan efek kesehatan

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 100;
        healthImpact.color = Color.red;
    }

    // Deteksi saat pemain bertabrakan dengan objek lain
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) // Jika terkena peluru
        {
            TakeDamage(10f); // Contoh damage dari peluru
            Destroy(other.gameObject); // Hapus peluru setelah mengenai pemain
        }

        if (other.CompareTag("Explode")) // Jika terkena ledakan
        {
            TakeDamage(25f); // Contoh damage dari ledakan
        }
    }

    void HealthDamageImpact()
    {
        float transparency = 1f - (playerHealth / 100f);
        Color imageColor = Color.white;
        imageColor.a = transparency;
        healthImpact.color = imageColor;

        Debug.Log("Player Health: " + playerHealth + ", Transparency: " + transparency);
    }

    // Mengurangi kesehatan pemain - ubah aksesibilitas menjadi public
    public void TakeDamage(float damage) // Ubah dari private menjadi public
    {
        if (playerHealth > 0)
        {
            playerHealth -= damage;
            playerHealth = Mathf.Max(playerHealth, 0); // Pastikan kesehatan tidak di bawah 0
            Debug.Log("Player is taking damage, current health: " + playerHealth);
        }
    }

    // Update is called once per frame 
    void Update()
    {
        HealthDamageImpact(); // Perbarui efek gambar kesehatan
    }
}
