using UnityEngine;

namespace Interaction
{
    public class InteractableChild : InteractableBase
    {
        [SerializeField] private InteractableBase _root;
        private void Awake()
        {
            if (_root == null)
                _root = GetComponentInParent<InteractableBase>();
        }
    
        public override void Interact()
        {
            _root.Interact();    
        }
    }
}
