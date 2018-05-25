using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierHP : EntityHP {
	[Header("Dependencies")]
	public AnimatorManager animatorManager;
	public SoldierAudioManager audioManager;

	[Space(1f)]
	public Slider sliderHP;

	[Space(1f)]
	public float corpseLifeSpan;
	
	void Update () {
		if (sliderHP) sliderHP.value = currentHP;
	}

	public override void TakeDamage (int d) {
		currentHP -= d;
		if (currentHP <= 0) {
			Die();
		} else {
			audioManager.TakeDamage();
		}
	}

	public override void Die () {
		animatorManager.Die();
		audioManager.Die();
		Destroy(gameObject, corpseLifeSpan);
	}
}
