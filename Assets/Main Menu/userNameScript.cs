using UnityEngine;
using TMPro;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;

public class UserNameScript : MonoBehaviour
{
    [Header("Input Field")]
    public TMP_InputField userNameInput;

    [Header("Current Username")]
    public TMP_Text userNameText;

    [Header("Message Text")]
    public TMP_Text messageText;

    private string currentUsername;
    public testDataScript testDataScriptInstance;

    // Reference to the main menu script to refresh username display
    private updateMainMenuUsernameScript mainMenuUsernameScript;

    void Start()
    {
        testDataScriptInstance = FindObjectOfType<testDataScript>();
        mainMenuUsernameScript = FindObjectOfType<updateMainMenuUsernameScript>();

        DisplayCurrentUsername();
    }

    void DisplayCurrentUsername()
    {
        if (PlayerPrefs.HasKey("currentUsername"))
        {
            currentUsername = PlayerPrefs.GetString("currentUsername");
        }
        else
        {
            currentUsername = "Not Set";
        }
        userNameText.text = currentUsername;
    }

    public void ChangeUsername()
    {
        currentUsername = userNameInput.text;
        if (ValidateUsername(currentUsername))
        {
            PlayerPrefs.DeleteKey("currentUsername");
            StartCoroutine(ValidateAndSaveUsername());
        }
    }

    private HashSet<string> restrictedUserNames = new HashSet<string>
    {
        "Admin", "admin", "Moderator", "moderator", "christ", "Christ","jesus", "Jesus" ,"satan", "devil", "false", "God"
    };
    bool ValidateUsername(string username)
    {
        if (username.Length < 5 || username.Length > 14)
        {
            DisplayMessage("Username must be between 5 and 14 characters.");
            return false;

        }

        if (username.Contains(" "))
        {
            DisplayMessage("Username cannot contain spaces.");
            return false;
        }
        
       string lowerCaseUsername = username.ToLower();

       foreach (string restrictedName in restrictedUserNames)
         {
              if (lowerCaseUsername.Contains(restrictedName.ToLower()))
              {
                DisplayMessage("Please choose something else");
                return false;
              }
         }

        foreach (char c in username)
        {
            if (!char.IsLetterOrDigit(c) && c != '_')
            {
                DisplayMessage("Username can only contain letters, numbers, and underscores.");
                return false;
            }
        }
        return true;
    }

    IEnumerator ValidateAndSaveUsername()
    {
        bool isUnique = false;

        var checkUsernameTask = testDataScriptInstance.GetReference().Child("users")
            .OrderByChild("username")
            .EqualTo(currentUsername)
            .GetValueAsync();

        yield return new WaitUntil(() => checkUsernameTask.IsCompleted);

        if (checkUsernameTask.Result == null || !checkUsernameTask.Result.Exists)
        {
            isUnique = true;
        }

        if (isUnique)
        {
            PlayerPrefs.SetString("currentUsername", currentUsername);
            PlayerPrefs.Save();
            DisplayCurrentUsername();
            DisplayMessage("Username updated successfully.");

            // Refresh the home screen display
            mainMenuUsernameScript?.RefreshUsernameDisplay();
        }
        else
        {
            DisplayMessage("Username already exists. Try a different one.");
        }
    }

    void DisplayMessage(string message)
    {
        messageText.text = message;
        messageText.color = Color.red;
    }

    public string GiveCurrentUsername()
    {
        return currentUsername;
    }
}





