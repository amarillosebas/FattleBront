using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapObjectIndicator : MonoBehaviour {
	public float blinkRate = 0.5f;
	private float _timeCounter = 0f;

	//private MotherShipMissionManager _msmm;

	void Start () {
		//_msmm = FindObjectOfType<MotherShipMissionManager>();
		transform.GetChild(0).gameObject.SetActive(false);
	}

	void Update () {
		//if (_msmm.displayObjectiveLocations) {
			Transform p = transform.parent;

			Quaternion newRot = Quaternion.identity;
			newRot.eulerAngles = new Vector3(0f, p.rotation.eulerAngles.y, 0f);
			transform.rotation = newRot;

			if (Time.time >= _timeCounter) {
				Transform c = transform.GetChild(0);
				c.gameObject.SetActive(!c.gameObject.activeSelf);
				_timeCounter = Time.time + blinkRate;
			}
		//}
	}
}
