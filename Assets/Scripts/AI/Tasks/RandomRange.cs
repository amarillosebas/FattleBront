using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class RandomRange : Conditional {
	public SharedFloat randomFloat;
	public SharedFloat minValue;
	public SharedFloat maxValue;

	void OnStart () {
		randomFloat.Value = Random.Range(minValue.Value, maxValue.Value);
	}
}
