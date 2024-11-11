using UnityEngine;

public class RocketControll : MonoBehaviour
{
    public float fallSpeed = 50f; // Kecepatan roket jatuh vertikal
    public float rotationSpeed = 200f; // Kecepatan rotasi saat jatuh
    public GameObject explosionEffect; // Prefab efek ledakan
    public int damage = 20; // Damage yang akan diberikan
    public float explosionDuration = 2f; // Durasi efek ledakan sebelum dihancurkan
    private Rigidbody rb; // Rigidbody untuk fisika

    void Start()
    {
        // Menambahkan komponen Rigidbody jika belum ada
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Atur Rigidbody agar lebih realistis
        rb.useGravity = true;
        rb.drag = 0.5f; // Sedikit tahanan udara
        rb.angularDrag = 0.3f;

        // Membuat roket jatuh lebih cepat tanpa mengubah arah
        rb.velocity = Vector3.down * fallSpeed;

        // Set rotasi roket agar menghadap ke bawah
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    void Update()
    {
        // Memberikan efek rotasi realistis saat jatuh
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Cek apakah roket menyentuh objek tertentu
        if (other.CompareTag("Plane") || other.CompareTag("Player"))
        {
            Explode();

            // Jika roket mengenai player, berikan damage
            if (other.CompareTag("Player"))
            {
                ApplyDamage(other.gameObject);
            }
        }
    }

    void Explode()
    {
        // Memunculkan efek ledakan
        if (explosionEffect != null)
        {
            // Instantiate efek ledakan dan simpan referensinya
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Hancurkan efek ledakan setelah durasi tertentu
            Destroy(explosion, explosionDuration);
        }

        // Hancurkan roket setelah meledak
        Destroy(gameObject);
    }

    void ApplyDamage(GameObject target)
    {
        // Memberikan damage ke target
        PlayerHealth targetHealth = target.GetComponent<PlayerHealth>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }
    }
}
