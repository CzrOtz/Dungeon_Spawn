using UnityEngine;

public class spearCollisionScript : MonoBehaviour
{
    public AudioClip enemyHitSound;    // Sound for hitting an enemy
    public AudioClip wallHitSound;     // Sound for hitting a wall

    public AudioSource enemyHitAudioSource;  // AudioSource for enemy hit sound
    public AudioSource wallHitAudioSource;   // AudioSource for wall hit sound

    private spearScript spearScript;
    private GameObject spear;
    private spearSpawnerScript spearSpawner;

    // Start is called before the first frame update
    void Start()
    {
        spear = GameObject.FindGameObjectWithTag("Spear");
        if (spear != null)
        {
            spearScript = spear.GetComponent<spearScript>();
            if (spearScript == null)
            {
                Debug.LogError("spearScript not found on spear.");
            }
        }
        else
        {
            Debug.Log("Spear GameObject not found yet");
        }

        spearSpawner = FindObjectOfType<spearSpawnerScript>();
        if (spearSpawner == null)
        {
            Debug.LogError("spearSpawnerScript not found.");
        }

        if (enemyHitAudioSource == null || wallHitAudioSource == null)
        {
            Debug.LogError("One or more AudioSources not assigned.");
        }
    }

    public void ManageCollision(Collision2D collision)
    {
        // Ensure spear is assigned
        if (spear == null)
        {
            spear = GameObject.FindGameObjectWithTag("Spear");
            if (spear != null)
            {
                spearScript = spear.GetComponent<spearScript>();
                if (spearScript == null)
                {
                    Debug.LogError("spearScript not found on spear.");
                    return;
                }
            }
            else
            {
                Debug.LogError("Spear not found during collision.");
                return;
            }
        }

        if (spearScript == null)
        {
            Debug.LogError("spearScript is still null. Cannot proceed.");
            return;
        }

        // Handle collision with Ghost
        if (collision.gameObject.CompareTag("Ghost"))
        {
            ghostScript ghost = collision.gameObject.GetComponent<ghostScript>();

            if (enemyHitAudioSource != null && spearScript.isMoving && ghost.health > 0)
            {
                enemyHitAudioSource.PlayOneShot(enemyHitSound);
            }

            if (ghost != null && spearScript.isMoving)
            {
                ghost.TakeDamage(spearSpawner.damage);
            }

            Destroy(spear);
        }

        // Handle collision with Cyclops
        if (collision.gameObject.CompareTag("cyclops"))
        {
            cyclopsScript cyclops = collision.gameObject.GetComponent<cyclopsScript>();

            if (enemyHitAudioSource != null && spearScript.isMoving && cyclops.health > 0)
            {
                enemyHitAudioSource.PlayOneShot(enemyHitSound);
            }

            if (cyclops != null && spearScript.isMoving)
            {
                cyclops.TakeDamage(spearSpawner.damage);
            }

            Destroy(spear);
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            testAgentScript boss = collision.gameObject.GetComponent<testAgentScript>();

            enemyHitAudioSource.PlayOneShot(enemyHitSound);
            Destroy(spear);
        }

        // Handle collision with Crab
        if (collision.gameObject.CompareTag("crab"))
        {
            crabScript crab = collision.gameObject.GetComponent<crabScript>();

            if (enemyHitAudioSource != null && spearScript.isMoving && crab.health > 0)
            {
                enemyHitAudioSource.PlayOneShot(enemyHitSound);
            }

            if (crab != null && spearScript.isMoving)
            {
                crab.TakeDamage(spearSpawner.damage);
            }

            Destroy(spear);
        }

        // Handle collision with Spawn Point
        if (collision.gameObject.CompareTag("spawnPoint"))
        {
            if (enemyHitAudioSource != null && spearScript.isMoving)
            {
                enemyHitAudioSource.PlayOneShot(enemyHitSound);
            }

            spawnHealtScript spawnPoint = collision.gameObject.GetComponent<spawnHealtScript>();
            if (spawnPoint != null && spearScript.isMoving)
            {
                spawnPoint.TakeDamage(spearSpawner.damage);
            }

            Destroy(spear);
        }

        // Handle collision with Wall
        if (collision.gameObject.CompareTag("wall"))
        {
            if (wallHitAudioSource != null && spearScript.isMoving)
            {
                wallHitAudioSource.PlayOneShot(wallHitSound);
            }

            Destroy(spear);
        }
    }
}


