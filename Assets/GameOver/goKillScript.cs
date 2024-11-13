using UnityEngine;
using TMPro;

public class goKillScript : MonoBehaviour
{
    public TMP_Text gameOverKillCountText;
    
    public DataCollector1Script gameOData;

    void Start()
    {
        // Access the static killCount directly
        int finalKillCount = KcountScript.killCount;
        gameOverKillCountText.text = finalKillCount.ToString();
        gameOData.CollectKills(finalKillCount);
    }
}



