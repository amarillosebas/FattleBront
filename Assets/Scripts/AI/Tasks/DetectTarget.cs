using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DetectTarget : Conditional {
	public SharedGameObject targetGameObject;
	public Shared_LayerMask objectLayerMask;
	public SharedString targetTag;
	public SharedVector3 offset;
	public SharedVector3 targetOffset;
	public SharedVector3 lastKnownPosition;
	public SharedFloat fieldOfViewAngle = 90;
	public SharedFloat viewDistance = 1000;
	public LayerMask ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
	public SharedFloat timeToLoseTarget;

	private Vector3 _noLastKnownPosition = new Vector3 (-999f, -999f, -999f);
	private float _timeToLoseTargetCounter = 0f;
	private bool _losingTarget = false;
	public SharedBool cannotLoseTarget;

	private Vector3 _temporalLastKnownPosition = Vector3.zero;
	public override TaskStatus OnUpdate () {
		GameObject temporalTarget = null;
		CheckTargetStateChange();

		temporalTarget = Look();
		if (temporalTarget == null) temporalTarget = Hear();
		if (temporalTarget == null) {
			if (!_losingTarget) {
				_timeToLoseTargetCounter = Time.time + timeToLoseTarget.Value;
				_losingTarget = true;
			}
			//targetGameObject.Value = null;
			//return TaskStatus.Failure;
		}
		else {
			_losingTarget = false;
			targetGameObject.Value = temporalTarget;
			_temporalLastKnownPosition = temporalTarget.transform.position;
			return TaskStatus.Success;
		}

		if (_losingTarget) {
			if (Time.time > _timeToLoseTargetCounter) {
				if (cannotLoseTarget.Value == false) {
					targetGameObject.Value = null;
					return TaskStatus.Failure;
				}
				else _timeToLoseTargetCounter = Time.time + timeToLoseTarget.Value;
			}
		}

		return TaskStatus.Running;
	}

	GameObject Look () {
		GameObject objectFound = null;

		var hitColliders = Physics.OverlapSphere(transform.position, viewDistance.Value, objectLayerMask.Value);
		if (hitColliders != null) {
			float closerObjectFound = Mathf.Infinity;
			for (int i = 0; i < hitColliders.Length; ++i) {
				float angle;
				GameObject obj =hitColliders[i].gameObject;

				//The target object needs to be within the field of view of the current object
				var direction = obj.transform.TransformPoint(targetOffset.Value) - transform.TransformPoint(offset.Value);

				angle = Vector3.Angle(direction, transform.forward);
				direction.y = 0;//why?

				if (direction.magnitude < viewDistance.Value && angle < fieldOfViewAngle.Value * 0.5f) {
					//The hit agent needs to be within view of the current agent
					RaycastHit hit;
					if (Physics.Linecast(transform.TransformPoint(offset.Value), obj.transform.TransformPoint(targetOffset.Value), out hit, ~ignoreLayerMask)) {
						//Debug.Log("within sight");
						if (hit.transform.tag.Equals(targetTag.Value)) {
							//Calcular distance, y ver cuál de todos está más cerca
							float distance = (transform.position - obj.transform.position).magnitude;
							if (distance < closerObjectFound) {
								closerObjectFound = distance;
								objectFound = obj;
							}
						}
					}
				}
			}
		}

		return objectFound;
	}

	GameObject Hear () {
		GameObject objectFound = null;

		//To-do

		return objectFound;
	}

	private bool _hadTargetLastFrame = false;
	void CheckTargetStateChange () {
		if (targetGameObject.Value != null && !_hadTargetLastFrame) {
			_hadTargetLastFrame = true;
			//Just aquired target
			//Debug.Log("Just aquired target");
			lastKnownPosition.Value = _noLastKnownPosition;
		} else if (targetGameObject.Value == null && _hadTargetLastFrame) {
			_hadTargetLastFrame = false;
			//Just lost target
			//Debug.Log("Just lost target");
			lastKnownPosition.Value = _temporalLastKnownPosition;
		}
	}
}
