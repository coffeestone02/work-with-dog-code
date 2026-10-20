using UnityEngine;

public class CursorController : MonoBehaviour
{
    void Start()
    {
        Texture2D cursorIcon = Resources.Load<Texture2D>("Cursor/Default");
        Cursor.SetCursor(cursorIcon, new Vector2(cursorIcon.width / 4, cursorIcon.height / 4), CursorMode.Auto);
    }
}
