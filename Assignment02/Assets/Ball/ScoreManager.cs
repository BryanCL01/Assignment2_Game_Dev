using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText; // Assign a UI Text element for displaying the score
    private int score;

    public void UpdateScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Score: " + score;
    }
}

