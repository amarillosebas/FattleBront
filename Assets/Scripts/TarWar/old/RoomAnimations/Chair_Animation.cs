using UnityEngine;
using System.Collections;

public class Chair_Animation : MonoBehaviour {
	private Animator _animator;

	void Start() {
		_animator = GetComponent<Animator> ();
	}

	public void animate() {
		_animator.SetTrigger ("triggerMove");
		AudioSource au = GetComponent<AudioSource> ();
		au.PlayDelayed (0.5f);
	}
}
