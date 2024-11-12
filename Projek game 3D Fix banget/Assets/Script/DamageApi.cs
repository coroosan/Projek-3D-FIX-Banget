using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageApi : MonoBehaviour
{
    public float damagePerSecond = 10f; // Damage yang diberikan per detik

    private void OnTriggerStay(Collider other)

    {
        // Mengecek apakah objek yang masuk zona adalah player
        if (other.CompareTag("Player"))
        {
            // Mengambil komponen PlayerHealth dari objek player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Memberikan damage kepada player
                playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}