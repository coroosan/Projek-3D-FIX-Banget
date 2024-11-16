using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Jika sudah ada instance lain, hapus objek ini
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Agar musik tetap ada ketika pindah scene
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Memastikan AudioSource ada dan teratur
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void OnEnable()
    {
        // Menambahkan listener untuk event ketika scene baru dimuat
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Menghapus listener saat objek dihancurkan
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Fungsi ini dipanggil setiap kali scene baru dimuat
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Jika sudah masuk ke scene selain main menu, hentikan musik
        if (scene.name != "MainMenu") // Ganti "MainMenu" dengan nama scene menu Anda
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            Destroy(gameObject); // Hapus objek musik setelah berhenti
        }
    }
}
