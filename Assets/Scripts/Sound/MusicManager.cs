using UnityEngine;

namespace Sound
{
    public class MusicManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        
        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        private void PlayMusic()
        {
            
        }
    }
}
