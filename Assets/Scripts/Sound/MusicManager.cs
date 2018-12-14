using UnityEngine;

namespace Sound
{
    public class MusicManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        private float _direction = 0;
        [SerializeField] private float _activatedVolume = 1;
        public bool IsEnabled = true;
        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
            
            LevelLoad.OnExitScene += Disable;
            _audioSource.volume = 0;
        }

        private void Start()
        {
            if (IsEnabled)
                Enable(2.5f);
        }

        private void OnDestroy()
        {
            LevelLoad.OnExitScene -= Disable;
        }

        public void Disable(float length)
        {
            if (!IsEnabled) return;
            _direction = (0 - _audioSource.volume) / length;
            IsEnabled = false;
        }
        
        public void Enable(float length)
        {
            if (IsEnabled) return;
            _direction = (_activatedVolume - _audioSource.volume) / length;
            IsEnabled = true;
        }
        
        private void Update()
        {
            if (_direction != 0)
            {
                _audioSource.volume = Mathf.Clamp(_audioSource.volume + _direction * Time.deltaTime, 0, _activatedVolume);
                if (_audioSource.volume >= _activatedVolume || _audioSource.volume <= 0)
                    _direction = 0;
            }
        }
    }
}
