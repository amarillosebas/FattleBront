using UnityEngine;
using System.Collections;

public class GamepadPlayer2 : MonoBehaviour {
	private Rigidbody _rb;
	public float speed;
	private SMGScript _ps;
	private int _rotation = 0;
	public float rotationSpeed;

	void Start () {
		_rb = GetComponent<Rigidbody>();
		_ps = GetComponent<SMGScript>();
	}
	
	void FixedUpdate () {
		Vector2 vel = new Vector2 (Input.GetAxis("2_horizontal"), Input.GetAxis("2_vertical"));
		_rb.MovePosition(Utils.CalculateMovement(transform, vel, speed));
		_rb.velocity = Vector3.zero;

		if (Input.GetAxis("2_shoot") > 0.75f) {
			_ps.TriggerOn();
		} else {
			_ps.TriggerOff();
		}

		if (Input.GetAxis("2_lookx") > 0.5f) {
			_rotation = 1;
		} else if (Input.GetAxis("2_lookx") < -0.5f) {
			_rotation = -1;
		} else {
			_rotation = 0;
		}
		if (_rotation != 0) {
			_rb.AddTorque(Vector3.up * _rotation * rotationSpeed);
		} else {
			_rb.angularVelocity=Vector3.zero;
		}
	}
}
