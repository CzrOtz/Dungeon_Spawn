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

    public testAgentScript bossScript; // Reference to the boss script

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
        // Check if all fountains are dead
        if (deadFountains == 7 && eosScript.enemyCount == 0 && !bossIsHere)
        {
            // Spawn the boss if conditions are met and the boss isn't spawned yet
            GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            bossScript = bossInstance.GetComponent<testAgentScript>(); // Get the boss script from the spawned boss
            bossIsHere = true; // Ensure the boss doesn't spawn again
        }

        // If the boss has spawned, check if it's dead to trigger the win
        if (bossIsHere && bossScript != null && bossScript.dead)
        {
            won = true;
            StartCoroutine(TriggerWin());
        }

        // Handle the switch to boss music once boss is here and 6 fountains are dead
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
