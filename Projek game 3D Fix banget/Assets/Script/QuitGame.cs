using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Fungsi ini akan dipanggil ketika button ditekan
    public void Quit()
    {
        // Keluar dari aplikasi
        Application.Quit();
        // Hanya untuk memastikan fungsi berjalan saat di editor
        Debug.Log("Game Closed!");
    }
}
