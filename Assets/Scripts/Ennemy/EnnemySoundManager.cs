using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySoundManager : MonoBehaviour {

	private AudioSource _soundManager;
	public AudioClip walkSound1;
	public AudioClip walkSound2;
	public AudioClip attackSound;
	public AudioClip tpSound;
	public AudioClip idleSound1;
	public AudioClip idleSound2;

	public float idleTimer = 3.0f;
	private float tmpTimer;
	private bool hasAttacked = false;
	private bool footstep = false;
	private bool idle = false;
	private bool isOpen = false;

 	public PauseMenuInterface _pauseMenu;

	// Use this for initialization
	void Start () {
		_soundManager = GetComponent<AudioSource>();
		tmpTimer = Time.time;
		isOpen = _pauseMenu.isOpen();
	}

	// Update is called once per frame
	void Update () {
		isOpen = _pauseMenu.isOpen();
		if (hasAttacked || isOpen)
			return;

		if (Time.time - tmpTimer > idleTimer) {
			Idle();
			tmpTimer = Time.time;
		}
	}

	public void Idle() {
		if (isOpen)
			return;
		if (idle)
			_soundManager.PlayOneShot(idleSound1);
		else
			_soundManager.PlayOneShot(idleSound2);
		idle = !idle;
	}

	public void Attack() {
		if (isOpen)
			return;
		_soundManager.PlayOneShot(attackSound);
		hasAttacked = true;
	}

	public void Teleport() {
		if (isOpen)
			return;
		_soundManager.PlayOneShot(tpSound);
	}

	public void Walk() {
		if (isOpen)
			return;
		if (!_soundManager.isPlaying) {
			if (footstep)
				_soundManager.PlayOneShot(walkSound1);
			else
				_soundManager.PlayOneShot(walkSound2);
			footstep = !footstep;
		}
	}

}
