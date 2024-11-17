using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Mencegah duplikasi Audio Manager
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Menetapkan instance dan mencegah penghancuran object
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Mendapatkan komponen AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // Menambahkan listener untuk event ketika scene baru dimuat
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Menghapus listener saat object dihancurkan
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Memeriksa nama scene
        if (scene.name == "MainMenu") // Ganti "MainMenu" sesuai dengan nama scene Main Menu Anda
        {
            // Memutar musik jika berada di Main Menu
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Menghentikan musik di scene lain
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
