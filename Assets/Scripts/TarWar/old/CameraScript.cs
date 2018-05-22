using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public GameObject[] viewableObjects;

	public GameObject[] starFighters;
	public GameObject[] soldiers;
	public GameObject[] vehicles;

	public Camera camera;

	public bool sfBool;
	public bool sBool;
	public bool vBool;

	/*private MeshRenderer[] _starFighters;
	private MeshRenderer[] _soldiers;
	private MeshRenderer[] _vehicles;*/

	void Start () {
		/*for (int i = 0; i < starFighters.Length; i++) {
			_starFighters[i] = starFighters[i].GetComponentsInChildren<MeshRenderer>();
		}
		for (int j = 0; j < soldiers.Length; j++) {
			_soldiers[j] = soldiers[j].GetComponentsInChildren<MeshRenderer>();
		}
		for (int k = 0; k < vehicles; k++) {
			_vehicles[k] = vehicles[k].GetComponentsInChildren<MeshRenderer>();
		}*/
		camera = GetComponent<Camera> ();
	}

	void Update () {
		Ray ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			GameObject gameObj = hit.transform.gameObject;
			for (int i = 0; i < viewableObjects.Length; i++) {
				if(viewableObjects[i].GetComponent<ViewableObject>()&&viewableObjects[i].GetComponent<BoxCollider>()) {
					if(viewableObjects[i] == gameObj) {
						gameObj.GetComponent<ViewableObject>().isBeingViewed = true;
					}else{
						viewableObjects[i].GetComponent<ViewableObject>().isBeingViewed = false;
					}
				}
			}
			//Debug.Log(hit.transform.gameObject.name);
//			print ("I'm looking at " + hit.transform.name);
		}
		checkObjects ();
	}

	void checkObjects() {
		if (sfBool) {
			iterateGameObjectList(starFighters);
		}
		if (sBool) {
			iterateGameObjectList(soldiers);
		}
		if (vBool) {
			iterateGameObjectList(vehicles);
		}
	}
	void iterateGameObjectList(GameObject[] gameObjectList) {
		for (int i = 0; i < gameObjectList.Length; i++) {
			Vector3 screenPoint = camera .WorldToViewportPoint(gameObjectList[i].transform.position);
			bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
			if(!onScreen) {
				gameObjectList[i].SetActive(true);
				//Debug.Log("Object: "+gameObjectList[i].name);
			}
		}
	}
}
