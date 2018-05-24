using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

public class AttackTarget : NavMeshMovement {
	public SharedGameObject target;
	public SharedVector3 targetPosition;

	public SharedFloat minAttackDistance;
	public SharedFloat maxAttackDistance;

	public SharedFloat targetTooCloseReactionTime;
	private float _reactionTimeCounter = 0f;

	public SharedBool canAttack;

	public override void OnStart () {
		base.OnStart();
		SetDestination(Target());
	}
	
	public override TaskStatus OnUpdate () {
		if (CanMove() && !IsWithinRange()) {
			SetDestination(Target());
			canAttack.Value = false;
		} else {
			Stop();
			canAttack.Value = true;
		}

		if (target.Value) {
			EntityHP hp = target.Value.GetComponent<EntityHP>();
			if (hp.currentHP <= 0) return TaskStatus.Success;
			else return TaskStatus.Running;
		} else {
			return TaskStatus.Failure;
		}
	}

	bool IsWithinRange () {
		float distance = 0f;
		if (target.Value) distance = (target.Value.transform.position - transform.position).magnitude;
		else return false;

		if (distance <= maxAttackDistance.Value) {
			if (distance >= minAttackDistance.Value) {
				return true;
			} else {
				//modify destination to be away from the target
				_goToTarget = false;
			}
		} else {
			//modify destination to be the target
			_goToTarget = true;
		}
		return false;
	}

	private bool _goToTarget = true;
	private Vector3 Target() {
		if (_goToTarget) {
			if (target.Value != null) {
				return target.Value.transform.position;
			}
		} else {
			//flee behavior
			canAttack.Value = false;
			if (target.Value) return transform.position + (transform.position - target.Value.transform.position).normalized;
			else return transform.position;
		}

		return targetPosition.Value;
	}

	private bool _lastFrameState = false;
	bool CanMove () {
		if (IsWithinRange() != _lastFrameState) {
			_reactionTimeCounter = Time.time + targetTooCloseReactionTime.Value;
		}
		_lastFrameState = IsWithinRange();

		if (Time.time > _reactionTimeCounter) {
			return true;
		} else {
			return false;
		}
	}
}
