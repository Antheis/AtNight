using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HandleMenuInteraction : MonoBehaviour {
	private Text text;
	public GameObject img;

	void Start()
	{
		text = GetComponent<Text>();
	}

	public void OnPointerEnter() {
		text.color = new Color(0.0f, 0.0f, 0.0f);
		img.SetActive(true);
	}

	public void OnPointerExit() {
		text.color = new Color(1.0f, 1.0f, 1.0f);
		img.SetActive(false);
	}

	public void OnPointerUp() {

	}

	public void OnPointerDown() {

	}

	public void ChangeScene(string sceneName) {
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}

	public void QuitGame() {
		Application.Quit();
	}
}
