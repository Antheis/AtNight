using UnityEngine;
using System.Collections;

public class CountdownTimer : MonoBehaviour 
{
	private float countdownTimerStartTime;
	private float countdownTimerDuration;

	public float GetTotalSeconds()
	{
		return countdownTimerDuration;
	}

	public void ResetTimer(float seconds)
	{
		countdownTimerStartTime = Time.time;
		countdownTimerDuration = seconds;
	}

	public float GetSecondsRemaining()
	{
		float elapsedSeconds = (Time.time - countdownTimerStartTime);
		float secondsLeft = (countdownTimerDuration - elapsedSeconds);
		return secondsLeft;
	}

	public float GetFractionSecondsRemaining()
	{
		float elapsedSeconds = (Time.time - countdownTimerStartTime);
		float secondsLeft = (countdownTimerDuration - elapsedSeconds);
		return secondsLeft;
	}

	public float GetProportionTimeRemaining()
	{
		float proportionLeft = (float)GetFractionSecondsRemaining() / (float)GetTotalSeconds();
		return proportionLeft;
	}
}
