using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InventoryInterface : MonoBehaviour {
	private bool open = false;
	public FirstPersonController FPS;

	public void HandleInventory() {
		open = !open;
		transform.gameObject.SetActive(open);
		FPS.enabled = !open;
		Time.timeScale = open ? 0.0f : 1.0f;
	}

	public bool isOpen() {
		return open;
	}
}
