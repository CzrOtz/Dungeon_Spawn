using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class testDataScript : MonoBehaviour
{
    private DatabaseReference reference;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase Initialized and ready to use.");
            }
            else
            {
                Debug.LogError("Failed to initialize Firebase: " + task.Exception);
            }
        });
    }

   public void SendWinningDataToDatabase(WinningData winningData)
    {
        // Debug.Log("1 This section is now prepping the data to be sent through the pipeline");

        // Generate a unique key for each winning run
        string uniqueKey = reference.Child("winning_run").Push().Key;

        string jsonData = JsonUtility.ToJson(winningData);
        // Debug.Log("3 this is the prepped data -> " + jsonData);

        // Use the unique key to create a new sub-node under "winning_run"
        reference.Child("winning_run").Child(uniqueKey)
            .SetRawJsonValueAsync(jsonData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Winning run data saved successfully!");
                }
                else
                {
                    Debug.LogError("Failed to save winning run data: " + task.Exception);
                }
            });
    }
}




