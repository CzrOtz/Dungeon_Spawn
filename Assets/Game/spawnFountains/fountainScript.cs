using UnityEngine;

public class fountainScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public spawnHealtScript spawnHealthScript;

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Find the spawnHealtScript in the scene
        
    }

    void Update()
    {
        // If the spawn point is dead, render the fountain tile
        if (spawnHealthScript != null && spawnHealthScript.Dead())
        {
            RenderFountainTile();
        }
    }

    // Function to render "tile_0007"
    public void RenderFountainTile()
    {
        // Load the sprite from the "Assets/Resources/Sprites" folder
        Sprite fountainTile = Resources.Load<Sprite>("sprites/tile_0007");

        // Check if the sprite was found
        if (fountainTile != null)
        {
            // Set the sprite to the SpriteRenderer component
            spriteRenderer.sprite = fountainTile;
        }
        else
        {
            Debug.LogError("Sprite 'tile_0007' not found in the 'Sprites' folder.");
        }
    }
}

