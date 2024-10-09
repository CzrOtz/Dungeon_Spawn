using UnityEngine;
using TMPro;


public class heroHealthDisplayScript : MonoBehaviour
{
    public float heroHealth;

    public TMP_Text heroHealthText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealthDisplay();
    }

    // Update is called once per frame
    public void Update()
    {
        //its called here
        UpdateHealthDisplay();
    }


    public void Copy(float actualHealth)
    {
        heroHealth = actualHealth;
    }
    public void UpdateHealthDisplay() 
    {
        if (heroHealthText != null) 
        {
            heroHealthText.text = "Health: " + heroHealth;
        }
    }
}
