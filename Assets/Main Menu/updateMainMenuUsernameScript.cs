using TMPro;
using UnityEngine;
using Firebase.Database;
using System.Collections;

public class updateMainMenuUsernameScript : MonoBehaviour
{
    public TMP_Text usernameText;
    private string currentUsername;
    public testDataScript testDataScriptInstance;

     private string lastCheckedUsername; // Store the last displayed username to detect changes

    void Start()
    {
        testDataScriptInstance = FindObjectOfType<testDataScript>();
        InitializeUsername();
        lastCheckedUsername = currentUsername; // Set the initial last checked username
    }

    void InitializeUsername()
    {
        if (PlayerPrefs.HasKey("currentUsername"))
        {
            currentUsername = PlayerPrefs.GetString("currentUsername");
            DisplayUsername();
        }
        else
        {
            StartCoroutine(GenerateUniqueUsername());
        }
    }

    void Update()
    {
        // Check if the username in PlayerPrefs has changed
        string updatedUsername = PlayerPrefs.GetString("currentUsername", "Not Set");
        if (updatedUsername != lastCheckedUsername)
        {
            currentUsername = updatedUsername;
            DisplayUsername();
            lastCheckedUsername = currentUsername; // Update the last checked username
        }
    }

    IEnumerator GenerateUniqueUsername()
    {
        bool isUnique = false;

        while (!isUnique)
        {
            currentUsername = "user" + Random.Range(1000000, 9999999);

            var checkUsernameTask = testDataScriptInstance.GetReference().Child("users")
                .OrderByChild("username")
                .EqualTo(currentUsername)
                .GetValueAsync();

            yield return new WaitUntil(() => checkUsernameTask.IsCompleted);

            if (checkUsernameTask.Result == null || !checkUsernameTask.Result.Exists)
            {
                isUnique = true;
            }
        }

        PlayerPrefs.SetString("currentUsername", currentUsername);
        PlayerPrefs.Save();
        DisplayUsername();
    }

    void DisplayUsername()
    {
        usernameText.text = currentUsername ?? "Not Set";
        Debug.Log("Username set to: " + currentUsername);
    }

    public void RefreshUsernameDisplay()
    {
        currentUsername = PlayerPrefs.GetString("currentUsername", "Not Set");
        DisplayUsername();
    }

    public string GetCurrentUsername()
    {
        return currentUsername;
    }
}



