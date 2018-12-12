using UnityEngine;

public class MouseStartState : MonoBehaviour
{
    public bool CursorIsVisible;
    
    private void Awake()
    {
        Cursor.visible = CursorIsVisible;
        Destroy(this);
    }
}
