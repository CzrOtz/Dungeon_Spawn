using UnityEngine;

public class mainMenuContollerScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject instructionsMenu;

    public GameObject creditsMenu;

    public GameObject leaderboard;

    public GameObject userNameMenu;
   
    void Start()
    {
        mainMenu.SetActive(true);
        instructionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        leaderboard.SetActive(false);
        userNameMenu.SetActive(false);
    }

    public void triggerMainMenu()
    {
        mainMenu.SetActive(true);
        instructionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        leaderboard.SetActive(false);
        userNameMenu.SetActive(false);
    }

    public void triggerInstructionsMenu()
    {
        mainMenu.SetActive(false);
        instructionsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        leaderboard.SetActive(false);
        userNameMenu.SetActive(false);
    }

    public void triggerCreditsMenu()
    {
        mainMenu.SetActive(false);
        instructionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
        leaderboard.SetActive(false);
        userNameMenu.SetActive(false);
    }

    public void triggerLeaderboard()
    {
        mainMenu.SetActive(false);
        instructionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        leaderboard.SetActive(true);
        userNameMenu.SetActive(false);
    }

    public void triggerUserNameMenu()
    {
        mainMenu.SetActive(false);
        instructionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        leaderboard.SetActive(false);
        userNameMenu.SetActive(true);
    }
}
