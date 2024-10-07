using UnityEngine;

public class redBottleScript : MonoBehaviour
{
    //increases spear power
    //has to spawn at 
    // Start is called before the first frame update
    //need a reference to the spear
    // need a reference to the hero



    //hero e
    private heroScript hero;
    private spearSpawnerScript spearSpawner;

    private float spearDamageBottleIncrease = 1.20f;
    void Start()
    {
        //we can reference in the start because the hero and weapon will be there
        spearSpawner = FindObjectOfType<spearSpawnerScript>();
        hero = FindAnyObjectByType<heroScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
              
            if (spearSpawner.damage > 100)
            {
                spearSpawner.damage += 5;
            } else {
                spearSpawner.damage += spearDamageBottleIncrease;
            }

            Destroy(gameObject);

        }
    }


}
