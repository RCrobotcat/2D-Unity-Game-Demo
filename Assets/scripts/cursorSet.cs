using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorSet : MonoBehaviour
{
    public Texture2D cursorTexture;//Êó±êÌùÍ¼
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);//ÉèÖÃÊó±êÌùÍ¼
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
