using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRocket : MonoBehaviour
{
    [Header("Rocket Settings")]
    public GameObject rocketPrefab; // Prefab roket yang ingin dipanggil
    public float spawnHeight = 10f; // Ketinggian tempat roket muncul
    public int numberOfRockets = 10; // Jumlah roket yang muncul sekaligus
    public float spawnInterval = 1.5f; // Interval waktu antar spawn roket

    private Collider areaCollider; // Collider dari area trigger
    private Coroutine spawnCoroutine; // Menyimpan Coroutine yang sedang berjalan

    void Start()
    {
        // Ambil collider dari area tempat roket akan muncul
        areaCollider = GetComponent<Collider>();

        // Pastikan collider diatur sebagai trigger
        if (areaCollider != null && !areaCollider.isTrigger)
        {
            Debug.LogWarning("Collider tidak diatur sebagai trigger. Mengaktifkan trigger...");
            areaCollider.isTrigger = true;
        }
    }

    // Ketika pemain memasuki area trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pemain masuk area trigger, mulai spawn roket!");
            if (spawnCoroutine == null) // Hanya mulai coroutine jika belum ada yang berjalan
            {
                spawnCoroutine = StartCoroutine(SpawnRocketsWithInterval());
            }
        }
    }

    // Ketika pemain keluar dari area trigger
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pemain keluar area trigger, menghentikan spawn roket!");
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine); // Hentikan Coroutine spawn roket
                spawnCoroutine = null; // Reset Coroutine
            }
        }
    }

    IEnumerator SpawnRocketsWithInterval()
    {
        if (rocketPrefab == null)
        {
            Debug.LogError("Prefab roket tidak diatur!");
            yield break;
        }

        // Loop terus-menerus hingga dihentikan
        while (true)
        {
            // Memunculkan sejumlah roket sekaligus
            for (int i = 0; i < numberOfRockets; i++)
            {
                // Dapatkan posisi acak di dalam bounds collider
                Vector3 randomPoint = GetRandomPointInBounds(areaCollider.bounds);

                // Tentukan posisi roket dengan ketinggian yang sudah ditentukan
                Vector3 spawnPosition = new Vector3(randomPoint.x, spawnHeight, randomPoint.z);

                // Spawn roket
                Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);
            }

            // Tunggu sesuai interval sebelum spawn berikutnya
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        // Mengacak titik di dalam bounds collider
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, bounds.center.y, randomZ);
    }
}
