using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;  // Untuk akses ke VideoPlayer
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Stage Settings")]
    public string nextSceneName; // Pastikan sama persis dengan nama scene Stage 1
    public VideoPlayer videoPlayer; // Referensi ke VideoPlayer

    private void Start()
    {
        // Memastikan fadeImage aktif dan transparan di awal
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0); // Transparan
        }

        // Menambahkan listener untuk event selesai video
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd; // Event ketika video selesai
        }

        // Memastikan kursor bisa digunakan setelah scene dimulai
        ResetCursorState();
    }

    // Metode yang akan dipanggil dari script lain
    public void TriggerSceneTransition()
    {
        StartCoroutine(TransitionToNextScene());
    }

    private IEnumerator TransitionToNextScene()
    {
        // Fade Out (menuju hitam)
        yield return StartCoroutine(FadeOut());

        // Ganti ke stage selanjutnya
        LoadNextStage();
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    private void LoadNextStage()
    {
        // Memastikan kursor bisa digunakan setelah scene berpindah
        ResetCursorState();

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // Event handler untuk saat video selesai
    private void OnVideoEnd(VideoPlayer vp)
    {
        // Mulai transisi setelah video selesai diputar
        StartCoroutine(TransitionToNextScene());
    }

    // Metode untuk memastikan kursor terlihat dan tidak terkunci
    private void ResetCursorState()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f; // Pastikan game tidak dalam keadaan pause
    }
}
