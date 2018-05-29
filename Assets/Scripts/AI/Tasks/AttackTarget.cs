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
	public SharedBool cannotLoseTarget;

	private bool _pauseAttackToMove = false;
	private bool _canAttackMove = false;
	public SharedFloat minAttackMovetime;
	public SharedFloat maxAttackMovetime;

	public SharedFloat minAttackMoveDistance = 0.5f;
	public SharedFloat maxAttackMoveDistance = 1.5f;
	public SharedFloat wanderRate = 2;

	public override void OnStart () {
		base.OnStart();
		SetDestination(Target());

		if (minAttackMovetime.Value > 0f && maxAttackMovetime.Value > 0f) {
			_canAttackMove = true;
			_moveToRandomPointCounter = Time.time + Random.Range(minAttackMovetime.Value, maxAttackMovetime.Value);
		}
	}
	
	public override TaskStatus OnUpdate () {
		CanMoveToRandomPoint();

		if (!_pauseAttackToMove) {
			if (CanCatchUpWithPlayer() && !IsWithinRange()) {
				SetDestination(Target());
				canAttack.Value = false;
			} else {
				Stop();
				canAttack.Value = true;
			}
		} else {
			MoveToRandomPoint();
			if (HasArrived()) {
				_movingToRandomPoint = false;
				_pauseAttackToMove = false;
				canAttack.Value = true;
			}
		}

		if (target.Value) {
			EntityHP hp = target.Value.GetComponent<EntityHP>();
			if (hp) {
				if (hp.currentHP <= 0) return TaskStatus.Success;
				else return TaskStatus.Running;
			}
			else return TaskStatus.Running;
		} else {
			return TaskStatus.Failure;
		}
	}

	private bool IsWithinRange () {
		float distance = 0f;
		if (target.Value) distance = (target.Value.transform.position - transform.position).magnitude;
		else return false;

		if (distance <= maxAttackDistance.Value) {
			if (distance >= minAttackDistance.Value) {
				return true;
			} else {
				//modify destination to be away from the target
				_goToTarget = false;
				cannotLoseTarget.Value = true;
			}
		} else {
			//modify destination to be the target
			_goToTarget = true;
			cannotLoseTarget.Value = false;
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
	bool CanCatchUpWithPlayer () {
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

	//private float _moveToRandomPointTime = 0f;
	private float _moveToRandomPointCounter = 0f;
	private void CanMoveToRandomPoint () {
		if (Time.time > _moveToRandomPointCounter) {
			canAttack.Value = false;
			_pauseAttackToMove = true;

			//_moveToRandomPointTime = Random.Range(minAttackMovetime, maxAttackMovetime);
			_moveToRandomPointCounter = Time.time + Random.Range(minAttackMovetime.Value, maxAttackMovetime.Value);
		}
	}

	private bool _movingToRandomPoint = false;
	private void MoveToRandomPoint () {
		if (!_movingToRandomPoint) {
			var direction = transform.forward;
			var validDestination = false;
			var attempts = 5;
			var destination = transform.position;

			while (!validDestination && attempts > 0) {
				direction = direction + Random.insideUnitSphere * wanderRate.Value;
				destination = transform.position + direction.normalized * Random.Range(minAttackMoveDistance.Value, maxAttackMoveDistance.Value);
				validDestination = SamplePosition(destination);
				attempts--;
			}

			if (validDestination) {
				SetDestination(destination);
			}
		}
		_movingToRandomPoint = true;
	}
}
