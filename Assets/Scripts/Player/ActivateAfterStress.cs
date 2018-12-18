using System.Collections;
using System.Collections.Generic;
using Sound;
using UnityEngine;

public class ActivateAfterStress : MonoBehaviour {

	[SerializeField] private MusicManager _mono;

	public float Treshhold = 25f;
	private PlayerInfo _player;
        
	private void Awake()
	{
		_player = FindObjectOfType<PlayerInfo>();
	}

	private void Update()
	{
		float StressPerc = _player.stressBar;
		if (StressPerc < Treshhold)
			_mono.Enable(5f);
		else
			_mono.Disable(2.5f);
	}
}
