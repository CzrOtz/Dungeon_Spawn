using UnityEngine;

public class mainMenuContollerScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject instructionsMenu;

    public GameObject creditsMenu;

    public GameObject leaderboard;

    
   
    void Start()
    {
        mainMenu.SetActive(true);
        instructionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        leaderboard.SetActive(false);
    }

    public void triggerMainMenu()
    {
        mainMenu.SetActive(true);
        instructionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        leaderboard.SetActive(false);
    }

    public void triggerInstructionsMenu()
    {
        mainMenu.SetActive(false);
        instructionsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        leaderboard.SetActive(false);
    }

    public void triggerCreditsMenu()
    {
        mainMenu.SetActive(false);
        instructionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
        leaderboard.SetActive(false);
    }

    public void triggerLeaderboard()
    {
        mainMenu.SetActive(false);
        instructionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        leaderboard.SetActive(true);
    }
}
