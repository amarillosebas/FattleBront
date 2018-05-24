using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Game Stats/Weapon Stats")]
public class WeaponStats : ScriptableObject {
	public new string name;

	[Space(5f)]
	public int damage;
	public float fireRate;
	public float shotLifeSpan;
	public float projectileSpeed;
	public float projectileForce;

	[Space(5f)]
	public float minAttackDistance;
	public float maxAttackDistance;
}
