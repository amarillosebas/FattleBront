using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Game Stats/Weapon Stats")]
public class WeaponStats : ScriptableObject {
	public new string name;

	[Space(5f)]
	[Header("Basic Stats")]
	public int damage;
	public float fireRate;
	public float accuracy;
	public float recoil;
	public AudioClip[] shotSFX;

	[Space(5f)]
	[Header("Projectile/shot stats")]
	public float projectileSpeed;
	public float projectileForce;
	public float shotLifeSpan;
	public float collisionDistance;

	[Space(5f)]
	[Header("Behavior stats")]
	public float minAttackDistance;
	public float maxAttackDistance;
}
