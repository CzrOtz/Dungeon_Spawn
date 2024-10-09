using UnityEngine;
using UnityEngine.SceneManagement; // To handle scene transitions
using System.Collections;
public class winScript : MonoBehaviour
{
    // Start is called before the first frame update
    //this script has to check a few things.
    //1 check that all enemies are dead
    //2 check that all the fountains are dead 
    //there are 7 fountains
    // we will have a public (accessible) variable at 0
    //when a fountain dies, we will add 1 to the variable
    // when the vartiable equals 7, the first check is complete
    //we now have to give the fountains access to the deadFounts variable

    public int deadFountains = 0;

    //if and only if all the fountains are dead, we will check if all the enemies are dead
    //asure that its public and give each one its own copy.
    
    public static bool won = false;
    private eosScript eosScript;
    
    void Start()
    {
        eosScript = FindObjectOfType<eosScript>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWin();
    }

    void CheckForWin()
    {
        //if and only if all fountans are dead, we will check if all the enemies are dead
        if (deadFountains == 7)
        {
            if (eosScript.enemyCount == 0)
            {
                won = true;
                
                StartCoroutine(TriggerWin());
                //end the game
            }
        }
    }

    IEnumerator TriggerWin()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameOver");
    }
    
}
