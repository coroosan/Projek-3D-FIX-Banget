using UnityEngine;

public class BulletControlEnemy : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 direction, float bulletSpeed = 10f, float bulletDamage = 10f)
    {
        speed = bulletSpeed;
        damage = bulletDamage;

        // Mengatur kecepatan peluru berdasarkan arah
        rb.velocity = direction * speed;

        // Hancurkan peluru setelah 5 detik untuk mencegah kebocoran memori
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah peluru mengenai pemain atau objek lainnya
        if (other.CompareTag("Player"))
        {
            // Berikan damage ke player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Mengurangi kesehatan pemain
            }

            Destroy(gameObject); // Hancurkan peluru setelah mengenai player
        }
    }
}
