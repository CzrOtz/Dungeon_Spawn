using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class testDataScript : MonoBehaviour
{
    public static testDataScript Instance { get; private set; }
    private DatabaseReference reference;
    public bool isFirebaseInitialized { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                isFirebaseInitialized = true;
                Debug.Log("Firebase Initialized and ready to use.");
            }
            else
            {
                Debug.LogError("Failed to initialize Firebase: " + task.Exception);
            }
        });
    }

    // Method to send data to Firebase
    public void SendWinningDataToDatabase(WinningData winningData)
    {
        if (!isFirebaseInitialized)
        {
            Debug.Log("Firebase is not initialized yet!");
            return;
        }

        string uniqueKey = reference.Child("winning_run").Push().Key;
        string jsonData = JsonUtility.ToJson(winningData);

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

    public DatabaseReference GetReference()
    {
        if (isFirebaseInitialized)
        {
            return reference;
        }
        else
        {
            Debug.Log("Firebase not initialized yet!");
            return null;
        }
    }
}




// using UnityEngine;
// using Firebase;
// using Firebase.Database;
// using Firebase.Extensions;

// public class testDataScript : MonoBehaviour
// {
//     public static testDataScript Instance { get; private set; }
//     private DatabaseReference reference;

//     void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     void Start()
//     {
//         // if (FirebaseAuthManager.Instance != null)
//         // {
//         //     FirebaseAuthManager.Instance.OnUserAuthenticated += InitializeFirebase;
//         // }
//         // else
//         // {
//         //     Debug.LogError("FirebaseAuthManager instance not found in the scene.");
//         // }
//         Debug.Log("testDataScript: starting firebase");
//     }

//     private void InitializeFirebase()
//     {
//         FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//         {
//             if (task.IsCompleted)
//             {
//                 reference = FirebaseDatabase.DefaultInstance.RootReference;
//                 Debug.Log("Firebase Initialized and ready to use.");
//             }
//             else
//             {
//                 Debug.LogError("Failed to initialize Firebase: " + task.Exception);
//             }
//         });
//     }

//     public void SendWinningDataToDatabase(WinningData winningData)
//     {
//         if (reference == null)
//         {
//             Debug.LogError("Firebase database reference not initialized!");
//             return;
//         }

//         string uniqueKey = reference.Child("winning_run").Push().Key;
//         string jsonData = JsonUtility.ToJson(winningData);

//         reference.Child("winning_run").Child(uniqueKey)
//             .SetRawJsonValueAsync(jsonData)
//             .ContinueWithOnMainThread(task =>
//             {
//                 if (task.IsCompleted)
//                 {
//                     Debug.Log("Winning run data saved successfully!");
//                 }
//                 else
//                 {
//                     Debug.LogError("Failed to save winning run data: " + task.Exception);
//                 }
//             });
//     }

//     public DatabaseReference GetReference()
//     {
//         if (reference != null)
//         {
//             return reference;
//         }
//         else
//         {
//             Debug.LogError("Firebase database reference not initialized!");
//             return null;
//         }
//     }
// }







