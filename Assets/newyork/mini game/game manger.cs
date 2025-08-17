using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    public Question[] questions;                 // fill in Inspector

    private static List<Question> unansweredQuestions;
    private Question currentQuestion;

    [Header("UI")]
    [SerializeField] private Image cardImage;    // big UI Image on your Canvas

    [Header("Sounds (only these)")]
    public AudioSource correctSfx;               // AudioSource with "correct" clip
    public AudioSource wrongSfx;                 // AudioSource with "wrong" clip

    void Start()
    {
        // safety: if no questions, bail early
        if (questions == null || questions.Length == 0)
        {
            Debug.LogWarning("No questions assigned on GameManager.");
            return;
        }

        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
            unansweredQuestions = questions.ToList();

        SetCurrentQuestion();
    }

    void SetCurrentQuestion()
    {
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
            unansweredQuestions = questions.ToList();

        int idx = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[idx];
        unansweredQuestions.RemoveAt(idx);

        if (cardImage) cardImage.sprite = currentQuestion.img;
        else Debug.LogWarning("Card Image not assigned on GameManager.");
    }

    // Hook these to your buttons
    public void OnTruePressed() => HandleAnswer(true);
    public void OnFalsePressed() => HandleAnswer(false);

    void HandleAnswer(bool pickedTrue)
    {
        bool isCorrect = currentQuestion != null && (pickedTrue == currentQuestion.isTrue);

        if (isCorrect)
        {
            if (correctSfx) correctSfx.Play();
        }
        else
        {
            if (wrongSfx) wrongSfx.Play();
        }

        // immediately load next picture
        SetCurrentQuestion();
    }
}
