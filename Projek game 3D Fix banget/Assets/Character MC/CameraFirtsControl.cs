using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFirtsControl : MonoBehaviour
{
    [SerializeField]
    Transform player, playerArms;
    [SerializeField]
    float mouseSensitivity;

    void Update() // 'update' harus diganti jadi 'Update'
    {
        Cursor.lockState = CursorLockMode.Locked;
        RotateCamera();
    }

    void RotateCamera()
    {
        float mouseY = Input.GetAxis("Mouse X"); // Mengambil input sumbu X dari mouse
        float mouseX = Input.GetAxis("Mouse Y"); // Mengambil input sumbu Y dari mouse

        float rotAmountX = mouseX * mouseSensitivity; // Menghitung rotasi sumbu X
        float rotAmountY = mouseY * mouseSensitivity; // Menghitung rotasi sumbu Y

        Vector3 rotPlayerArms = playerArms.transform.rotation.eulerAngles; // Mendapatkan rotasi dari playerArms
        Vector3 rotPlayer = player.transform.rotation.eulerAngles; // Mendapatkan rotasi dari player

        rotPlayerArms.x -= rotAmountY; // Mengurangi rotasi sumbu X untuk playerArms
        rotPlayerArms.z = 0; // Mengunci rotasi di sumbu Z
        rotPlayer.y += rotAmountX; // Menambah rotasi di sumbu Y untuk player

        playerArms.rotation = Quaternion.Euler(rotPlayerArms); // Mengatur rotasi baru untuk playerArms
        player.rotation = Quaternion.Euler(rotPlayer); // Mengatur rotasi baru untuk player
    }
}
