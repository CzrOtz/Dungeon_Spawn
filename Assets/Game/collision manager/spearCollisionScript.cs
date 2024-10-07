using UnityEngine;

public class spearCollisionScript : MonoBehaviour
{
    public AudioClip enemyHitSound;    // Sound for hitting an enemy
    public AudioClip wallHitSound;     // Sound for hitting a wall

    public AudioSource audioSource;    // AudioSource component assigned via the Inspector

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

        if (audioSource == null)
        {
            Debug.LogError("audioSource not assigned.");
        }
    }

    public void ManageCollision(Collision2D collision)
    {
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

        if (collision.gameObject.CompareTag("Ghost"))
        {
            ghostScript ghost = collision.gameObject.GetComponent<ghostScript>();

            if (enemyHitSound != null && spearScript.isMoving && ghost.health > 0)
            {
                AudioSource.PlayClipAtPoint(enemyHitSound, transform.position, 1);
            }

            
            if (ghost != null && spearScript.isMoving)
            {
                ghost.TakeDamage(spearSpawner.damage);
            }

            Destroy(spear);
        }

        if (collision.gameObject.CompareTag("cyclops"))
        {
            cyclopsScript cyclops = collision.gameObject.GetComponent<cyclopsScript>();

            if (enemyHitSound != null && spearScript.isMoving && cyclops.health > 0 )
            {
                AudioSource.PlayClipAtPoint(enemyHitSound, transform.position, 1);
            }

            
            if (cyclops != null && spearScript.isMoving)
            {
                cyclops.TakeDamage(spearSpawner.damage);
            }

            Destroy(spear);
        }

        if (collision.gameObject.CompareTag("crab"))
        {
            crabScript crab = collision.gameObject.GetComponent<crabScript>();

            if (enemyHitSound != null && spearScript.isMoving && crab.health > 0)
            {
                AudioSource.PlayClipAtPoint(enemyHitSound, transform.position, 1);
            }

            
            if (crab != null && spearScript.isMoving)
            {
                crab.TakeDamage(spearSpawner.damage);
            }

            Destroy(spear);
        }

        // New code: Handle collision with spawn point
        if (collision.gameObject.CompareTag("spawnPoint"))
        {
            if (enemyHitSound != null && spearScript.isMoving)
            {
                AudioSource.PlayClipAtPoint(enemyHitSound, transform.position, 1);
            }

            spawnHealtScript spawnPoint = collision.gameObject.GetComponent<spawnHealtScript>();
            if (spawnPoint != null && spearScript.isMoving)
            {
                // Decrease the spawn point's health by the spear's damage
                spawnPoint.TakeDamage(spearSpawner.damage);
            }

            Destroy(spear);
        }

        if (collision.gameObject.CompareTag("wall"))
        {
            if (wallHitSound != null && spearScript.isMoving)
            {
                AudioSource.PlayClipAtPoint(wallHitSound, transform.position, 0.35f);
            }

            Destroy(spear);
        }
    }
}

