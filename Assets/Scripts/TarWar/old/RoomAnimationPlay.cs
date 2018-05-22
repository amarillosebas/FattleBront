using UnityEngine;
using System.Collections;

public class RoomAnimationPlay : MonoBehaviour {
	private Animator _animator;

	void Start () {
		_animator = GetComponent<Animator> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			_animator.SetTrigger("OpenTrigger");
		}
	}
}
