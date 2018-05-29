using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoldierStats", menuName = "Game Stats/Soldier Stats")]
public class SoldierStats : ScriptableObject {
	[Space(5f)]
	[Header("Movement")]
	public float movementSpeed;
	public float rotationSpeed;

	[Space(5f)]
	[Header("Wander Behavior")]
	public float minWanderDistance;
	public float maxWanderDistance;
	public float minWanderPause;
	public float maxWanderPause;
	public float wanderRate;

	[Space(5f)]
	[Header("Attack Behavior")]
	public float targetTooCloseReactionTime;
	public float minAttackMoveTime;
	public float maxAttackMoveTime;
	public float minAttackMoveDistance;
	public float maxAttackMoveDistance;
}
