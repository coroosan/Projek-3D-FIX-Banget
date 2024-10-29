using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPawnRocket : MonoBehaviour
{
    public GameObject rocketPrefab; // Prefab roket yang ingin dipanggil
    public float spawnHeight = 10f; // Ketinggian tempat roket muncul
    public int numberOfRockets = 5; // Jumlah roket yang akan muncul

    public Collider areaCollider;

    void Start()
    {
        // Ambil collider dari area tempat roket akan muncul
        areaCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ketika pemain memasuki area trigger, mulai spawn roket
            SpawnRockets();
        }
    }

    void SpawnRockets()
    {
        for (int i = 0; i < numberOfRockets; i++)
        {
            // Dapatkan posisi acak di dalam bounds collider
            Vector3 randomPoint = GetRandomPointInBounds(areaCollider.bounds);

            // Tentukan posisi roket dengan ketinggian yang sudah ditentukan
            Vector3 spawnPosition = new Vector3(randomPoint.x, spawnHeight, randomPoint.z);

            // Spawn roket
            Instantiate(rocketPrefab, spawnPosition, Quaternion.identity);
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