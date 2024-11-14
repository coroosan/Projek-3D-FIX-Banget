using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // Instance singleton

    public AudioClip footstepSound; // Clip suara langkah kaki
    public AudioClip jumpSound; // Clip suara lompatan
    public AudioClip shootSound; // Clip suara tembakan

    private AudioSource audioSource; // AudioSource untuk memutar suara

    void Awake()
    {
        // Membuat instance jika belum ada
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Menambahkan AudioSource pada GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Fungsi untuk memutar suara
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
