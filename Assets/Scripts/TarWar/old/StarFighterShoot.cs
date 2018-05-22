using UnityEngine;
using System.Collections;

public class StarFighterShoot : MonoBehaviour {
	public enum Factions {
		Rebel,
		Trooper
	}
	public Factions faction;

	public GameObject shot_t;
	public GameObject shot_x;

	private GameObject _shot;
	public float minTime;
	public float maxTime;

	private float _time;

	void Start () {
		if (faction == Factions.Trooper) _shot = shot_t;
		else _shot = shot_x;

		waitToShoot ();
	}

	void waitToShoot() {
		StartCoroutine ("toShoot");
	}
	IEnumerator toShoot() {
		_time = Random.Range (minTime, maxTime);
		yield return new WaitForSeconds(_time);
		GameObject go = GameObject.Instantiate (_shot, transform.position, transform.rotation) as GameObject;
		Destroy (go, 3f);
		waitToShoot ();
	}
}
