using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 180f; // 3 minutes in seconds
    private float remainingTime;

    public Text timerText; // Reference to the UI Text component that displays the timer
    public GameObject gameOverPanel; // Reference to the Game Over panel
    public GameObject congratulationsPanel; // Reference to the Congratulations panel
    public Text finalScoreText; // Reference to the Final Score Text

    private bool timerRunning = true;

    void Start()
    {
        remainingTime = totalTime;
        UpdateTimerDisplay();
        gameOverPanel.SetActive(false); // Ensure the Game Over panel is hidden at the start
        congratulationsPanel.SetActive(false); // Ensure the Congratulations panel is hidden at the start
    }

    void Update()
    {
        if (timerRunning)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (remainingTime <= 0)
            {
                timerRunning = false;
                TimeUp();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TimeUp()
    {
        Debug.Log("Time is up!");
        // Show the Game Over panel
        gameOverPanel.SetActive(true);
        // Make the cursor visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Optionally, pause the game by setting time scale to 0
        Time.timeScale = 0f;
    }

    public void PlayerReachedEndPoint()
    {
        timerRunning = false;
        int bonusScore = CalculateBonusScore();
        ScoreManager.Instance.AddScore(bonusScore);
        Debug.Log("Player reached the end point! Bonus score: " + bonusScore);
        ShowCongratulationsPanel(bonusScore);
    }

    private int CalculateBonusScore()
    {
        // For example, give 10 points for every second left
        return Mathf.FloorToInt(remainingTime) * 10;
    }

    private void ShowCongratulationsPanel(int bonusScore)
    {
        congratulationsPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + ScoreManager.Instance.GetScore();
        // Make the cursor visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Optionally, pause the game by setting time scale to 0
        Time.timeScale = 0f;
    }

    public void ExitGame()
    {
        // Function to exit the game
        Debug.Log("Exiting game...");
        Application.Quit();
        // For the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}