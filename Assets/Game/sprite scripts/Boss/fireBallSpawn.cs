using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fireBallSpawn : MonoBehaviour
{
    public GameObject fireBallPrefab;         // The fireball prefab to spawn
    public int numberOfFireBalls = 3;         // Number of fireballs to spawn at the same time
    public float spawnInterval = 0.5f;        // Interval between spawning each fireball
    public float rotationRadius = 3f;         // Radius of the circular path
    public float rotationSpeed = 50f;         // Speed at which fireballs orbit (degrees per second)
    public float launchDelay = 5f;            // Time before the first fireball launches
    public float launchInterval = 1f;         // Time between each fireball's launch
    public float coolDown = 10f;              // Cooldown time after all fireballs are destroyed

    private List<GameObject> spawnedFireBalls = new List<GameObject>();
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnFireBallsCoroutine());
    }

    IEnumerator SpawnFireBallsCoroutine()
    {
        while (true)
        {
            // Spawn fireballs
            isSpawning = true;
            for (int i = 0; i < numberOfFireBalls; i++)
            {
                SpawnFireBall(i);
                yield return new WaitForSeconds(spawnInterval);
            }
            isSpawning = false;

            // Wait until all fireballs are destroyed
            while (spawnedFireBalls.Exists(fb => fb != null))
            {
                yield return null;
            }

            // Clear the list of fireballs
            spawnedFireBalls.Clear();

            // Wait for cooldown
            yield return new WaitForSeconds(coolDown);
        }
    }

    void SpawnFireBall(int index)
    {
        // Calculate the angle for positioning (in radians)
        float angle = index * Mathf.PI * 2f / numberOfFireBalls;
        Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * rotationRadius;

        // Instantiate the fireball
        GameObject fireBall = Instantiate(fireBallPrefab, spawnPosition, Quaternion.identity);

        // Set fireball to orbit around the boss
        fireBallScript fbScript = fireBall.GetComponent<fireBallScript>();
        if (fbScript != null)
        {
            fbScript.SetOrbitParams(transform, rotationRadius, rotationSpeed, angle);

            // Set individual waitForLaunch time to stagger the launches
            fbScript.waitForLaunch = launchDelay + launchInterval * index;
        }

        // Add to the list of spawned fireballs
        spawnedFireBalls.Add(fireBall);
    }
}



