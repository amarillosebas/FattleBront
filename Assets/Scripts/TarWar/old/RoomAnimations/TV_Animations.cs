using UnityEngine;
using System.Collections;

public class TV_Animations : MonoBehaviour {
	public Renderer rend;
	public Texture onTexture;

	void Start () {
		//rend = GetComponentInChildren<Renderer> ();
	}

	public void turnOn() {
		rend.material.SetTexture ("_MainTex", onTexture);
		AudioSource au = GetComponent<AudioSource> ();
		au.Play ();
	}
}
