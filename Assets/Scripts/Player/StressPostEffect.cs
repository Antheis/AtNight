using AuraAPI;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Player
{
    public class StressPostEffect : MonoBehaviour
    {
        [SerializeField] private PostProcessVolume _calm;
        [SerializeField] private PostProcessVolume _stressed;
        
        private PlayerInfo _player;
        
        private void Awake()
        {
            _player = FindObjectOfType<PlayerInfo>();
        }

        private void Update()
        {
            float StressPerc = _player.stressBar / 100f;
            _calm.weight = StressPerc;
            _stressed.weight = 1f - StressPerc;
        }
    }
}
