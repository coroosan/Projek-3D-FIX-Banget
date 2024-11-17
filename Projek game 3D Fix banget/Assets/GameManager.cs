using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private EnemyWithNavMesh[] enemies;

    void Start()
    {
        // Cari semua musuh di scene dan simpan ke array
        enemies = FindObjectsOfType<EnemyWithNavMesh>();
    }

    public void RespawnPlayerAndEnemies()
    {
        // Respawn semua musuh yang ditemukan
        foreach (EnemyWithNavMesh enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Respawn();
            }
        }

        // Tambahkan logika respawn untuk player jika perlu
        Debug.Log("Player and enemies have been respawned!");
    }
}
