using UnityEngine;
using System.Collections;

public class WallAnimation : MonoBehaviour {
	public GameObject dustP;

	public void dust() {
		dustP.SetActive (true);
		AudioSource au = GetComponent<AudioSource> ();
		au.Play ();
	}
}
