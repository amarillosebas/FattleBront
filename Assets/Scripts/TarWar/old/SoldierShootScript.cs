using UnityEngine;
using System.Collections;
using WhiteCat;

public class SoldierShootScript : MonoBehaviour {
	public enum Factions {
		Rebel,
		Trooper
	}
	public Factions faction;
	public GameObject rebelTargets;
	public GameObject trooperTargets;
	private Transform[] _targets;

	private Quaternion _originalRotation;

	//public TweenInterpolator ti;
	private float _speed;

	private float _waitTime = 0;
	public float minTime;
	public float maxTime;

	private Animator _animator;
	private int _numberOfShots;
	public int minShots;
	public int maxShots;

	public GameObject laserShot;
	public Transform muzzle;
	public float bulletLifeSpan;

	private bool _canTurn;
	private bool _canTurnBack;
	private Quaternion _toRotation;
	private Quaternion _prevRotation;
	private int _currentTarget;

	void Start () {
		_animator = GetComponent<Animator> ();
		//_speed = ti.speed;

		if (faction == Factions.Rebel) {
			_targets = new Transform[rebelTargets.transform.childCount];
			for (int i = 0; i < _targets.Length; i++) {
				_targets[i] = rebelTargets.transform.GetChild(i).GetComponent<Transform>();
			}
		} else if (faction == Factions.Trooper) {
			_targets = new Transform[trooperTargets.transform.childCount];
			for (int i = 0; i < _targets.Length; i++) {
				_targets[i] = trooperTargets.transform.GetChild(i).GetComponent<Transform>();
			}
		}

		_waitTime = UnityEngine.Random.Range(minTime, maxTime);
		waitToShoot();
	}

	void FixedUpdate () {
		if (_canTurn) {
			_toRotation = Quaternion.LookRotation(_targets[_currentTarget].position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, _toRotation, 5 * Time.deltaTime);
			if (Quaternion.Angle(transform.rotation, _toRotation) < 1) _canTurn = false;
		}
		if (_canTurnBack) {
			//_toRotation = Quaternion.LookRotation(transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, transform.parent.parent.rotation, 5 * Time.deltaTime);
			if (Quaternion.Angle (transform.rotation, transform.parent.parent.rotation) < 10) {
				_canTurnBack = false;
				transform.rotation = transform.parent.rotation;
			}
		}
	}

	void waitToShoot() {
		StartCoroutine ("toShoot");
	}
	IEnumerator toShoot() {
		_numberOfShots = UnityEngine.Random.Range (minShots, maxShots + 1);
		yield return new WaitForSeconds(_waitTime);
		//ti.speed = 0;
		_originalRotation = transform.rotation;
		_currentTarget = UnityEngine.Random.Range (0, _targets.Length);
		_prevRotation = transform.parent.parent.rotation;

		_canTurn = true;

		//transform.LookAt (_targets [_currentTarget]);
		_animator.SetTrigger ("triggerShoot");
	}

	public void backToRun() {
		if (_numberOfShots <= 1) {
			_canTurnBack = true;
			//transform.rotation = _originalRotation;
			_animator.SetTrigger ("triggerRun");

			_waitTime = UnityEngine.Random.Range (minTime, maxTime);
			waitToShoot ();

			//ti.speed = _speed;
		} else {
			_numberOfShots--;
		}
	}

	public void shootLaser() {
		GameObject go = GameObject.Instantiate(laserShot, muzzle.position, muzzle.rotation) as GameObject;
		GameObject.Destroy(go, bulletLifeSpan);
	}
}
