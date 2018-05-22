using UnityEngine;
using System.Collections;

public class Radio_Animations : MonoBehaviour {
	public TextMesh text;
	private bool _stop = false;
	public bool change;

	private bool _crazy;

	private float _signal;

	private AudioSource _audio;
	public AudioSource au2;

	void Start () {
		_signal = 85.3f;
		_audio = GetComponent<AudioSource> ();
	}

	void Update () {
		if (change) {
			_crazy = true;

			au2.Play();
			au2.volume += 0.005f;
			_audio.volume -= 0.005f;
			
			if (au2.volume >= 0.5f || _audio.volume <= 0) {
				change = false;
				au2.volume = 0.5f;
				_audio.volume = 0f;
				_audio.Stop();
			}
		}
		if (_stop) {
			au2.volume -= 0.001f;
			if (au2.volume <= 0) _stop = false;
		}
		if (_crazy) {
			_signal += 0.1f;
			if (_signal >= 108) _signal = 80;
		}
		text.text = _signal.ToString("F1") + " FM";
	}

	/*public void goCrazy() {
		_audio = GetComponent<AudioSource> ();
		_audio.Stop ();
		_goCrazy = true;
		_audio.clip = _crazyClip;
		_audio.Play ();
	}*/

	public void toStop() {
		_stop = true;
	}
}
