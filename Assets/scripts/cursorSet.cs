using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorSet : MonoBehaviour
{
    public Texture2D cursorTexture;//�����ͼ
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);//���������ͼ
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
