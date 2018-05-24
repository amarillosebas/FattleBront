using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;

public class SoldierWeaponManager : MonoBehaviour {
	[Header("Dependencies")]
	public WeaponList weaponList;
	public AiSettings ai;
	public AnimatorManager animatorManager;
	public SoldierAudioManager audioManager;

	[Space(5f)]
	public int startingWeapon;
	public Transform gunPivot;
	public Weapon equipedWeapon;
	public float waitToAttackTime = 0.5f;

	private SharedBool _canAttack;

	private Transform _target;

	void Start () {
		SpawnWeapon(startingWeapon);
		_canAttack = (SharedBool) ai.behaviorTree.GetVariable("canAttack");
	}

	private bool _lastFrameAttacked = false;
	private float _toAttackTimeCounter = 0f;
	void Update () {
		if (_canAttack.Value == true) {
			if (!_lastFrameAttacked) {
				_toAttackTimeCounter = Time.time + waitToAttackTime;
				_lastFrameAttacked = true;
			}
			if (Time.time > _toAttackTimeCounter) {
				Shoot();
			}
		} else {
			_lastFrameAttacked = false;
		}
	}

	public void Shoot() {
		if (ai.GetTarget(out _target) && equipedWeapon.Shoot(_target)) {
			animatorManager.Shoot();
			audioManager.Shoot(equipedWeapon.weaponInfo.shotSFX);
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
