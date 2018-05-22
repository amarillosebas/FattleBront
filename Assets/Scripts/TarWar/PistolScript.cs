using UnityEngine;
using System.Collections;

public class PistolScript : MonoBehaviour {
	public GameObject laserShot;
	public Transform muzzle;
	public GameObject particle;
	public float shotDistance;

	void Start () {
	
	}

	public void Shoot () {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, shotDistance)) {
			Quaternion rot = Quaternion.identity;
			rot.eulerAngles = new Vector3 (-90f, 0f, 0f);
			GameObject p = Instantiate (particle, hit.point, rot) as GameObject;
			Destroy(p, 1f);
		}
		Instantiate (laserShot, muzzle.position, muzzle.rotation);
	}
}
