using UnityEngine;
using TMPro;

public class goTimeScript : MonoBehaviour
{
    public TMP_Text gameOverTimeText;

    public DataCollector1Script gameOData;
    void Start()
    {
        float finalTime = timeScript.elapsedTime; // Access the static elapsed time

        int hours = Mathf.FloorToInt(finalTime / 3600);
        int minutes = Mathf.FloorToInt((finalTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(finalTime % 60);
        string time;

        gameOverTimeText.text = string.Format("Time  {0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        time = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        gameOData.CollectTimeString(time);
        gameOData.CollectTimeRaw(hours, minutes, seconds);
    }
}

