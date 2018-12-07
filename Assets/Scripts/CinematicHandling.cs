using Player;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CinematicHandling : MonoBehaviour
{
	private CountdownTimer _countdownTimer;
	private Image _img;
	private FirstPersonController _player;
	private float fadeDuration = 0.5f;
	private bool _fadeToBlack = false;
	private CanvasGroup _group;

	void Awake ()
	{
		_group = GetComponentInChildren<CanvasGroup>();
		_countdownTimer = GetComponent<CountdownTimer>();
		_img = GetComponent<Image>();
		Show(fadeDuration);
		
		_player = FindObjectOfType<FirstPersonController>();
	}
	
	void Update ()
	{
		float remainingPerc = _countdownTimer.GetProportionTimeRemaining();
		
		_group.alpha = (_fadeToBlack) ? (1 - remainingPerc) : (remainingPerc);

		if (remainingPerc <= 0.1f && _player != null)
			_player.enabled = true;
		if (remainingPerc <= 0f)
			enabled = (false);
	}
	
	public void Show(float timerTotal)
	{
		_countdownTimer.ResetTimer(timerTotal);
		_fadeToBlack = false;
		enabled = (true);
	}

	public void FadeToBlack(float timerTotal)
	{
		_countdownTimer.ResetTimer(timerTotal);
		_fadeToBlack = true;
		enabled = (true);
	}
}
