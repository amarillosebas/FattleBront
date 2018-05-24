using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public WeaponStats weaponInfo;
	public Transform muzzle;
	public GameObject shotPrefab;
	
	public virtual bool Shoot () {
		bool hasShot = false;
		if (!_onCooldown) {
			GameObject shot = Instantiate(shotPrefab, muzzle.position, muzzle.rotation);
			Destroy(shot, weaponInfo.shotLifeSpan);
			_onCooldown = true;
			shot.GetComponent<ShotBehavior>().movementSpeed = weaponInfo.projectileSpeed;
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
				float randomTime = Random.Range(weaponInfo.fireRate - 0.1f, weaponInfo.fireRate + 0.1f);
				_shootingTimer = Time.time + randomTime;
			}
		}
	}
}
