using UnityEngine;
using System.Collections;

public class ShotBehaviorReflected : MonoBehaviour {
	public float speed;

	public GameObject explosion;

	public float collisionDistance;
	private bool _isHitting = false;
	private bool _canEnd = false;

	void Start () {
		Destroy (gameObject, 3f);
	}
	
	void Update () {
		transform.position += transform.forward * Time.deltaTime * speed;

		if (Physics.Raycast(transform.position + transform.forward, transform.forward, collisionDistance)) {
			_isHitting = true;
		} else {
			if (_isHitting) _canEnd = true;
		}
		
		if (_canEnd) { 
			Quaternion rot = Quaternion.identity;
			rot.eulerAngles = new Vector3 (-90f, 0f, 0f);
			GameObject p = Instantiate (explosion, transform.position, rot) as GameObject;
			Destroy(p, 1f);

			_canEnd = false;
			_isHitting = false;

			Destroy (gameObject);
		}
		Debug.DrawRay(transform.position + transform.forward, transform.forward * collisionDistance, Color.blue, 0f, true);
	}
}
