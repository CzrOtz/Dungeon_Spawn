using TMPro;
using UnityEngine;

public class scoreScript : MonoBehaviour
{
    public static int score = 0;  // Static variable to store score across scenes
    public TMP_Text scoreText;

    void Start()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();
        }

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void IncreaseScore(int points)
    {
        score += points;
        UpdateScoreText();
    }
}


