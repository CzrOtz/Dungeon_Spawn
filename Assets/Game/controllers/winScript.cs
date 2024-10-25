using UnityEngine;
using UnityEngine.SceneManagement; // To handle scene transitions
using System.Collections;

public class winScript : MonoBehaviour
{
    public int deadFountains = 0;

    [Header("Normal Music")]
    public AudioClip normalMusic;
    public AudioSource normalMusicSource;

    [Header("Build Up Music")]
    public AudioClip buildUpMusic;
    public AudioSource buildUpMusicSource;

    [Header("Boss Music (6 dead fountains)")]
    public AudioClip sixFountains;
    public AudioSource sixFountainsSource;

    public static bool won = false;
    private eosScript eosScript;

    public bool bossIsHere = false;
    private bool countdownStarted = false; // Track whether countdown music has started

    [Header("Boss Settings")]
    public GameObject bossPrefab; // Reference to the boss prefab
    public Transform bossSpawnPoint; // A Transform where the boss should be instantiated

    public testAgentScript bossScript; // Reference to the boss script

    public GameObject lightPrefab;
    public GameObject lightingInstance;

    public float countDown = 30f;

    public bossCountDownScript bossCountdownUI; // Reference to the boss countdown script

    void Start()
    {
        eosScript = FindObjectOfType<eosScript>();
        if (eosScript == null)
        {
            Debug.LogError("eosScript not found in the scene.");
        }

        if (bossCountdownUI == null)
        {
            Debug.LogError("bossCountDownScript not found in the scene. Make sure the script is attached to a UI element.");
        }
    }

    void Update()
    {
        CheckForWin();
    }

    void CheckForWin()
    {
        if (!bossIsHere)
        {
            if (deadFountains >= 7)
            {
                // All fountains destroyed before countdown ends, spawn boss immediately
                SpawnBoss();

                // Hide the countdown UI if it's visible
                if (bossCountdownUI != null)
                {
                    bossCountdownUI.HideCountDown();
                }
            }
            else if (deadFountains == 6)
            {
                // Start the countdown if not already started
                if (!countdownStarted)
                {
                    StartCountdownMusic();
                    countdownStarted = true;
                }

                // Decrease the countdown timer
                countDown -= Time.deltaTime;

                // Update the countdown UI
                int timeRemaining = Mathf.CeilToInt(countDown);
                if (bossCountdownUI != null)
                {
                    bossCountdownUI.ShowCountDown(timeRemaining);
                }
                else
                {
                    Debug.LogError("Cannot show countdown: bossCountdownUI is null.");
                }

                // Spawn the boss if the countdown reaches zero
                if (countDown <= 0)
                {
                    SpawnBoss();

                    // Hide the countdown UI
                    if (bossCountdownUI != null)
                    {
                        bossCountdownUI.HideCountDown();
                    }
                }
            }
        }
        else
        {
            // Boss is present
            if (deadFountains >= 7)
            {
                // All fountains destroyed, make boss vulnerable
                if (bossScript != null)
                {
                    bossScript.canTakeDamage = true;
                }
            }
            else
            {
                // Fountains remain, boss is invulnerable
                if (bossScript != null)
                {
                    bossScript.canTakeDamage = false;
                }
            }
        }

        // Check if win conditions are met
        if (bossScript.dead && deadFountains == 7 && eosScript.enemyCount == 0)
        {
            won = true;
            if (lightingInstance != null)
            {
                Destroy(lightingInstance);
            }
            StartCoroutine(TriggerWin());
        }
        
    }

    // Method to start the build-up music and stop normal music
    void StartCountdownMusic()
    {
        // Stop the normal music
        if (normalMusicSource.isPlaying)
        {
            normalMusicSource.Stop();
        }

        // Start the build-up music
        if (!buildUpMusicSource.isPlaying)
        {
            buildUpMusicSource.clip = buildUpMusic;
            buildUpMusicSource.Play();
        }
    }

    // Method to spawn the boss and handle music
    void SpawnBoss()
    {
        // Stop all previous music
        if (normalMusicSource.isPlaying)
        {
            normalMusicSource.Stop();
        }
        if (buildUpMusicSource.isPlaying)
        {
            buildUpMusicSource.Stop();
        }

        // Start playing the boss music
        if (!sixFountainsSource.isPlaying)
        {
            sixFountainsSource.clip = sixFountains;
            sixFountainsSource.Play();
        }

        // Spawn the boss and lighting instance
        GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        bossScript = bossInstance.GetComponent<testAgentScript>(); // Get the boss script from the spawned boss
        bossIsHere = true; // Ensure the boss doesn't spawn again
        lightingInstance = Instantiate(lightPrefab, new Vector3(7, 16, 0), Quaternion.identity);

        // Initially, the boss cannot take damage
        if (bossScript != null)
        {
            bossScript.canTakeDamage = false;
        }
    }

    IEnumerator TriggerWin()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameOver");
    }
}




