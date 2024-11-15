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

        if (spawnPoint != null && player != null)
        {
            // Cek apakah player memiliki CharacterController
            CharacterController characterController = player.GetComponent<CharacterController>();

            if (characterController != null)
            {
                // Pindahkan posisi menggunakan Move() jika ada CharacterController
                player.transform.position = spawnPoint.position;
                characterController.enabled = false;  // Nonaktifkan CharacterController sementara
                player.transform.position = spawnPoint.position;  // Set posisi lagi setelah nonaktifkan controller
                characterController.enabled = true;   // Aktifkan kembali CharacterController
            }
            else
            {
                // Jika tidak ada CharacterController, set posisi langsung
                player.transform.position = spawnPoint.position;
            }

            // Debug posisi setelah respawn
            Debug.Log("Player position after respawn: " + player.transform.position);
        }
        else
        {
            Debug.LogError("SpawnPoint or Player is not set correctly!");
        }
    }

}

