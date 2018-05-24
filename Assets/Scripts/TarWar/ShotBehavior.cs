using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {
	public float movementSpeed;

	void Start () {
	
	}
	
	void Update () {
		transform.position += transform.forward * Time.deltaTime * movementSpeed;
	
	}
}
