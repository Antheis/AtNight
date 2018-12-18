using UnityEngine;

public class MouseStartState : MonoBehaviour
{
    public bool CursorIsVisible;
    
    private void Awake()
    {
        Cursor.visible = CursorIsVisible;
        Cursor.lockState = CursorLockMode.None;
        Destroy(this);
    }
}
