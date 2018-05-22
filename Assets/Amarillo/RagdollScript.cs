using UnityEngine;
using System.Collections;

public class RagdollScript : MonoBehaviour {
	public Rigidbody center;
	public bool stand = true;
	public float force = 100f;

	public Rigidbody[] animatedLimbs;
	public bool pairs;
	public bool randomizeNotPairs;

	public bool canTwitch;
	public float torqueApplied = 7500000f;
	public float torqueRange = 500000f;
	private float _minT;
	private float _maxT;

	private int _counter = 0;
	public int frames = 20;
	private float t = 0;
	private int direction = 1;

	public float factor = 1f;

	void Start () {
		_minT = torqueApplied - torqueRange;
		_maxT = torqueApplied + torqueRange;
	}
	
	void FixedUpdate () {
		if (stand) center.AddForce(Vector3.up * force);

		if (canTwitch) {
			AnimateLimbs();
		}
	}

	void AnimateLimbs () {
		if (_counter >= frames) {
			direction = Random.Range(0,2);
			if (direction == 0) direction = -1;
			t = Random.Range(_minT, _maxT);
			_counter = 0;
		} else {
			_counter++;
		}

		if (pairs) {
			for (int i = 0; i < animatedLimbs.Length; i++) {
				if (i % 2 == 0)	animatedLimbs[i].AddTorque(animatedLimbs[i].transform.GetComponent<CharacterJoint>().axis * t * factor * direction, ForceMode.Force);
				else 			animatedLimbs[i].AddTorque(animatedLimbs[i].transform.GetComponent<CharacterJoint>().axis * t * factor * -direction, ForceMode.Force);
			}
		} else {
			if (randomizeNotPairs) {
				for (int i = 0; i < animatedLimbs.Length; i++) {
					direction = Random.Range(0,2);
					if (direction == 0) direction = -1;
					animatedLimbs[i].AddTorque(animatedLimbs[i].transform.GetComponent<CharacterJoint>().axis * t * factor * direction, ForceMode.Force);
				}
			} else {
				for (int i = 0; i < animatedLimbs.Length; i++) {
					animatedLimbs[i].AddTorque(animatedLimbs[i].transform.GetComponent<CharacterJoint>().axis * t * factor * direction, ForceMode.Force);
				}
			}
			
		}
	}
}
