using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class StartShooting : MonoBehaviour {
	private StarFighterShoot _laser;
	private AudioSource _audio;
	public float minPitch;
	public float maxPitch;

	void Start() {
		_laser = gameObject.transform.parent.GetComponentInChildren<StarFighterShoot> ();
		_laser.enabled = false;
	}
	void startToShoot() {
		_laser.enabled = true;
	}
	void sound() {
		_audio = GetComponent<AudioSource> ();
		_audio.pitch = Random.Range (minPitch, maxPitch);
		_audio.Play ();
	}
}
