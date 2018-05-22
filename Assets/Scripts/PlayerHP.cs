using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VRTK;

public class PlayerHP : MonoBehaviour {
	public int currentHP = 100;
	private PlayerSkinAudioManager _playerAudio;

	public Slider sldHP;

	public Animator cameraAnimator;
	public VRTK_ControllerEvents[] controllers;

	public Transform hpCanvas;

	void Start () {
		_playerAudio = GetComponent<PlayerSkinAudioManager>();
	}

	void Update () {
		if (sldHP) sldHP.value = currentHP;
		if (hpCanvas) hpCanvas.rotation = Quaternion.identity;
	}

	public void TakeDamage (int d) {
		currentHP -= d;
		if (currentHP <= 0) {
			Die();
		} else {
			_playerAudio.Hurt();
			if (cameraAnimator) cameraAnimator.SetTrigger("HurtPlayer");
		}
	}

	void Die () {
		_playerAudio.Die();
		if (cameraAnimator) cameraAnimator.SetTrigger("KillPlayer");
		foreach (VRTK_ControllerEvents c in controllers) {
			c.enabled = false;
		}
		StartCoroutine(Restart());
	}

	IEnumerator Restart () {
		yield return new WaitForSeconds(5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
