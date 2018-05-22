using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ItemSpawner : MonoBehaviour {
	public GameObject itemPrefab;
	public bool allowedToGrab = true;

	public PlayerItems items;

	void Start () {
		StartCoroutine(WaitForPlayer());
	}

	private void OnTriggerStay(Collider collider) {
		if (allowedToGrab) {
			VRTK_InteractGrab grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>() ? collider.gameObject.GetComponent<VRTK_InteractGrab>() : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());
			if (CanGrab(grabbingController) && items.grenadeAmmo > 0) {
				GameObject newItem = Instantiate(itemPrefab);
				items.grenadeAmmo--;
				grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(newItem);
				grabbingController.AttemptGrab();
			}
		}
	}

	private bool CanGrab(VRTK_InteractGrab grabbingController) {
		return (grabbingController && grabbingController.GetGrabbedObject() == null && grabbingController.IsGrabButtonPressed());

	}

	IEnumerator WaitForPlayer () {
		yield return new WaitForSeconds(0.5f);
		items = FindObjectOfType<PlayerItems>();
	}
}
