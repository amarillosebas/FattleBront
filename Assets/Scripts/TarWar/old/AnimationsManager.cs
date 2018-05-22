using UnityEngine;
using System.Collections;

public class AnimationsManager : MonoBehaviour {
	public AnimatedRigidbody[] _animatedRigidbodies;
	public Animator room;
	public GameObject starFighters;
	public GameObject ships;
	public GameObject soldiers;
	public GameObject vehicles;
	public Animator hand;
	public AudioSource tone;
	public GameObject[] otherObjects;
	public Radio_Animations _radio;
	public fadeToBlack _fader;

	private Animator[] _starFighters;
	private Animator[] _ships;
	private Animator[] _soldiers;
	private Animator[] _vehicles;

	public float timeToCrazyRadio;
	public float timeToAnimateRigidbodies;
	public float timeToOpenRoom;
	public float timeToShips;
	public float timeToStarFighters;
	public float timeToSoldiers;
	public float timeToVehicles;
	public float timeToSoundRingtone;
	public float timeToShowHand;
	public float timeToCloseRoom;
	public float timeToHideHand;
	public float timeToEnd;

	public CameraScript camScript;

	public AudioClip magic;
	public AudioClip walls;
	public AudioClip wallsFell;
	public AudioClip battleground;
	public AudioClip battlemusic;
	public AudioSource audios1;
	public AudioSource audios2;

	void Start () {
		room.speed = 0;
		hand.speed = 0;

		_starFighters = new Animator[starFighters.transform.childCount];
		_ships = new Animator[ships.transform.childCount];
		_soldiers = new Animator[soldiers.transform.childCount];
		_vehicles = new Animator[vehicles.transform.childCount];

		/*for (int i = 0; i < _starFighters.Length; i++) {
			_starFighters[i] = starFighters.transform.GetChild(i).GetComponentInChildren<Animator>();
			_starFighters[i].speed = 0;
		}*/
		for (int j = 0; j < _ships.Length; j++) {
			_ships[j] = ships.transform.GetChild(j).GetComponentInChildren<Animator>();
			_ships[j].speed = 0;
		}
		for (int k = 0; k < _soldiers.Length; k++) {
			_soldiers[k] = soldiers.transform.GetChild(k).GetComponentInChildren<Animator>();
			//_soldiers[k].speed = 0;
		}
		for (int l = 0; l < _vehicles.Length; l++) {
			_vehicles[l] = vehicles.transform.GetChild(l).GetComponentInChildren<Animator>();
			//_vehicles[l].speed = 0;
		}

		RandomizeArray (_starFighters);
		RandomizeArray (_ships);
		RandomizeArray (_soldiers);
		RandomizeArray (_vehicles);

		waitToCrazyRadio ();
		waitToAnimateRigidbodies ();
		waitToOpenRoom ();
		waitToStarFighters ();
		waitToShips ();
		waitToSoldiers ();
		waitToVehicles ();
		waitToCloseRoom ();
	}

	void waitToCrazyRadio() {
		StartCoroutine ("toCrazyRadio");
	}
	IEnumerator toCrazyRadio() {
		yield return new WaitForSeconds(timeToCrazyRadio); Debug.Log("=== CrazyRadio ===" + Time.time);
		_radio.change = true;
	}

	void waitToAnimateRigidbodies() {
		StartCoroutine ("toAnimateRigidbodies");
	}
	IEnumerator toAnimateRigidbodies() {
		yield return new WaitForSeconds(timeToAnimateRigidbodies); Debug.Log("=== AnimateRigibodies ===" + Time.time);
		for (int i = 0; i < _animatedRigidbodies.Length; i++) {
			_animatedRigidbodies[i].deActivateRigidbody();
		}
		audios1.clip = magic;
		audios1.Play ();
	}

