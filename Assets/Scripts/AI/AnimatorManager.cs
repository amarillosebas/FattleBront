using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatorManager : MonoBehaviour {
	[Header("Dependencies")]
	public Animator soldierAnimator;
	public NavMeshAgent navAgent;
	public AiSettings ai;

	//[Space(5f)]
	public enum MoveState {
		Idle,
		Walk
	}
	public MoveState moveState;
	
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

	public void Die () {
		ai.behaviorTree.enabled = false;
		navAgent.enabled = false;
		Collider c = GetComponent<Collider>();
		c.enabled = false;
	}
}
