using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject timerText; // Reference to the Timer Text GameObject
    public GameObject scoreText; // Reference to the Score Text GameObject

    void Start()
    {
        // Lock and hide the cursor during gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ensure the in-game UI elements are active
        if (timerText != null)
        {
            timerText.SetActive(true);
        }

        if (scoreText != null)
        {
            scoreText.SetActive(true);
        }
    }
}