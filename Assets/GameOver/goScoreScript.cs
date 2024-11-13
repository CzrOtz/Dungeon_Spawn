using UnityEngine;
using TMPro;

public class goScoreScript : MonoBehaviour
{
    public TMP_Text gameOverScoreText;

    public DataCollector1Script gameOData;

    void Start()
    {
        int finalScore = scoreScript.score; // Access the static score
        gameOverScoreText.text = finalScore.ToString();
        gameOData.CollectScore(finalScore);
    }
}


