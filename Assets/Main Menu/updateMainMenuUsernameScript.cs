using TMPro;
using UnityEngine;
using System.Collections;

public class updateMainMenuUsernameScript : MonoBehaviour
{
    public TMP_Text usernameText;
    private string currentUsername;
    public testDataScript testDataScriptInstance;

    private string lastCheckedUsername;

    void Start()
    {
        testDataScriptInstance = FindObjectOfType<testDataScript>();
        InitializeUsername();
        lastCheckedUsername = currentUsername;
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
        string updatedUsername = PlayerPrefs.GetString("currentUsername", "Not Set");
        if (updatedUsername != lastCheckedUsername)
        {
            currentUsername = updatedUsername;
            DisplayUsername();
            lastCheckedUsername = currentUsername;
        }
    }

    IEnumerator GenerateUniqueUsername()
    {
        // Wait until Firebase is fully initialized in testDataScript
        yield return new WaitUntil(() => testDataScriptInstance != null && testDataScriptInstance.isFirebaseInitialized);

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

        // Add the username to the database
        testDataScriptInstance.GetReference().Child("users").Child(currentUsername).SetValueAsync(currentUsername);

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



