using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // Referensi ke UI Image untuk efek fade
    public float fadeDuration = 1f; // Durasi efek fade

    void Start()
    {
        // Memulai fade in ketika scene dimulai
        StartCoroutine(FadeIn());
    }

    // Fungsi untuk memulai permainan dari Main Menu ke Cutscene
    public void StartGame(string cutsceneSceneName)
    {
        StartCoroutine(FadeOutAndLoadScene(cutsceneSceneName));
    }

    // Fungsi Fade In (transparan ke hitam)
    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 1f; // Mulai dari warna hitam penuh

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    // Fungsi Fade Out (hitam ke transparan)
    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0f; // Mulai dari transparan

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Setelah fade selesai, load scene berikutnya
        SceneManager.LoadScene(sceneName);
    }
}
