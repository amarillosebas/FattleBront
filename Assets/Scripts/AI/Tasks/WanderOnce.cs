using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

public class WanderOnce : NavMeshMovement {
	public SharedFloat minWanderDistance = 20;
	public SharedFloat maxWanderDistance = 20;
	public SharedFloat wanderRate = 2;
	public SharedInt targetRetries = 1;

	public override void OnStart () {
		base.OnStart();
		SetTarget();
	}

	public override TaskStatus OnUpdate () {
		if (HasArrived()) {
			return TaskStatus.Success;
		}
		return TaskStatus.Running;
	}

	private void SetTarget () {
		var direction = transform.forward;
		var validDestination = false;
		var attempts = targetRetries.Value;
		var destination = transform.position;

		while (!validDestination && attempts > 0) {
			direction = direction + Random.insideUnitSphere * wanderRate.Value;
			destination = transform.position + direction.normalized * Random.Range(minWanderDistance.Value, maxWanderDistance.Value);
			validDestination = SamplePosition(destination);
			attempts--;
		}

		if (validDestination) {
			SetDestination(destination);
		}
	}
}
