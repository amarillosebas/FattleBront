using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerSkinCommunicator : MonoBehaviour {
	public Rigidbody rb;
	public Transform cam;
	public Animator cameraAnimator;

	public bool updateRotation = true;
	//public Transform minimapDummy;
	public PlayerHP hpScript;
	public VRTK_ControllerEvents[] controllers;

	void Start () {
		hpScript.cameraAnimator = cameraAnimator;
		hpScript.controllers = controllers;
	}

	void Update () {
		if (updateRotation) {
			Quaternion newRot = Quaternion.identity;
			newRot.eulerAngles = new Vector3(0f, cam.rotation.eulerAngles.y, 0f);
			transform.rotation = newRot;
		}
	}
}
