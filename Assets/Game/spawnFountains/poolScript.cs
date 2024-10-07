using UnityEngine;

public class poolScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;


    public spawnHealtScript spawnHealthScript;

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        //THIS NEEDS TO BE PUBLIC 
        
    }

    void Update()
    {
        // If the spawn point is dead, render the pool tile
        if (spawnHealthScript != null && spawnHealthScript.Dead())
        {
            RenderPoolTile();
        }
    }

    // Function to render "tile_0031"
    public void RenderPoolTile()
    {
        // Load the sprite from the "Assets/Resources/Sprites" folder
        Sprite poolTile = Resources.Load<Sprite>("sprites/tile_0031");

        // Check if the sprite was found
        if (poolTile != null)
        {
            // Set the sprite to the SpriteRenderer component
            spriteRenderer.sprite = poolTile;
        }
        else
        {
            Debug.LogError("Sprite 'tile_0031' not found in the 'Sprites' folder.");
        }
    }
}
