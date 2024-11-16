using System.Collections;
using UnityEngine;

public class EnergyWeaponWithSlider : MonoBehaviour
{
    public float energyUsagePerShot = 10f;  // Jumlah energi per tembakan
    public float maxEnergy = 100f;          // Maksimal energi
    public float currentEnergy;             // Energi saat ini
    public bool canShoot = true;            // Apakah senjata dapat menembak
    public float energyCooldownTime = 5f;   // Waktu cooldown energi
    public float energyRegenRate = 10f;    // Laju regenerasi energi

    public delegate void ShootEventHandler(); // Event untuk tembakan
    public event ShootEventHandler OnShoot; // Event untuk menembak

    void Start()
    {
        currentEnergy = maxEnergy; // Set energi awal ke maksimal
    }

    public void Shoot()
    {
        if (currentEnergy > 0 && canShoot)
        {
            currentEnergy -= energyUsagePerShot;
            Debug.Log("Energi saat ini: " + currentEnergy);

            if (currentEnergy <= 0)
            {
                currentEnergy = 0;
                canShoot = false; // Hentikan tembakan jika energi habis
                StartCoroutine(CooldownAndRegenerate());
                Debug.Log("Energi habis, cooldown dimulai...");
            }

            OnShoot?.Invoke(); // Panggil event untuk tembakan
        }
        else
        {
            Debug.Log("Energi tidak cukup untuk menembak!");
        }
    }

    IEnumerator CooldownAndRegenerate()
    {
        yield return new WaitForSeconds(energyCooldownTime);

        while (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRegenRate * Time.deltaTime;
            currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            yield return null;
        }

        canShoot = true;
        Debug.Log("Energi pulih, siap menembak!");
    }
}
