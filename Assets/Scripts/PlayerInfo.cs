﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
	[HideInInspector]
	public int batteries = 0;
	public int batteryLife = 100;
	[HideInInspector]
	public int pills = 0;
	public float stressBar = 100.0f;

	public Text batteryDisplay;
	public Text pillDisplay;
	public Slider flashlightDisplay;
	public Slider stressDisplay;

	private Light _flashlight;
	public bool FlashLightIsOn
	{
		get { return (_flashlight.enabled); }
	}
	private void Awake()
	{
		_flashlight = GetComponentInChildren<Light>();
		
		batteryDisplay.text = "x " + batteries;
		pillDisplay.text = "x " + pills;
		StartCoroutine(HandleFlashlightBattery());
	}

	private void UpdateFlashlight()
	{
		flashlightDisplay.value = batteryLife;
		Color col = flashlightDisplay.fillRect.GetComponent<Image>().color;
		col.g = 200 * (flashlightDisplay.value * 0.01f) / 255;
		col.b = col.g;
		flashlightDisplay.fillRect.GetComponent<Image>().color = col;
	}

	private void UpdateStress()
	{
		stressDisplay.value = stressBar;
		Color col = stressDisplay.fillRect.GetComponent<Image>().color;
		col.g = stressDisplay.value * 0.01f;
		col.b = col.g;
		stressDisplay.fillRect.GetComponent<Image>().color = col;
	}

	public void addBattery() {
		++batteries;
		batteryDisplay.text = "x " + batteries;
	}

	public void useBattery() {
		if (batteries == 0)
			return ;
		--batteries;
		batteryDisplay.text = "x " + batteries;
		batteryLife += 50;
		if (batteryLife > 100)
			batteryLife = 100;
		UpdateFlashlight();
	}

	public void addPill() {
		++pills;
		pillDisplay.text = "x " + pills;
	}

	public void usePill() {
		if (pills == 0)
			return ;
		--pills;
		pillDisplay.text = "x " + pills;
		stressBar += 50;
		if (stressBar > 100)
			stressBar = 100;
		UpdateStress();
	}

	public void StopFlashLight()
	{
		HandleFlashlight(false);
	}
	public void HandleFlashlight(bool on)
	{
		if (!FlashLightIsOn && on)
			--batteryLife;
		_flashlight.enabled = on;
	}

	IEnumerator HandleFlashlightBattery() {
		yield return new WaitForSeconds(1);
		if (FlashLightIsOn) {
			--batteryLife;
			UpdateFlashlight();
		} else {
			stressBar -= 0.5f;
			UpdateStress();
		}
		StartCoroutine(HandleFlashlightBattery());
	}
}
