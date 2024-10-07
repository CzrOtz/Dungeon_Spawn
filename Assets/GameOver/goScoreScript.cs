using UnityEngine;
using TMPro;

public class goScoreScript : MonoBehaviour
{
    public TMP_Text gameOverScoreText;

    void Start()
    {
        int finalScore = scoreScript.score; // Access the static score
        gameOverScoreText.text = "Score  " + finalScore.ToString();
    }
}


