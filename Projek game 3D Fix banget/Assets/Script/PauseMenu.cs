using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    private bool _isPaused = false;

    public bool IsPaused => _isPaused;  // Properti untuk mengakses status pause

    void Update()
    {
        // Cek jika kita berada di dalam scene gameplay (misalnya bukan MainMenu)
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (!_isPaused && Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
        else
        {
            // Di MainMenu, hanya buka/tutup cursor saat menekan Escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleCursorVisibility();
            }
        }
    }

    public void ResumeGame()
    {
        _isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        _isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("tesMainMenu");
    }

    private void ToggleCursorVisibility()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
