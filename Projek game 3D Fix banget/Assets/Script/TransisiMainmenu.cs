using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Stage Settings")]
    public string nextSceneName; // Pastikan sama persis


    private void Start()
    {
        // Memastikan fadeImage aktif dan transparan di awal
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0); // Transparan
        }
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
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
