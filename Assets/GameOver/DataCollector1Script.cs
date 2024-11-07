using UnityEngine;

public class DataCollector1Script : MonoBehaviour
{
    public float score;
    public string time;
    public int hour;
    public int minute;
    public int second;
    public int kills;
    public string playerName;  // Default player name for testing
    public bool win;

    public int points;  // Total points for the run

    void Start()
    {
        playerName = PlayerPrefs.GetString("currentUsername", "Not_Set"); // Fallback to "Dev_Testing_1" if not set

        if (win)
        {
            WinningRunData();
        }
    }

    public void CollectScore(float scr)
    {
        score = scr;
    }

    public void CollectTimeString(string tm)
    {
        time = tm;
    }

    public void CollectTimeRaw(int hr, int mnt, int snd)
    {
        hour = hr;
        minute = mnt;
        second = snd;
    }

    public void CollectKills(int klz)
    {
        kills = klz;
    }

    public void winOrLose(bool wn)
    {
        win = wn;
    }

    private void CalculatePoints()
    {
        // Reset points to avoid accumulation on multiple calls
        points = 0;

        // Calculate points for kills, score, and time
        Debug.Log("Calculating points for this run...");
        Debug.Log("points 1:   " + points);
        points += kills;  // Flat rate for kills
        Debug.Log("points 2:   " + points);
        points += (int)(score / 100);  // Scaled down score
        Debug.Log("points 3:   " + points);
        points += (int)(1000 * Mathf.Exp(-0.00288f * (hour * 3600 + minute * 60 + second - 600)));  // Exponential time scoring
        Debug.Log("points 4:   " + points);
        
        
    }

    public void WinningRunData()
    {
        if (win)
        {
            // Calculate points for this run
            CalculatePoints();

            WinningData winningData = new WinningData
            {
                playerName = playerName,
                score = score,
                time = time,
                hour = hour,
                minute = minute,
                second = second,
                kills = kills,
                points = points  // Attach calculated points to the winning data
            };

            if (testDataScript.Instance != null)
            {
                testDataScript.Instance.SendWinningDataToDatabase(winningData);
            }
            else
            {
                Debug.LogError("testDataScript instance not initialized.");
            }

        }
    }
}
 
