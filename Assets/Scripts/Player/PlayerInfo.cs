using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
	[HideInInspector]
	public int batteries = 0;
	public float batteryLife = 100;
	[HideInInspector]
	public int pills = 0;
	public float stressBar = 100.0f;

	public float StressLoss = 1.5f;
	public float BatterieLoss = 1.5f;

    [Space(5)]
    [Header("Display")]
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
	}

    private void Update()
    {
	    if (FlashLightIsOn) {
		    batteryLife = Mathf.Clamp(batteryLife - (BatterieLoss*Time.deltaTime), 0f, 100f);
		    if (batteryLife <= 0f)
			    HandleFlashlight(false);
		    UpdateFlashlight();
	    } else {
		    stressBar = Mathf.Clamp(stressBar - (StressLoss*Time.deltaTime), 0f, 100f);
		    UpdateStress();
	    }
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

	public void AddBattery()
	{
		++batteries;
		batteryDisplay.text = "x " + batteries;
	}

	public void UseBattery()
	{
		if (batteries == 0)
			return ;
		--batteries;
		batteryDisplay.text = "x " + batteries;
		batteryLife += 50;
		if (batteryLife > 100)
			batteryLife = 100;
		UpdateFlashlight();
	}

	public void AddPill() {
		++pills;
		pillDisplay.text = "x " + pills;
	}

	public void UsePill()
	{
		if (pills == 0)
			return ;
		--pills;
		pillDisplay.text = "x " + pills;
		stressBar += 50;
		if (stressBar > 100)
			stressBar = 100;
		UpdateStress();
	}

	public AudioSource FlashLightClick;
	
	public void StopFlashLight()
	{
		HandleFlashlight(false);
	}
	public void HandleFlashlight(bool on)
	{
		if (FlashLightClick != null)
			FlashLightClick.Play();
		_flashlight.enabled = on && (batteryLife > 0f);
	}
}
