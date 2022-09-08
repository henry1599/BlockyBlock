using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture2D;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture2D, Vector3.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
