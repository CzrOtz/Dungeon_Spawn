using System.Collections.Generic;
using UnityEngine;

public class cursorScript : MonoBehaviour
{
    [SerializeField] private List<Sprite> cursorSprites; // List to hold multiple sprites (Assign in Inspector)
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial sprite to the first one in the list (idle)
        if (cursorSprites != null && cursorSprites.Count > 0)
        {
            spriteRenderer.sprite = cursorSprites[0];
        }
        
        // Hide the system cursor
        Cursor.visible = false;
    }

    void Update()
    {
        // Move the object to follow the cursor's position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;

        // Change sprite based on mouse click (switch between first and second sprite)
        if (Input.GetMouseButton(0) && cursorSprites.Count > 1)
        {
            spriteRenderer.sprite = cursorSprites[1]; // Use second sprite when mouse is pressed
        }
        else
        {
            spriteRenderer.sprite = cursorSprites[0]; // Use first sprite when mouse is not pressed
        }
    }
}
