using UnityEngine;
using TMPro;

public class eosScript : MonoBehaviour
{
    public int enemyCount = 0;
    public TMP_Text enemyCountText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateEnemyCountText();
    }

    // Update is called once per frame
    void Update()
    {
        // {EOS: + enemyCount }
        UpdateEnemyCountText();
    }

    public void Add() {
        enemyCount++;
        UpdateEnemyCountText();
    }

    public void Subtract() {
        if (enemyCount > 0)
        {
            enemyCount--;
            UpdateEnemyCountText();
        }
    }

    public void UpdateEnemyCountText() 
    {
        if (enemyCountText != null) 
        {
            enemyCountText.text = "EOS: " + enemyCount;
        }
    }

}
