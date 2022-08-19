using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Texture2D pointer; // normal mouse pointer
 
    private void Update()
    {
        Cursor.SetCursor(pointer, new Vector2(12.5f, 12.5f), CursorMode.Auto);
    }
}
