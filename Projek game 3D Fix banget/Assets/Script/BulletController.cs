using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float bulletLifetime = 5f;  // Waktu hidup peluru sebelum dihancurkan
    private float bulletDamage;

    private void Start()
    {
        // Menghancurkan peluru setelah beberapa detik
        Destroy(gameObject, bulletLifetime);
    }

    // Set damage yang akan diberikan peluru
    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Mengecek apakah objek yang tertembak memiliki komponen "Health"
        Health targetHealth = collision.gameObject.GetComponent<Health>();
        if (targetHealth != null)
        {
            // Memberikan damage ke target
            targetHealth.TakeDamage(bulletDamage);
        }

        // Hancurkan peluru setelah bertabrakan
        Destroy(gameObject);
    }
}
