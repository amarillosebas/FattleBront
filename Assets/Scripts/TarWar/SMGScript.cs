using UnityEngine;
using System.Collections;

public class SMGScript : MonoBehaviour {
	public GameObject laserShot;
	public Transform muzzle;
	public GameObject particle;
	public float shotDistance;

	public float rateOfFire;
	private bool _canShoot;
	private float _lastShot = 0f;

	private bool _trigger;

	void Start () {
	
	}

	void Update () {
		if (_canShoot) {
			_canShoot = false;

			if (_trigger) {
				RaycastHit hit;
				if (Physics.Raycast(transform.position, transform.forward, out hit, shotDistance)) {
					Quaternion rot = Quaternion.identity;
					rot.eulerAngles = new Vector3 (-90f, 0f, 0f);
					GameObject p = Instantiate (particle, hit.point, rot) as GameObject;
					Destroy(p, 1f);
				}
				Instantiate (laserShot, muzzle.position, muzzle.rotation);
			}

			_lastShot = Time.time;
		}
		if (rateOfFire + _lastShot < Time.time) _canShoot = true;
	}

	public void TriggerOn () {
		_canShoot = true;
		_trigger = true;
	}
	public void TriggerOff () {
		_trigger = false;
		_lastShot = Time.time;
	}
}
