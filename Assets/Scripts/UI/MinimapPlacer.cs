using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPlacer : MonoBehaviour {
	public GameObject minimap;

	void Start () {
		GameObject mm = Instantiate(minimap, transform.position, transform.rotation);
		mm.transform.parent = transform;
	}
}
