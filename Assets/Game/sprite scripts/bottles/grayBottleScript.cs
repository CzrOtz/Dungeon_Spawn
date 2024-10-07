using UnityEngine;


public class grayBottleScript : MonoBehaviour
{
    // Start is called before the first frame update
    //this script will increase the speed of the spear

    private heroScript hero;
    private spearScript spear;

    public float spearSpeedBottleIncrease = 1.18f;

    private bool remove = false;

    private spearSpawnerScript spearSpawner;

    void Start()
    {
        spearSpawner = FindObjectOfType<spearSpawnerScript>();
        hero = FindAnyObjectByType<heroScript>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (remove == true) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
            if (spearSpawner.speed > 40)
            {
                spearSpawner.speed += 5;
            } else {
                spearSpawner.speed *= spearSpeedBottleIncrease;
            }

            Destroy(gameObject);
        }

        
    }

    



}
