using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerImpactEnforcer : MonoBehaviour {
	private CharacterController _controller;

	public float mass = 3.0f;

	Vector3 impact = Vector3.zero;

	void Start () {
		_controller = GetComponent<CharacterController>();
	}

	public void AddImpact (Vector3 dir, float force) {
		dir.Normalize();
		if (dir.y < 0) dir.y = -dir.y;
		impact += dir.normalized * force / mass;
	}
	
	void Update () {
		if (impact.magnitude > 0.2f) _controller.Move(impact * Time.deltaTime);
		impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
	}
}
