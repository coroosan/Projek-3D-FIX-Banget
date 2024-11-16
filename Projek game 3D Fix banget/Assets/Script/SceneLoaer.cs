using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Start()
    {
        if (fadeImage != null)
        {
            Debug.Log("Starting Fade In in Game Scene");
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogError("fadeImage is missing in Game Scene!");
        }
    }


    // Method untuk memuat scene dengan nama
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    // Method untuk memuat scene dengan index
    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(FadeAndLoadScene(SceneManager.GetSceneByBuildIndex(sceneIndex).name));
    }

    // Coroutine untuk fade out sebelum load scene
    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadeImage != null)
        {
            // Aktifkan fadeImage dan lakukan fade-out
            fadeImage.gameObject.SetActive(true);
            for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
            {
                fadeImage.color = new Color(0, 0, 0, t / fadeDuration);
                yield return null;
            }
        }

        // Load scene baru
        SceneManager.LoadScene(sceneName);
    }


    private IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        for (float t = fadeDuration; t >= 0; t -= Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, t / fadeDuration);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(false);
    }


}
