using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseMenuInterface : MonoBehaviour {
	private bool open = false;
	public FirstPersonController FPS;
	public GameObject battery;
	public GameObject stressBar;
	public void HandlePauseMenu() {
		open = !open;
		transform.gameObject.SetActive(open);
		FPS.enabled = !open;
		battery.SetActive(!open);
		stressBar.SetActive(!open);
		Time.timeScale = open ? 0.0f : 1.0f;
	}

	public bool isOpen() {
		return open;
	}
}
