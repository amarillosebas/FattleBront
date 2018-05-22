using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPlayerSpawnPosition : MonoBehaviour {
	public Transform playerParent;
	public Transform vrCamera;

	IEnumerator Start () {
		yield return null;
		yield return null;
		Vector3 d = playerParent.position - vrCamera.position;
		d.y = 0f;
		playerParent.position += d;
		//vrCamera.rotation = playerParent.rotation;
	}
}
