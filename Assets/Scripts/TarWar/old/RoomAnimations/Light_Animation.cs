using UnityEngine;
using System.Collections;

public class Light_Animation : MonoBehaviour {
	private Animator _animator;
	private Light _light;

	void Start () {
		_animator = GetComponent<Animator> ();
		_light = GetComponentInChildren<Light> ();
	}

	public void animate() {
		_animator.SetTrigger ("triggerTurn");
	}

	public void turnOff() {
		_light.enabled = false;
	}
	public void turnOn() {
		_light.enabled = true;
	}

	public void playSound() {
		AudioSource au = GetComponent<AudioSource> ();
		au.Play ();
	}
}
