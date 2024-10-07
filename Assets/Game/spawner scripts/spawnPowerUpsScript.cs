using System.Collections;
using UnityEngine;

public class spawnPowerUpsScript : MonoBehaviour
{
    public GameObject redBottlePrefab;
    public GameObject blueBottlePrefab;
    public GameObject greenBottlePrefab;
    public GameObject grayBottlePrefab;

    private GameObject currentBottle;  // Reference to the current active bottle

    // Timing settings
    private float spawnInterval = 7f;  
    private float bottleLifeTime = 15f;   

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to manage power-up spawning
        StartCoroutine(HandleBottleSpawning());
    }

    // Coroutine to manage the bottle spawn and disappear logic
    IEnumerator HandleBottleSpawning()
    {
        while (true)
        {
            // Wait for the spawn interval
            yield return new WaitForSeconds(spawnInterval);

            // Pick and spawn a random bottle
            pickARandomBottle();

            // Wait for the bottle lifetime
            yield return new WaitForSeconds(bottleLifeTime);

            // If the bottle is still there, destroy it
            if (currentBottle != null)
            {
                Destroy(currentBottle);
                currentBottle = null;
            }

            // Wait for 3 seconds before the next spawn cycle
            yield return new WaitForSeconds(3f);
        }
    }

    // Pick a random bottle to spawn
    void pickARandomBottle()
    {
        int randomBottle = Random.Range(1, 6);

        switch (randomBottle)
        {
           
            case 2:
                SpawnRedBottle();
                break;
            case 3:
                SpawnBlueBottle();
                
                break;
            case 4:
                SpawnGreenBottle();
                break;
            case 5:
                SpawnGrayBottle();
                
                break;
            default:
                SpawnRedBottle();
                break;
        }
    }

    // Bottle spawning methods
    void SpawnRedBottle()
    {
        currentBottle = Instantiate(redBottlePrefab, transform.position, transform.rotation);
    }

    void SpawnBlueBottle()
    {
        currentBottle = Instantiate(blueBottlePrefab, transform.position, transform.rotation);
    }

    void SpawnGreenBottle()
    {
        currentBottle = Instantiate(greenBottlePrefab, transform.position, transform.rotation);
    }

    void SpawnGrayBottle()
    {
        currentBottle = Instantiate(grayBottlePrefab, transform.position, transform.rotation);
    }
}

