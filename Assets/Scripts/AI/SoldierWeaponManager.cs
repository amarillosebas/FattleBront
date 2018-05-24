using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;

public class SoldierWeaponManager : MonoBehaviour {
	public AiSettings ai;
	public AnimatorManager animatorManager;

	[Space(5f)]
	public WeaponList weaponList;
	public int startingWeapon;
	public Transform gunPivot;
	public Weapon equipedWeapon;

	private SharedBool _canAttack;

	void Start () {
		SpawnWeapon(startingWeapon);
		_canAttack = (SharedBool) ai.behaviorTree.GetVariable("canAttack");
	}

	void Update () {
		if (_canAttack.Value == true) {
			Shoot();
		}
	}

	public void Shoot() {
		if (equipedWeapon.Shoot()) {
			animatorManager.Shoot();
		}
	}

	void SpawnWeapon (int i) {
		if (equipedWeapon) Destroy(equipedWeapon.gameObject);
		GameObject w = Instantiate(weaponList.weapons[i], gunPivot.position, gunPivot.rotation);
		w.transform.parent = gunPivot;
		equipedWeapon = w.GetComponent<Weapon>();

		ai.behaviorTree.SetVariableValue("minAttackDistance", equipedWeapon.weaponInfo.minAttackDistance);
		ai.behaviorTree.SetVariableValue("maxAttackDistance", equipedWeapon.weaponInfo.maxAttackDistance);
	}
}
