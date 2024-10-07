using UnityEngine;

public class spearSpawnerScript : MonoBehaviour
{
    public GameObject spearPrefab; // Prefab of the spear
    private GameObject currentSpear; // Reference to the currently spawned spear

    //speed and damage are here because if not, they would be reset
    // every time a new instance of spear is instantiated
    public float speed = 20f;

    public float damage = 15;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn the first spear when the game starts
        SpawnNewSpear();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the current spear is null (either fired and destroyed or never existed)
        if (currentSpear == null)
        {
            // Spawn a new spear after the previous one is destroyed
            SpawnNewSpear();
        }
        else
        {
            // Ensure the current spear stays attached to the spawner until it's fired
            if (!currentSpear.GetComponent<spearScript>().isMoving)
            {
                // Keep the spear attached to the spawner's position
                currentSpear.transform.position = transform.position;
            }
        }
    }

    // Method to spawn a new spear
    void SpawnNewSpear()
    {
        // Instantiate a new spear at the spawner's position and rotation
        currentSpear = Instantiate(spearPrefab, transform.position, transform.rotation);
    }
}
