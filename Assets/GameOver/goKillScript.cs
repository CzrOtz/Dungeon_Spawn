using UnityEngine;
using TMPro;

public class goKillScript : MonoBehaviour
{
    public TMP_Text gameOverKillCountText;

    void Start()
    {
        // Access the static killCount directly
        int finalKillCount = KcountScript.killCount;
        gameOverKillCountText.text = "Kills  " + finalKillCount.ToString();
    }
}



