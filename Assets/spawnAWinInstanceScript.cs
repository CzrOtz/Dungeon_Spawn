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

    void Start()
    {
        SetupReference();
    }

    void OnEnable()
    {
        SetupReference();
        if (reference != null)
        {
            PopulateLeaderboard();
        }
        else
        {
            Debug.LogWarning("Firebase not initialized; will retry to populate leaderboard when ready.");
        }
    }

    private void SetupReference()
    {
        if (testDataScript.Instance != null && testDataScript.Instance.GetReference() != null)
        {
            reference = testDataScript.Instance.GetReference().Child("winning_run");
        }
        else
        {
            Debug.LogError("testDataScript instance or database reference not available.");
        }
    }

    public void PopulateLeaderboard()
    {
        // Clear previous instances
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // Fetch data and populate
        FetchAndDisplayData();
    }

    private void FetchAndDisplayData()
    {
        if (reference == null)
        {
            Debug.LogError("Database reference not set.");
            return;
        }

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

                // Spawn and populate each instance
                for (int i = 0; i < winDataList.Count; i++)
                {
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
                    }
                    else
                    {
                        Debug.LogError("winInstanceScript component missing from winInstancePrefab.");
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to fetch leaderboard data: " + task.Exception);
            }
        });
    }
}




