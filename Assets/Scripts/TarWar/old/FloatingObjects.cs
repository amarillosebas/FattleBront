using UnityEngine;
using System.Collections;

public class FloatingObjects : MonoBehaviour {
	public Rigidbody[] rbs;
	private int _numberOfRigidbodies;
	public float floatForce1;
	public float floatForce2;
	public float floatTime;
	public float raiseTime;
	public float rotateSpeed;

	public bool doFloat;

	void Start () {
		rbs = new Rigidbody[transform.childCount];
		for (int i = 0; i < rbs.Length; i++) {
			rbs[i] = transform.GetChild(i).GetComponent<Rigidbody>();
			rbs[i].GetComponent<FloatyThing>().canFloat = true;
			rbs[i].GetComponent<FloatyThing>().gettingUp = true;
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Z))
			doFloat = !doFloat;
		if (doFloat) {
			for (int k = 0; k < rbs.Length; k++) {
				if (rbs[k].GetComponent<FloatyThing>().gettingUp)
					StartCoroutine(raiseIt(rbs[k]));
				else rbs[k].AddTorque(rotateSpeed, rotateSpeed, rotateSpeed);
				if (rbs[k].GetComponent<FloatyThing>().canFloat)
					StartCoroutine(floatIt(rbs[k]));
			}
			for (int l = 0; l < rbs.Length; l++) {
				rbs[l].AddForce(Vector3.up * floatForce2 / 9f);
			}
		}
	}

	public void FloatThings() {
		doFloat = true;
	}

	IEnumerator raiseIt (Rigidbody rb)
	{
		rb.AddForce(Vector3.up * floatForce1);
		yield return new WaitForSeconds(raiseTime);
		rb.GetComponent<FloatyThing> ().gettingUp = false;
	}
	IEnumerator floatIt (Rigidbody rb)
	{
		rb.GetComponent<FloatyThing> ().canFloat = false;
		rb.AddForce(Vector3.up * floatForce2);
		yield return new WaitForSeconds(floatTime);
		rb.GetComponent<FloatyThing> ().canFloat = true;
	}
}
