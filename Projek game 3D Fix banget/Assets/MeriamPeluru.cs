using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeriamPeluru : MonoBehaviour
{
    public Transform target; // Target yang akan dituju (Player)
    public float speed = 10f; // Kecepatan peluru
    public float lifeTime = 5f; // Waktu hidup peluru sebelum hancur
    public int damage = 10; // Damage yang diberikan ke player


    void Start()
    {
        // Cari GameObject dengan tag "Player" dan ambil Transform-nya
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

        // Hancurkan peluru setelah lifeTime (untuk mencegah peluru bergerak terus)
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target != null)
        {
            // Gerakkan peluru menuju posisi player
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah peluru menyentuh player
        if (other.CompareTag("Player"))
        {
            // Ambil script PlayerHealth untuk mengurangi health
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Hancurkan peluru saat terkena player
            Destroy(gameObject);
        }
    }
}