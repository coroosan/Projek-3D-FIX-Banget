using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    public int playerHealth = 100;

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log("Player Health: " + playerHealth); // Check in Console if health reduces

        CheckHealth(); // Check health after applying damage
    }

    public void CheckHealth()
    {
        if (playerHealth <= 0)
        {
            ActivateRagdoll();
        }
    }

    private void ActivateRagdoll()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }
        Debug.Log("Ragdoll activated!");
    }
}
