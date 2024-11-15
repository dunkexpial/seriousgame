using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    public bool isMainMenu; // Public boolean to switch hotspot behavior

    private Vector2 cursorHotspot;

    // Start is called before the first frame update
    void Start()
    {
        // Set cursor hotspot based on isMainMenu
        if (isMainMenu)
        {
            cursorHotspot = new Vector2(0, 0); // Top left corner
        }
        else
        {
            cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2); // Center of the texture
        }

        // Set the cursor with the correct hotspot
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }
}
