using UnityEngine;

public class ObstacleInteraction : MonoBehaviour
{
    public QuestionsManager questionsManager; // Reference to the QuestionsManager

    private Question currentQuestion;

    void Start()
    {
        // Find the QuestionsManager in the scene
        questionsManager = FindObjectOfType<QuestionsManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowQuestion();
        }
    }

    private void ShowQuestion()
    {
        if (questionsManager != null)
        {
            currentQuestion = questionsManager.GetRandomQuestion();
            UIManager.Instance.ShowQuestionPanel(currentQuestion.questionText, currentQuestion.answers, this);
        }
        else
        {
            Debug.LogError("QuestionsManager not found.");
        }
    }

    public void CheckAnswer(int index)
    {
        if (index == currentQuestion.correctAnswerIndex)
        {
            ScoreManager.Instance.AddScore(1000); // Add 1000 points for correct answer
            Destroy(gameObject); // Remove the obstacle if the answer is correct
            Debug.Log("Correct answer!");
        }
        else
        {
            Debug.Log("Wrong answer. Try again.");
        }
    }
}