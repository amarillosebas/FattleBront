using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshToAnimator : MonoBehaviour {
	public Animator soldierAnimator;
	public NavMeshAgent navAgent;
	//public Rigidbody soldierRigidbody;

	public enum MoveState {
		Idle,
		Walk
	}
	public MoveState moveState;

	void Start () {
		
	}
	
	void Update () {
		if (navAgent.hasPath) {
			moveState = MoveState.Walk;
		} else {
			moveState = MoveState.Idle;
		}

		soldierAnimator.SetFloat("Speed", navAgent.velocity.magnitude);
	}

	/*float GetAngularVelocity () {
		Vector3 s = navAgent.transform.InverseTransformDirection(navAgent.velocity).normalized;
		float t = s.x;
		return t;
	}*/
}
