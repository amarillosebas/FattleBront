using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniMapUI : MonoBehaviour {
	public PlayerHP hp;
	public PlayerItems items;

	public TextMeshProUGUI txtHP;
	public TextMeshProUGUI txtAmmo;

	private float weirdError = 0f;
	private bool ready = false;

	void Start () {
		weirdError = Time.time + 0.5f;
		//StartCoroutine("WaitForPlayer");
	}
	
	void Update () {
		if (!ready && Time.time >= weirdError) {
			hp = FindObjectOfType<PlayerHP>();
			items = FindObjectOfType<PlayerItems>();
			ready = true;
		}

		if (hp) txtHP.text = hp.currentHP + "";
		if (items) txtAmmo.text = items.grenadeAmmo + "";
	}

	/*IEnumerator WaitForPlayer () {
		Debug.Log("caca");
		//yield return null;
		hp = FindObjectOfType<PlayerHP>();
		items = FindObjectOfType<PlayerItems>();
		Debug.Log("ya pe");
		yield return null;
		Debug.Log("ya mierda");
	}*/
}
