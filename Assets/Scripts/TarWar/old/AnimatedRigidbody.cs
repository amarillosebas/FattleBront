using UnityEngine;
using System.Collections;

public class AnimatedRigidbody : MonoBehaviour {
	private Rigidbody _rb;
	private Animator _animator;

	public float min;
	public float max;

	private float _tx;
	private float _ty;
	private float _tz;

	void Start () {
		_rb = GetComponent<Rigidbody> ();
		_animator = GetComponent<Animator> ();
		_animator.speed = 0;
		_animator.enabled = false;

		_tx = Random.Range (min, max);
		_ty = Random.Range (min, max);
		_tz = Random.Range (min, max);
	}

	public void deActivateRigidbody() {
		_animator.enabled = true;
		_rb.isKinematic = true;
		_animator.speed = Random.Range(0.75f, 1.25f);
	}

	public void activateRigidbody() {
		_rb = GetComponent<Rigidbody> ();
		_rb.isKinematic = false;
		_animator.enabled = false;
	}

	public void jump(float f) {
		_rb.AddForce (Vector3.up * f);
		_rb.AddTorque (_tx, _ty, _tz);
	}
}
