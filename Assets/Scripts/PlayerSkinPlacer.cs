using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerSkinPlacer : MonoBehaviour {
	public GameObject skin;
	private Vector3 _skinPosition;
	private GameObject _mySkin;
	private PlayerSkinCommunicator _skinCommunicator;
	public Animator cameraAnimator;
	public VRTK_ControllerEvents[] controllers;

	void Start () {
		_mySkin = Instantiate(skin, transform.position, transform.rotation);
		_skinCommunicator = _mySkin.GetComponent<PlayerSkinCommunicator>();
		_skinCommunicator.rb = transform.parent.gameObject.GetComponent<Rigidbody>();
		_skinCommunicator.cam = transform;
		_skinCommunicator.cameraAnimator = cameraAnimator;
		_skinCommunicator.controllers = controllers;
	}
	
	void FixedUpdate () {
		_skinPosition = new Vector3 (transform.position.x, 0f, transform.position.z);
		_mySkin.transform.position = _skinPosition;
	}
}
