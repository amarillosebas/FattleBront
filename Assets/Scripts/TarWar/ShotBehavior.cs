using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {
	public float movementSpeed;
	public GameObject impact;
	public float collisionDistance = 2f;
	public int damage;
	
	void Update () {
		transform.position += transform.forward * Time.deltaTime * movementSpeed;
	
		RaycastHit hit;
		if (Physics.Raycast(transform.position + transform.forward, transform.forward, out hit, collisionDistance)) {
			EntityHP hp = hit.transform.GetComponent<EntityHP>();
			if (hp) hp.TakeDamage(damage);
			if (impact) {
				GameObject p = Instantiate (impact, transform.position, Quaternion.identity) as GameObject;
				Destroy(p, 1f);
			}
			Destroy (gameObject);
		}
	}
}
