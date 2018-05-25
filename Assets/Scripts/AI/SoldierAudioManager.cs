using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoldierAudioManager : AudioManager {
	[Header("Dependencies")]
	public SoldierWeaponManager weaponManager;
	public GameObject audioSourcesParent;

	[Space(5f)]
	public GameObject voiceAudio;
	private AudioSource[] _voiceAudioSources;
	public GameObject weaponAudio;
	private AudioSource[] _weaponAudioSources;

	private int whichWeaponAudioSource = 0;

	void Start () {
		_voiceAudioSources = voiceAudio.GetComponents<AudioSource>();
		_weaponAudioSources = weaponAudio.GetComponents<AudioSource>();
	}
	
	public void Shoot (AudioClip[] ac) {
		whichWeaponAudioSource++;
		if (whichWeaponAudioSource >= _weaponAudioSources.Length) {
			whichWeaponAudioSource = 0;
		}

		PlayClipAlways(_weaponAudioSources[whichWeaponAudioSource], ac, 0.9f, 1.1f);
	}

	public void TakeDamage () {
		
	}

	public void Die () {
		audioSourcesParent.transform.parent = null;
		Destroy(audioSourcesParent, 2f);
	}
}
