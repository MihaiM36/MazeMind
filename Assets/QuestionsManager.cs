using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Question
{
    public string questionText;
    public string[] answers;
    public int correctAnswerIndex;
}

public class QuestionsManager : MonoBehaviour
{
    public List<Question> questions;
    private List<int> askedQuestionIndices = new List<int>();

    void Start()
    {
        ResetQuestions();
    }

    public Question GetRandomQuestion()
    {
        if (askedQuestionIndices.Count == questions.Count)
        {
            Debug.Log("All questions have been asked.");
            ResetQuestions(); // Optionally reset questions if all have been asked
        }

        int questionIndex;
        do
        {
            questionIndex = Random.Range(0, questions.Count);
        } while (askedQuestionIndices.Contains(questionIndex));

        askedQuestionIndices.Add(questionIndex);

        return questions[questionIndex];
    }

    public void ResetQuestions()
    {
        askedQuestionIndices.Clear();
    }
}