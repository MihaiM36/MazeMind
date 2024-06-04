using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject timerText; // Reference to the Timer Text GameObject
    public GameObject scoreText; // Reference to the Score Text GameObject
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        if (timerText != null)
        {
            timerText.SetActive(true);
        }
        if (scoreText != null)
        {
            scoreText.SetActive(true);
        }
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        if (timerText != null)
        {
            timerText.SetActive(false);
        }
        if (scoreText != null)
        {
            scoreText.SetActive(false);
        }
        Time.timeScale = 0f; // Pause game time
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume game time before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenOptions()
    {
        // Implement options functionality if needed
        Debug.Log("Options button pressed.");
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; // Resume game time before exiting
        SceneManager.LoadScene("MainMenuScene"); // Replace with your main menu scene name
    }
}