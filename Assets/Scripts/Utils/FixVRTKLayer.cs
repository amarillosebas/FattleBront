using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixVRTKLayer : MonoBehaviour {
	public string layerName;
	public GameObject guiltyChild;
	private Transform[] theChildren;

	public bool fixHitboxes = false;

	void Start () {
		if (fixHitboxes) {
			StartCoroutine(FixHitBoxes());
		} else {
			StartCoroutine(FixLayerTimer());
			theChildren = GetComponentsInChildren<Transform>();
		}
	}

	IEnumerator FixLayerTimer () {
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		guiltyChild = transform.GetChild(3).gameObject;
		guiltyChild.layer = LayerMask.NameToLayer(layerName);
		foreach (Transform c in theChildren) {
			c.gameObject.layer = LayerMask.NameToLayer(layerName);
		}
	}

	IEnumerator FixHitBoxes () {
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		transform.GetChild(3).gameObject.layer = LayerMask.NameToLayer(layerName);
		transform.GetChild(3).GetChild(0).gameObject.layer = LayerMask.NameToLayer(layerName);
	}
}
