using UnityEngine;

public class goCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Ensure the system cursor is visible again in this scene
        Cursor.visible = true;

        // Optionally reset the cursor lock state to make sure it's not confined
        Cursor.lockState = CursorLockMode.None;
    }
}
