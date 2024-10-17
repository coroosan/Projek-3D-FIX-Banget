using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            // Logika untuk mati (misalnya, menghapus pemain)
            Debug.Log("Player is dead!");
        }
    }
}