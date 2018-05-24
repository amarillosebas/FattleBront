using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BehaviorDesigner.Runtime;

public class SoldierTransformManager : MonoBehaviour {
	[Header("Dependencies")]
	public Transform target;
	public AnimatorManager animatorManager;
	public AiSettings ai;

	[Space(5f)]
	public float extraRotationSpeed;

	void Start () {
		
	}
	
	void Update () {
		if (animatorManager.navAgent.enabled) if (animatorManager.navAgent.isStopped) {
			if (ai.GetTarget(out target)) {
				LookAtTarget(target.position);
			}
		} else {
			AddNavAgentRotation();
		}
	}

	void AddNavAgentRotation () {
		Vector3 lookrotation = animatorManager.navAgent.steeringTarget - transform.position;
		if (lookrotation.magnitude > 0.001f) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);
	}
	void LookAtTarget (Vector3 d) {
		Vector3 lookPos = d - transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, ai.statsInfo.rotationSpeed * Time.deltaTime);
	}
}
