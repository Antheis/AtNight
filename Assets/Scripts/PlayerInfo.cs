using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
	[HideInInspector]
	public int memories = 0;
	[HideInInspector]
	public int batteries = 0;
	public int batteryLife = 100;
	private bool flashlight = false;

	public Text batteryDisplay;

	void Start() {
		StartCoroutine(HandleFlashlightBattery());
	}

	public void addMemory() {
		++memories;
	}

	public void addBattery() {
		++batteries;
		batteryDisplay.text = "x " + batteries;
	}

	public void HandleFlashlight() {
		flashlight = !flashlight;
		Debug.Log("Flashlight: " + flashlight);
		if (flashlight)
			--batteryLife;
	}

	IEnumerator HandleFlashlightBattery() {
		yield return new WaitForSeconds(1);
		if (flashlight)
			--batteryLife;
	}
}
