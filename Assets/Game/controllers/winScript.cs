using UnityEngine;
using UnityEngine.SceneManagement; // To handle scene transitions
using System.Collections;
public class winScript : MonoBehaviour
{
    

    public int deadFountains = 0;

    [Header("Normal Music")]
    public AudioClip normalMusic;
    public AudioSource normalMusicSource;

    [Header("6 dead fountains")]

    public AudioClip sixFountains;
    public AudioSource sixFountainsSource;
    
    public static bool won = false;
    private eosScript eosScript;

    public bool bossIsHere = false;

    [Header("Boss Settings")]
    public GameObject bossPrefab; // Reference to the boss prefab
    public Transform bossSpawnPoint; // A Transform where the boss should be instantiated

    
    
    void Start()
    {
        eosScript = FindObjectOfType<eosScript>();
    
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWin();
    }

    void CheckForWin()
    {
        //if and only if all fountans are dead, we will check if all the enemies are dead
        if (deadFountains == 7)
        {
            if (eosScript.enemyCount == 0)
            { 

                if (eosScript.enemyCount == 0 && !bossIsHere)
                {
                    // Spawn the boss if the conditions are met and the boss isn't already there
                    Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
                    bossIsHere = true; // Ensure the boss doesn't spawn again
                }

            }
        }

        if (deadFountains == 7 && bossIsHere)
        {
            if (normalMusicSource.isPlaying)
            {
                normalMusicSource.Stop();  // Stop normal music
            }

            if (!sixFountainsSource.isPlaying)  // Ensure boss music is not already playing
            {
                sixFountainsSource.Play();  // Play the boss music
            }
        }
    }

    IEnumerator TriggerWin()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameOver");
    }
    
}
