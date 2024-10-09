using UnityEngine;

public class blueBottleScript : MonoBehaviour
{

    //this script will increase the speed of the hero
    //starting speed should be 10 
    //decelaration rate should be 10 constant 

    private heroScript hero;
    private float increaseSpeedRate = 1.10f;


    // Start is called before the first frame update
    void Start()
    {
        hero = FindObjectOfType<heroScript>();
        // Debug.Log("hero.speed += increaseSpeedRate: " + (hero.speed *= increaseSpeedRate));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
          if (hero.speed > 95)
          {
              hero.speed += 0.2f;
          } else {
              hero.speed *= increaseSpeedRate;
          }

          Destroy(gameObject);
        }

        
    }
    
}
