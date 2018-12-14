using UnityEngine;

namespace Sound
{
	public class LevelMusicManager : MonoBehaviour
	{
		[SerializeField] private MusicManager[] _allMusic;
		[SerializeField] private int[] _musicThreshold;
		private int _playedMusic = -1;

		private void Awake()
		{
			MemoryHandler mem = FindObjectOfType<MemoryHandler>();
			mem.OnNewMemoryPickedUp += OnNewMemoryPickedUp;
		}
		
		private void Start()
		{
			OnNewMemoryPickedUp(0);
		}

		private void OnNewMemoryPickedUp(int nbPickedUp)
		{
			int lastIdx = -1;
			for (int x = 0; x < _musicThreshold.Length; x++)
			{
				if (_musicThreshold[x] <= nbPickedUp)
					lastIdx = x;
			}
			PlayMusic(lastIdx);
		}

		private void PlayMusic(int idx)
		{
			if (_playedMusic != -1)
				_allMusic[_playedMusic].Disable(1f);
			_playedMusic = idx;
			_allMusic[_playedMusic].Enable(1f);
		}
	}
}
