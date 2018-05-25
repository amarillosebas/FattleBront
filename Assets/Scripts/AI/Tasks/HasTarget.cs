using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class HasTarget : Conditional {
	public SharedGameObject targetGameObject;
	
	public override TaskStatus OnUpdate () {
		if (targetGameObject.Value) return TaskStatus.Success;
		else return TaskStatus.Running;
	}
}
