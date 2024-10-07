using UnityEngine;

public class greenBottleScript : MonoBehaviour
{
    //this script will increase the health of the hero

    // Start is called before the first frame update
    
    private heroScript hero;
    public float greenBottleHealthIncrease = 25f;
    void Start()
    {
        hero = FindObjectOfType<heroScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
            
            if (hero.health > 100)
            {
                hero.health += 10;
            } else {
                hero.health += greenBottleHealthIncrease;
            }

            Destroy(gameObject);
        }

        
    }
}
