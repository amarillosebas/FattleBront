using UnityEngine;
using System.Collections;

public class RingtoneScript : MonoBehaviour {
	public bool toStop;
	public bool change;
	private AudioSource _audio;

	public GameObject phone;
	private bool _shown;

	void Start () {
		_audio = GetComponent<AudioSource> ();
	}

	void Update () {
		if (toStop) {
			_audio.volume -= 0.002f;
			if (_audio.volume <= 0) toStop = false;
		}
	}

	void hideShow() {
		_shown = !_shown;
		if (phone) phone.SetActive (_shown);
	}
}
