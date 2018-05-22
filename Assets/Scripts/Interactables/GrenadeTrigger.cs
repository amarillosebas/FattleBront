using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTrigger : MonoBehaviour {
	public ItemSpawner grenadePouch;
	public float damageInterval;
	private float damageTimeCounter = 0f;

	public int damageAmount;

	void Start () {
		grenadePouch = FindObjectOfType<ItemSpawner>();
	}

	void OnTriggerStay (Collider c) {
		if (c.transform.name == "[VRTK][AUTOGEN][HeadsetColliderContainer]") {
			if (grenadePouch.allowedToGrab = true) grenadePouch.allowedToGrab = false;
		}
	}
	void OnTriggerExit (Collider c) {
		if (c.transform.name == "[VRTK][AUTOGEN][HeadsetColliderContainer]") {
			grenadePouch.allowedToGrab = true;
		}
	}

	void Update () {
		if (Time.time >= damageTimeCounter) {
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.localScale.x * 0.5f);
			foreach (Collider c in hitColliders) {
				//if (c.tag == "Mostro") c.transform.GetComponent<ControllerPlayerHP>().TakeDamage(damageAmount);
			}
			damageTimeCounter = Time.time + damageInterval;
		}
	}
}
