using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class CinematicHandling : MonoBehaviour {
	private CountdownTimer countdownTimer;
	private Image img;
	public FirstPersonController FPS;
	private int fadeDuration = 3;

	void Awake () {
		countdownTimer = GetComponent<CountdownTimer>();
		img = GetComponent<Image>();
		StartFading(fadeDuration);
		FPS.enabled = false;
	}
	
	void Update () {
		float alphaRemaining = countdownTimer.GetProportionTimeRemaining();
		Debug.Log(alphaRemaining);
		Color c = img.color;
		c.a = alphaRemaining;
		img.color = c;
		if (alphaRemaining < 0.1)
			FPS.enabled = true;
		if (alphaRemaining < 0.01)
			Destroy(transform.parent.gameObject);
	}
	
	public void StartFading (int timerTotal) {
		countdownTimer.ResetTimer(timerTotal);
	}
}
