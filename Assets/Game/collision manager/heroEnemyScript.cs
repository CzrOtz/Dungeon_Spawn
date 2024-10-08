using UnityEngine;

public class heroEnemyScript : MonoBehaviour
{
    private ghostScript ghost;
    private cyclopsScript cyclops;

    private crabScript crab;

    private GameObject hero;
    private heroScript heroScript;

    private heroHealthDisplayScript heroHealthDisplay;
    void Start()
    {
        heroHealthDisplay = FindObjectOfType<heroHealthDisplayScript>();
        ghost = FindObjectOfType<ghostScript>();
        cyclops = FindObjectOfType<cyclopsScript>();
        crab = FindObjectOfType<crabScript>();
        hero = GameObject.FindGameObjectWithTag("Hero");
        

        if (hero != null)
        {
            // Get the heroScript component from the hero GameObject
            heroScript = hero.GetComponent<heroScript>();

            if (heroScript == null)
            {
                Debug.LogError("heroScript component not found on Hero object.");
            }
        }

        if (hero == null)
        {
            Debug.LogError("Hero not found in H-E-S");
        }

        if (heroHealthDisplay == null)
        {
            Debug.LogError("hhd is not found");
        }        
        
    }

    // Update is called once per frame
    void Update()
    {
          
    }


    void UpDateHealthDisplay()
    {
        if (heroScript.health <= 0)
        {
            heroScript.health = 0;
            heroHealthDisplay.Copy(heroScript.health);
        }
    }

    
    public void ManageCollisions(Collision2D collision)
    {
        

        if (collision.gameObject.CompareTag("Ghost"))
        {
            ghostScript ghost = collision.gameObject.GetComponent<ghostScript>();
            if (ghost != null)
            {
                heroScript.takingDamage = true;
                heroScript.health -= ghost.attack_damage;
                heroScript.health = Mathf.Round(heroScript.health * 100f) / 100f;
                

                UpDateHealthDisplay();
            }
            else
            {
                Debug.LogError("ghost not found");
            }
        }

        if (collision.gameObject.CompareTag("cyclops"))
        {
            cyclopsScript cyclops = collision.gameObject.GetComponent<cyclopsScript>();

            if (cyclops != null)
            {
                heroScript.health -= cyclops.attack_damage;
                heroScript.health = Mathf.Round(heroScript.health * 100f) / 100f;

                

                UpDateHealthDisplay();
            }
            else
            {
                Debug.LogError("cyclops not found in colide 2d");
            }
        }

        if (collision.gameObject.CompareTag("crab"))
        {
            crabScript crab = collision.gameObject.GetComponent<crabScript>();

            if (crab != null)
            {
                heroScript.health -= crab.attack_damage;
                heroScript.health = Mathf.Round(heroScript.health * 100f) / 100f;

                

                UpDateHealthDisplay();
            }
            else
            {
                Debug.LogError("crab not found in colide 2d");
            }
        }
    }

    
}