	void waitToOpenRoom() {
		StartCoroutine ("toOpenRoom");
	}
	IEnumerator toOpenRoom() {
		yield return new WaitForSeconds(timeToOpenRoom); Debug.Log("=== OpenRoom ===" + Time.time);
		for (int i = 0; i < _animatedRigidbodies.Length; i++) {
			_animatedRigidbodies[i].activateRigidbody();
		}
		AudioSource au = GetComponent<AudioSource> ();
		au.PlayDelayed (0.5f);
		audios2.clip = walls;
		audios2.Play ();
		room.speed = 1;
		_radio.toStop ();

		audios1.clip = battleground;
		audios1.volume = 1;
		audios1.PlayDelayed (1f);
	}

	void waitToStarFighters() {
		StartCoroutine ("toStarFighters");
	}
	IEnumerator toStarFighters() {
		yield return new WaitForSeconds(timeToStarFighters);
		Debug.Log("=== AppearFighters ===" + Time.time);
		camScript.sfBool = true;
	}

	void waitToShips() {
		StartCoroutine ("toShips");
	}
	IEnumerator toShips() {
		yield return new WaitForSeconds(timeToShips);
		Debug.Log("=== AppearShips ===" + Time.time);
		
		audios2.clip = battlemusic;
		audios2.volume = 0.35f;
		audios2.PlayDelayed (0.75f);

		for (int i = 0; i < _ships.Length; i++) {
			_ships[i].speed = 1;
			yield return new WaitForSeconds(Random.Range(0.2f, 0.6f));
		}
	}
	
	void waitToSoldiers() {
		StartCoroutine ("toSoldiers");
	}
	IEnumerator toSoldiers() {
		yield return new WaitForSeconds(timeToSoldiers);
		Debug.Log("=== AppearSoldiers ===" + Time.time);
		camScript.sBool = true;
	}
	
	void waitToVehicles() {
		StartCoroutine ("toVehicles");
	}
	IEnumerator toVehicles() {
		yield return new WaitForSeconds(timeToVehicles);
		Debug.Log("=== AppearVehicles ===" + Time.time);
		camScript.vBool = true;
	}

	void waitToCloseRoom() {
		StartCoroutine ("toCloseRoom");
	}
	IEnumerator toCloseRoom() {
		yield return new WaitForSeconds(timeToSoundRingtone);
		tone.volume = 0.35f;
		tone.Play ();
		Debug.Log("=== Ringtone ===" + Time.time);
		StartCoroutine ("toShowHand");

	}
	IEnumerator toShowHand() {
		yield return new WaitForSeconds(timeToShowHand - timeToSoundRingtone);
		//Debug.Log("=== ShowHand ===" + Time.time);
		hand.speed = 1;
		StartCoroutine ("toCloseCloseRoom");
	}
	IEnumerator toCloseCloseRoom() {
		yield return new WaitForSeconds(timeToCloseRoom - timeToShowHand);
		Debug.Log("=== CloseRoom ===" + Time.time);
		room.SetTrigger ("triggerClose");
		StartCoroutine ("toHideHand");
		StartCoroutine ("toEnd");
	}
	IEnumerator toHideHand() {
		yield return new WaitForSeconds(timeToHideHand - timeToCloseRoom);
		Debug.Log("=== HideHand ===" + Time.time);
		tone.GetComponent<RingtoneScript>().toStop = true;
		hand.SetTrigger ("triggerHide");
		StartCoroutine ("toStopAnimations");
	}
	IEnumerator toStopAnimations() {
		yield return new WaitForSeconds (1.5f);
		for (int i = 0; i < otherObjects.Length; i++) {
			otherObjects[i].SetActive(false);
		}
	}
	IEnumerator toEnd() {
		yield return new WaitForSeconds(timeToEnd - timeToCloseRoom);
		_fader.sceneEnding = true;
		Debug.Log("=== End ===" + Time.time);
	}

	void RandomizeArray<T>(T[] a){
		for (int i = a.Length - 1; i > 0; i--) {
			int r = Random.Range(0, i);
			T tmp = a[i];
			a[i] = a[r];
			a[r] = tmp;
		}
	}
}
