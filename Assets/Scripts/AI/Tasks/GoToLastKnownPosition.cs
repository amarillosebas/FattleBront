using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

public class GoToLastKnownPosition : NavMeshMovement {
	public SharedVector3 lastKnownPosition;
	public float startTime;

	private Vector3 _noLastKnownPosition = new Vector3 (-999f, -999f, -999f);

	public override void OnStart () {
		base.OnStart();
		_canStart = false;
		StartCoroutine(StartTimer(startTime));
	}

	public override TaskStatus OnUpdate () {
		if (_canStart) {
			if (lastKnownPosition.Value != _noLastKnownPosition) {
				SetDestination(lastKnownPosition.Value);
			}
			else {
				return TaskStatus.Success;
			}

			if (HasArrived()) {
				return TaskStatus.Success;
			}
		}
		return TaskStatus.Running;
	}

	private bool _canStart = false;
	IEnumerator StartTimer (float t) {
		yield return new WaitForSeconds(t);
		_canStart = true;
	}
}
