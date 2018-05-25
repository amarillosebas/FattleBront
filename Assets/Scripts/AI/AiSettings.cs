using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class AiSettings : MonoBehaviour {
	[Header("Dependencies")]
	public BehaviorTree behaviorTree;
	public SoldierStats statsInfo;

	[Space(5f)]
	public Shared_LayerMask targetLayers;

	void Start () {
		switch (transform.tag) {
			case "Trooper": targetTag = "Rebel"; break;
			case "Rebel": targetTag = "Trooper"; break;
		}
		SetSharedVariables();
	}

	string targetTag = "";
	void SetSharedVariables () {
		behaviorTree.SetVariableValue("movementSpeed", statsInfo.movementSpeed);
		behaviorTree.SetVariableValue("rotationSpeed", statsInfo.rotationSpeed);
		behaviorTree.SetVariableValue("angularSpeed", statsInfo.rotationSpeed * 100f);
		behaviorTree.SetVariableValue("minWanderDistance", statsInfo.minWanderDistance);
		behaviorTree.SetVariableValue("maxWanderDistance", statsInfo.maxWanderDistance);
		behaviorTree.SetVariableValue("minWanderPause", statsInfo.minWanderPause);
		behaviorTree.SetVariableValue("maxWanderPause", statsInfo.maxWanderPause);
		behaviorTree.SetVariableValue("wanderRate", statsInfo.wanderRate);
		behaviorTree.SetVariableValue("targetLayers", targetLayers);
		behaviorTree.SetVariableValue("targetTag", targetTag);
		behaviorTree.SetVariableValue("targetTooCloseReactionTime", statsInfo.targetTooCloseReactionTime);
	}
	
	public bool GetTarget (out Transform t) {
		SharedGameObject sgo = behaviorTree.GetVariable("target") as SharedGameObject;

		if (sgo.Value != null) {
			t = sgo.Value.transform;
			return true;
		} else {
			t = null;
			return false;
		}
	}
}
