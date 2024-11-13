using TMPro;
using UnityEngine;

public class scoreScript : MonoBehaviour
{
    public static int score = 0;  // Static variable to store score across scenes
    public TMP_Text scoreText;

    void Start()
    {
        //assure to clear out the score since it is static
        score = 0;

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


