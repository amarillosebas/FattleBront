using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public WeaponStats weaponInfo;
	public Transform nozzle;
	public GameObject shotPrefab;

	public float nozzleLookAtAngle;
	
	public virtual bool Shoot (Transform t) {
		bool hasShot = false;
		if (!_onCooldown) {
			SetRandomShootingTime();
			RecoilCalculation(t);
			GameObject shot = Instantiate(shotPrefab, nozzle.position, nozzle.rotation);
			Destroy(shot, weaponInfo.shotLifeSpan);
			_onCooldown = true;
			ShotBehavior sb = shot.GetComponent<ShotBehavior>();
			sb.movementSpeed = weaponInfo.projectileSpeed;
			sb.collisionDistance = weaponInfo.collisionDistance;
			sb.damage = weaponInfo.damage;
			hasShot = true;
		}
		return hasShot;
	}

	private bool _onCooldown = false;
	private float _shootingTimer = 0f;
	void Update () {
		if (_onCooldown) {
			if (Time.time > _shootingTimer) {
				_onCooldown = false;
				SetRandomShootingTime();
			}
		}
	}

	void RecoilCalculation (Transform t) {
		Vector3 heightCompensatedT = t.position;
		heightCompensatedT.y += 1f;
		Vector3 look = heightCompensatedT - transform.position;
		Quaternion q = Quaternion.LookRotation(look);

		nozzle.rotation = q;
		//if (Quaternion.Angle (q, transform.rotation) <= nozzleLookAtAngle) nozzle.rotation = q;
		//else nozzle.rotation = transform.rotation;

		float rX = Random.Range(-weaponInfo.accuracy, weaponInfo.accuracy);
		float rY = Random.Range(-weaponInfo.accuracy, weaponInfo.accuracy);
		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (nozzle.rotation.eulerAngles.x + rX, nozzle.rotation.eulerAngles.y + rY/* - 90f*/, nozzle.rotation.eulerAngles.z);
		nozzle.rotation = rot;
	}

	void SetRandomShootingTime () {
		float randomTime = Random.Range(weaponInfo.fireRate - 0.1f, weaponInfo.fireRate + 0.1f);
		_shootingTimer = Time.time + randomTime;
	}
}
