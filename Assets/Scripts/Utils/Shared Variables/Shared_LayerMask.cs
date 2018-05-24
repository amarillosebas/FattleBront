using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

[System.Serializable]
public class Shared_LayerMask : SharedVariable <LayerMask> {
	public static implicit operator Shared_LayerMask(LayerMask value) { return new Shared_LayerMask { Value = value }; }
}
