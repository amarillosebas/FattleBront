using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class InteractableGrenade : VRTK_InteractableObject {
	public float timeToActivate;
	public float activeTime;

	public Rigidbody rb;
	public Animator anim;

	public override void Ungrabbed(VRTK_InteractGrab grabbingObject) {
		base.Ungrabbed(grabbingObject);
		StartCoroutine(ActivateGrenade(timeToActivate));
	}
	
	void Update () {
		base.Update();
	}

	IEnumerator ActivateGrenade (float t) {
		yield return new WaitForSeconds(t);
		isGrabbable = false;
		//rb.isKinematic = true;
		Destroy(rb);
		anim.SetTrigger("Start");
		StartCoroutine(DeactivateGrenade(activeTime));
	}
	IEnumerator DeactivateGrenade (float t) {
		yield return new WaitForSeconds(t);
		anim.SetTrigger("End");
		StartCoroutine(DestroyTimer(1f));
	}
	IEnumerator DestroyTimer (float t) {
		yield return new WaitForSeconds(t);
		Destroy(gameObject);
	}
}
