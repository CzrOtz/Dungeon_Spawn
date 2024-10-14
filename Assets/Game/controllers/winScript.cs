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
                won = true;
                
                StartCoroutine(TriggerWin());
                //end the game
            }
        }

        if (deadFountains == 6)
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
