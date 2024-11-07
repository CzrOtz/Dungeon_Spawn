using UnityEngine;
using TMPro;

public class winInstanceScript : MonoBehaviour
{
    [Header("TextMeshPro Objects")]
    public TMP_Text rankingTXT;
    public TMP_Text playerNameTXT;
    public TMP_Text pointsTXT;
    public TMP_Text killTXT;
    public TMP_Text timeTXT;
    public TMP_Text scoreTXT;

    // Set data for each instance
    public void SetData(int rank, WinningData data)
    {
        rankingTXT.text = rank.ToString();
        playerNameTXT.text = data.playerName;
        pointsTXT.text = data.points.ToString();
        killTXT.text = data.kills.ToString();
        timeTXT.text = data.time;
        scoreTXT.text = data.score.ToString();
    }
}
