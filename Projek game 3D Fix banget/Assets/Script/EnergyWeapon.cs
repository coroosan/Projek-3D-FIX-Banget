using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyWeaponWithSlider : MonoBehaviour
{
    public float maxEnergy = 100f; // Energi maksimum
    private float currentEnergy; // Energi saat ini
    public float energyUsagePerShot = 10f; // Energi yang digunakan per tembakan
    public float energyRegenRate = 5f; // Kecepatan regenerasi energi per detik
    public float energyCooldownTime = 2f; // Waktu cooldown sebelum regenerasi energi

    public Slider energySlider; // Referensi ke Slider UI

    private bool canShoot = true; // Apakah pemain bisa menembak

    void Start()
    {
        currentEnergy = maxEnergy; // Set energi awal ke maksimum
        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
            energySlider.value = currentEnergy;
        }
    }

    void Update()
    {
        // Handle tembakan jika tombol ditekan
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            Shoot();
        }

        // Handle regenerasi energi
        if (!canShoot)
        {
            StartCoroutine(CooldownAndRegenerate());
        }

        // Update tampilan UI Slider
        UpdateEnergyUI();
    }

    void Shoot()
    {
        if (currentEnergy > 0)
        {
            currentEnergy -= energyUsagePerShot;
            Debug.Log("Energi saat ini: " + currentEnergy);

            // Cek apakah energi habis
            if (currentEnergy <= 0)
            {
                currentEnergy = 0;
                canShoot = false; // Hentikan tembakan jika energi habis
                Debug.Log("Energi habis, cooldown dimulai...");
            }
        }
    }

    // Coroutine untuk cooldown dan regenerasi energi
    IEnumerator CooldownAndRegenerate()
    {
        // Tunggu selama cooldown
        yield return new WaitForSeconds(energyCooldownTime);

        // Mulai regenerasi energi
        while (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRegenRate * Time.deltaTime;
            currentEnergy = Mathf.Min(currentEnergy, maxEnergy); // Pastikan tidak melebihi max
            UpdateEnergyUI();
            yield return null;
        }

        canShoot = true; // Izinkan menembak lagi setelah energi pulih
        Debug.Log("Energi pulih, siap menembak!");
    }

    // Fungsi untuk memperbarui UI Slider
    void UpdateEnergyUI()
    {
        if (energySlider != null)
        {
            energySlider.value = currentEnergy;
        }
    }
}
