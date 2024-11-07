using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class spawnAWinInstanceScript : MonoBehaviour
{
    public GameObject winInstancePrefab; // Prefab for each leaderboard entry
    public Transform container; // Parent container for win instances
    private DatabaseReference reference;
    private testDataScript testDataScriptInstance;

    void Start()
    {
        if (testDataScript.Instance != null && testDataScript.Instance.isFirebaseInitialized)
        {
            reference = testDataScript.Instance.GetReference().Child("winning_run");
            PopulateLeaderboard();
        }
        else
        {
            Debug.LogError("testDataScript instance not found or not initialized.");
        }
    }


    private IEnumerator WaitForFirebaseInitialization()
    {
        Debug.Log("Step 2 --> Waiting for Firebase initialization...");
        while (!testDataScriptInstance.isFirebaseInitialized)
        {
            yield return null;
            Debug.Log("Step 2.5 --> Still waiting for Firebase initialization...");
        }

        // Firebase is ready
        reference = testDataScriptInstance.GetReference().Child("winning_run");
        Debug.Log("Step 2.75 --> Firebase initialized and DatabaseReference acquired.");
        
        PopulateLeaderboard();
    }

    
    void OnEnable() 
    {
        Debug.Log("Step 3 --> OnEnable() called. Preparing to populate leaderboard.");
        
        if (testDataScriptInstance != null && testDataScriptInstance.isFirebaseInitialized)
        {
            PopulateLeaderboard();
        }
        else if (testDataScriptInstance != null)
        {
            StartCoroutine(WaitForFirebaseInitialization());
        }
        else
        {
            Debug.LogError("Step 3.5 --> testDataScript instance not found in the scene during OnEnable.");
        }
    }

    public void PopulateLeaderboard()
    {
        Debug.Log("Step 4 --> Clearing previous leaderboard instances.");
        
        // Clear previous instances
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // Fetch data and populate
        FetchAndDisplayData();
        Debug.Log("Step 4.5 --> Fetching and displaying leaderboard data.");
    }

    private void FetchAndDisplayData()
    {
        Debug.Log("Step 5 --> Fetching leaderboard data from Firebase...");
    
        reference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                List<WinningData> winDataList = new List<WinningData>();

                DataSnapshot snapshot = task.Result;
                if (snapshot.ChildrenCount == 0)
                {
                    Debug.LogWarning("No data found in Firebase under 'winning_run'.");
                    return;
                }
            
                foreach (DataSnapshot entry in snapshot.Children)
                {
                    WinningData data = JsonUtility.FromJson<WinningData>(entry.GetRawJsonValue());
                    if (data != null)
                    {
                        winDataList.Add(data);
                    }
                    else
                    {
                        Debug.LogWarning("Encountered null data while parsing leaderboard entry.");
                    }
                }

                // Sort by points in descending order
                winDataList.Sort((x, y) => y.points.CompareTo(x.points));
                Debug.Log("Step 6 --> Data sorted by points in descending order.");

                // Spawn and populate each instance
                for (int i = 0; i < winDataList.Count; i++)
                {
                    Debug.Log($"Step 7 --> Spawning win instance {i + 1} with points: {winDataList[i].points}");
                
                    GameObject winInstance = Instantiate(winInstancePrefab, container);
                    if (winInstance == null)
                    {
                        Debug.LogError("Failed to instantiate winInstancePrefab.");
                        continue;
                    }

                    winInstanceScript instanceScript = winInstance.GetComponent<winInstanceScript>();
                    if (instanceScript != null)
                    {
                        instanceScript.SetData(i + 1, winDataList[i]);
                        Debug.Log($"Step 7.5 --> Win instance populated with data: {winDataList[i]}");
                    }
                    else
                    {
                        Debug.LogError("winInstanceScript component is missing from the winInstancePrefab.");
                    }
                }
            }
            else
            {
                Debug.LogError("Step 5.75 --> Failed to fetch leaderboard data: " + task.Exception);
            }
        });
    }

}


