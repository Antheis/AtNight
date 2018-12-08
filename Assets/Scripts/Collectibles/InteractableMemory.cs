using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(MemoryInfo))]
    public class InteractableMemory : InteractableBase
    {
        private MemoryInfo _info;

        private void Awake()
        {
            _info = GetComponent<MemoryInfo>();
        }

        public override void Interact()
        {
            _info.PickObject();
            Destroy(gameObject);
        }
    }
}
