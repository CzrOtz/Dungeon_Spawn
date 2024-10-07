using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManagerScript : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();
    }

}
