using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class InteractableFlashlight : VRTK_InteractableObject {
	public Light light;
	public Collider col;

	public override void StartUsing(VRTK_InteractUse usingObject) {
		base.StartUsing(usingObject);
		light.enabled = true;
	}

	public override void StopUsing(VRTK_InteractUse usingObject) {
		base.StopUsing(usingObject);
		light.enabled = false;
	}

	/*public override void Grabbed(VRTK_InteractGrab grabbingObject) {
		base.Ungrabbed(grabbingObject);
		col.isTrigger = true;
	}

	public override void Ungrabbed(VRTK_InteractGrab grabbingObject) {
		base.Ungrabbed(grabbingObject);
		col.isTrigger = false;
	}*/
	
	void Update () {
		base.Update();
	}
}
