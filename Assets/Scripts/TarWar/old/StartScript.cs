using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {
	private GUITexture _black;

	void Start () {
		_black = GetComponent<GUITexture> ();
		_black.enabled = false;
	}

	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			_black.enabled = true;
			Application.LoadLevel ("scene08");
		}
		if (Input.GetKey(KeyCode.Escape)) Application.Quit();
	}
}
