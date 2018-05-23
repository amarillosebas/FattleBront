using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class SoldierTransformManager : MonoBehaviour {
	public BehaviorTree behaviorTree;

	public SoldierStats statsInfo;

	public Transform target;
	public bool targetSighted;

	void Start () {
		behaviorTree.SetVariableValue("movementSpeed", statsInfo.movementSpeed);
		behaviorTree.SetVariableValue("rotationSpeed", statsInfo.rotationSpeed);
		behaviorTree.SetVariableValue("minWanderDistance", statsInfo.minWanderDistance);
		behaviorTree.SetVariableValue("maxWanderDistance", statsInfo.maxWanderDistance);
		behaviorTree.SetVariableValue("minWanderPause", statsInfo.minWanderPause);
		behaviorTree.SetVariableValue("maxWanderPause", statsInfo.maxWanderPause);
		behaviorTree.SetVariableValue("wanderRate", statsInfo.wanderRate);
	}
	
	void Update () {
		if (targetSighted) {
			if (target) {
				Quaternion newRotation =  Quaternion.RotateTowards (
					transform.rotation,
					Quaternion.LookRotation(target.position - transform.position),
					Time.deltaTime * statsInfo.rotationSpeed
				);

				newRotation.eulerAngles = new Vector3 (
					0f, -newRotation.eulerAngles.y, 0f
				);

				transform.rotation = newRotation;
			}
		}
	}
}
