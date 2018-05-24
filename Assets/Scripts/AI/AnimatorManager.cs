using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatorManager : MonoBehaviour {
	public Animator soldierAnimator;
	public NavMeshAgent navAgent;

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

	public void Shoot () {
		soldierAnimator.SetTrigger("Shoot");
	}

	/*float GetAngularVelocity () {
		Vector3 s = navAgent.transform.InverseTransformDirection(navAgent.velocity).normalized;
		float t = s.x;
		return t;
	}*/
}
