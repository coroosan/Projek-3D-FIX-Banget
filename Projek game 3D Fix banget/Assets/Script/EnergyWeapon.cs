using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // Tambahkan untuk menggunakan Slider

public class EnergyWeaponWithSlider : MonoBehaviour
{
    public float energyUsagePerShot = 10f;  // Jumlah energi per tembakan
    public float maxEnergy = 100f;          // Maksimal energi
    public float currentEnergy;             // Energi saat ini
    public bool canShoot = true;            // Apakah senjata dapat menembak
    public float energyCooldownTime = 5f;   // Waktu cooldown energi
    public float energyRegenRate = 10f;    // Laju regenerasi energi

    public Slider energySlider; // Referensi ke UI Slider
    public delegate void ShootEventHandler(); // Event untuk tembakan
    public event ShootEventHandler OnShoot; // Event untuk menembak

    void Start()
    {
        currentEnergy = maxEnergy; // Set energi awal ke maksimal
        UpdateEnergySlider();  // Update slider saat mulai
    }

    public void Shoot()
    {
        if (currentEnergy > 0 && canShoot)
        {
            currentEnergy -= energyUsagePerShot;
            Debug.Log("Energi saat ini: " + currentEnergy);

            // Perbarui slider energi
            UpdateEnergySlider();

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
            currentEnergy = Mathf.Min(currentEnergy, maxEnergy); // Jangan lebih dari maksimal
            UpdateEnergySlider(); // Perbarui slider saat regenerasi
            yield return null;
        }

        canShoot = true;
        Debug.Log("Energi pulih, siap menembak!");
    }

    // Fungsi untuk memperbarui slider energi
    void UpdateEnergySlider()
    {
        if (energySlider != null)
        {
            energySlider.value = currentEnergy / maxEnergy; // Update slider dengan proporsi energi saat ini
        }
    }
}
