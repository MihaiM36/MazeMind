using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Debug.Log("MainMenu Start method called");
        // Ensure the cursor is unlocked and visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        Debug.Log("StartGame button pressed.");
        // Load the game scene, assuming it is called "GameScene"
        SceneManager.LoadScene("GameScene");
    }

    public void OpenOptions()
    {
        Debug.Log("Options button pressed.");
        // Open the options menu
        // For now, we'll just print a message to the console
    }

    public void ExitGame()
    {
        Debug.Log("Exit button pressed.");
        // Exit the game
        Application.Quit();
        // For the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}