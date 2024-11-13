using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnRestartPlayer : MonoBehaviour
{
    public Transform spawnPoint; // Titik spawn pemain
    public GameObject player;    // Referensi ke objek pemain

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // Atau metode lain untuk mendapatkan referensi ke pemain
        }
    }

    public void RespawnPlayer()
    {
        Debug.Log("Respawning player...");
        player.transform.position = spawnPoint.position;
        player.SetActive(true);  // Pastikan pemain diaktifkan
    }
}

