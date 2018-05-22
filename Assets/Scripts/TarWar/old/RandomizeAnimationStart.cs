using UnityEngine;
using System.Collections;

public class RandomizeAnimationStart : MonoBehaviour {
	private Animator _animator;
	//private WhiteCat.TweenInterpolator _ti;
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator> ();
		if (_animator) _animator.ForceStateNormalizedTime(UnityEngine.Random.Range(0.0f, 1.0f));

		//_ti = GetComponent<WhiteCat.TweenInterpolator> ();
		//if (_ti)_ti.normalizedTime = UnityEngine.Random.Range(0.0f, 1.0f);
	}
}
