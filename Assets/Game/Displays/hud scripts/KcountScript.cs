using TMPro;
using UnityEngine;

public class KcountScript : MonoBehaviour
{
    public static int killCount;  // This must be static

    public TMP_Text killCountText;

    void Start()
    {
        //clear out the kill count since it is static;
        killCount = 0;

        killCountText = GetComponent<TMP_Text>();
        UpdateKillCountText();
    }

    void UpdateKillCountText()
    {
        killCountText.text = killCount.ToString();
    }

    public void IncreaseKillCount()
    {
        killCount++;
        UpdateKillCountText();
    }
}


