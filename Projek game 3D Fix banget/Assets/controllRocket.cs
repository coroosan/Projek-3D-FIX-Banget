using UnityEngine;

public class RocketControll : MonoBehaviour
{
    public float fallSpeed = 10f; // Kecepatan jatuh roket
    public GameObject explosionEffect; // Prefab efek ledakan
    public int damage = 20; // Damage yang akan diberikan

    void Update()
    {
        // Buat roket jatuh
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Cek apakah roket menyentuh objek dengan tag "Enemy"
        if (other.CompareTag("Player")) // Hanya memeriksa tag "Enemy"
        {
            Explode();
            ApplyDamage(other.gameObject);
        }
    }

    void Explode()
    {
        // Memunculkan efek ledakan
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Menghancurkan roket setelah meledak
        Destroy(gameObject);
    }

    void ApplyDamage(GameObject target)
    {
        // Implementasikan logika untuk memberikan damage ke target
        PlayerHealth targetHealth = target.GetComponent<PlayerHealth>(); // Ubah dari Health ke PlayerHealth
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }
    }
}
