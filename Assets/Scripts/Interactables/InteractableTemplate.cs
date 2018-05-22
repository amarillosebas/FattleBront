using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class InteractableTemplate : VRTK_InteractableObject {

	public override void StartUsing(VRTK_InteractUse usingObject) {
		base.StartUsing(usingObject);
	}

	public override void StopUsing(VRTK_InteractUse usingObject) {
		base.StopUsing(usingObject);
	}
	
	void Update () {
		base.Update();
	}
}
