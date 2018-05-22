using UnityEngine;
using System.Collections;

public class fadeToBlack : MonoBehaviour {
	public float fadeSpeed;
	private GUITexture _gT;

	public bool sceneStarting;
	public bool sceneEnding;

	public Color fadeColor;

	public float timeToEnd;

	void Awake () {
		_gT = GetComponent<GUITexture> ();
		_gT.pixelInset = new Rect (0f, 0f, Screen.width * 100, Screen.height * 100);
	}

	void Update () {
		if (sceneStarting) StartScene ();
		if (sceneEnding) EndScene ();

		if (Input.GetKeyDown (KeyCode.Space)) {
			Application.LoadLevel("scene08");
		}
		if (Input.GetKey(KeyCode.Escape)) Application.Quit();
		if (Input.GetKey(KeyCode.Z)) Application.LoadLevel("sceneStart");
	}

	void FadeIn() {
		_gT.color = Color.Lerp (_gT.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	void FadeOut() {
		_gT.color = Color.Lerp (_gT.color, fadeColor, fadeSpeed * Time.deltaTime);
	}

	public void StartScene() {
		FadeIn ();

		if (_gT.color.a <= 0.001f) {
			_gT.color = Color.clear;
			_gT.enabled = false;
			sceneStarting  = false;
		}
	}

	public void EndScene() {
		_gT.enabled = true;
		FadeOut ();

		if (_gT.color.a >= 0.999f) {
			_gT.color = fadeColor;
			sceneEnding = false;
			Application.LoadLevel("sceneStart");
		}
	}
}
