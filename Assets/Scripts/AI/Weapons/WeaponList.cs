using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponList", menuName = "Game Stats/Weapon List")]
public class WeaponList : ScriptableObject {
	public GameObject[] weapons;
}
