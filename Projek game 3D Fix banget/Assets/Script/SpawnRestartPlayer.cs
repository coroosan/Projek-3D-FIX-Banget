using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnRestartPlayer : MonoBehaviour
{
    public Transform spawnPoint; // Titik spawn pemain
    public GameObject player;
    public GameObject weapon;// Referensi ke objek pemain

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

            // Reset posisi dan rotasi kamera
            Camera playerCamera = player.GetComponentInChildren<Camera>();

            if (playerCamera != null)
            {
                // Reset posisi dan rotasi kamera agar sesuai dengan kondisi awal permainan
                playerCamera.transform.localPosition = Vector3.zero;  // Reset posisi kamera
                playerCamera.transform.localRotation = Quaternion.identity;  // Reset rotasi kamera
            }

            // Debug posisi setelah respawn
            Debug.Log("Player position after respawn: " + player.transform.position);
        }
        else
        {
            Debug.LogError("SpawnPoint or Player is not set correctly!");
        }
    }




    // Menambahkan metode untuk mendapatkan posisi spawn player
    public Vector3 GetPlayerStartPosition()
    {
        if (spawnPoint != null)
        {
            return spawnPoint.position; // Mengembalikan posisi spawn point
        }
        else
        {
            Debug.LogError("Spawn point is not set!");
            return Vector3.zero; // Mengembalikan posisi default jika spawn point tidak ditemukan
        }
    }
}
