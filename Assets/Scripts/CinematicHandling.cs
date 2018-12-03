using Player;
using UnityEngine;
using UnityEngine.UI;

public class CinematicHandling : MonoBehaviour
{
	private CountdownTimer _countdownTimer;
	private Image _img;
	private FirstPersonController _player;
	private int fadeDuration = 3;
	private bool _fadeToBlack = false;
	
	void Awake ()
	{
		_countdownTimer = GetComponent<CountdownTimer>();
		_img = GetComponent<Image>();
		Show(fadeDuration);
		
		_player = FindObjectOfType<FirstPersonController>();
	}
	
	void Update ()
	{
		float remainingPerc = _countdownTimer.GetProportionTimeRemaining();
		Color c = _img.color;
		c.a = (_fadeToBlack) ? (1 - remainingPerc) : (remainingPerc);
		_img.color = c;
		if (remainingPerc <= 0.1f)
			_player.enabled = true;
		if (remainingPerc <= 0.01f)
			enabled = (false);
	}
	
	public void Show(int timerTotal)
	{
		_countdownTimer.ResetTimer(timerTotal);
		_fadeToBlack = false;
		enabled = (true);
	}

	public void FadeToBlack(int timerTotal)
	{
		_countdownTimer.ResetTimer(timerTotal);
		_fadeToBlack = true;
		enabled = (true);
	}
}
