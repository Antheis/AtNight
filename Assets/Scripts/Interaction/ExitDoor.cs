using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class ExitDoor : InteractableBase
    {
        private MemoryHandler _memoryHandler;
        [SerializeField] private CanvasGroup _group;
        
        private void Awake()
        {
            _memoryHandler = FindObjectOfType<MemoryHandler>();
        }
        
        public override void Interact()
        {
            if (_memoryHandler.GetAllMemories())
                ; // ToDo: EndGame
            else
                StartCoroutine(DisplayHint(3f, 3f)); 
        }

        private IEnumerator DisplayHint(float duration, float fadeDuration)
        {
            _group.GetComponentInChildren<Text>().text =
                "You need to find " + _memoryHandler.TotalMemories + " more items ...";
            yield return new WaitForSeconds(duration);
            float actualDur = fadeDuration;
            while (actualDur > 0f)
            {
                var actualValue = actualDur / duration;
                _group.alpha = actualValue;
                actualDur -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
