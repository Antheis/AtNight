using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class ExitDoor : InteractableBase
    {
        private LevelLoad _levelLoad;
        private MemoryHandler _memoryHandler;
        [SerializeField] private CanvasGroup _group;
        
        private void Awake()
        {
            _memoryHandler = FindObjectOfType<MemoryHandler>();
            _levelLoad = FindObjectOfType<LevelLoad>();
        }
        
        public override void Interact()
        {
            if (_memoryHandler.GetAllMemories())
            {
                _levelLoad.ChangeScene("VictoryScene");
                var ennemies = GameObject.FindGameObjectsWithTag("Ennemy");
                foreach (var ennemy in ennemies)
                    ennemy.SetActive(false);
            }
            else
                StartCoroutine(DisplayHint(3f, 3f)); 
        }

        private IEnumerator DisplayHint(float duration, float fadeDuration)
        {
            _group.GetComponentInChildren<Text>().text =
                "You need to find " + (6 - _memoryHandler.TotalMemories) + " more items ...";
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
