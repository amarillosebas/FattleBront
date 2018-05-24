using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAimIK : MonoBehaviour {
	[Header("Dependencies")]
	public AnimatorManager animatorManager;
	public AiSettings ai;
	
	[Space(5f)]
	public bool ikActive = false;
	public Transform target;
	public float heightCompensation = 1f;

	void OnAnimatorIK () {
		if (ikActive) {
			if (ai.GetTarget(out target)) {
				animatorManager.soldierAnimator.SetLookAtWeight(1f, 1f, 1f, 1f, 1f);
				Vector3 lookAtPosition = target.position;
				lookAtPosition.y += heightCompensation;
				animatorManager.soldierAnimator.SetLookAtPosition(lookAtPosition);
			}
		}
	}
}
