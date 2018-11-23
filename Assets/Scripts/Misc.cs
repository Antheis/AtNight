using UnityEngine;

namespace Delegates
{
    public delegate void VoidDelegate();
}

public static class Layer
{
    private static int _interactable = LayerMask.GetMask("Interactable");
    public static int Interactable
    {
        get { return _interactable; }
    }
}