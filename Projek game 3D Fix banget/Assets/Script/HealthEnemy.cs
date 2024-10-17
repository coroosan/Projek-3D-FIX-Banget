using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " menerima damage: " + damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Logika ketika objek mati, seperti menghancurkan objek
        Debug.Log(gameObject.name + " mati!");
        Destroy(gameObject);
    }
}
