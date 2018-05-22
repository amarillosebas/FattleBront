using UnityEngine;
using System.Collections;

public class Closet_Animation : MonoBehaviour {
	private Animator _animator;

	void Start () {
		_animator = GetComponent<Animator> ();
		_animator.speed = 0;
	}

	public void animate() {
		_animator.speed = 1;
		AudioSource au = GetComponent<AudioSource> ();
		au.Play ();
	}
}
