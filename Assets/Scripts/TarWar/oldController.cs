using UnityEngine;
using System.Collections;

public class oldController : MonoBehaviour {
	public Transform camera;
	public SteamVR_TrackedObject controller1;
	public SteamVR_TrackedObject controller2;
	private SteamVR_Controller.Device device1;
	private SteamVR_Controller.Device device2;

	public GameObject pistol;
	private GameObject _OnHand;

	private Rigidbody _rb;

	public float speed;

	void Start () {
		_rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
		device1 = SteamVR_Controller.Input((int)controller1.index);
		device2 = SteamVR_Controller.Input((int)controller2.index);

		if (device2.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
			Vector3 move = controller2.transform.forward;
			move = new Vector3 (move.x * device2.GetAxis().y, 0f, move.z * device2.GetAxis().y);
			Vector3 move2 = controller2.transform.right;
			move2 = new Vector3 (move2.x * device2.GetAxis().x, 0f, move2.z * device2.GetAxis().x);

			_rb.MovePosition(transform.position + ((move + move2) / 2) * speed * Time.deltaTime);
		}

		if (device1.GetTouchDown(SteamVR_Controller.ButtonMask.Grip)) {
			if (!_OnHand) {
				_OnHand = Instantiate(pistol, controller1.transform.position, controller1.transform.rotation) as GameObject;
				_OnHand.transform.SetParent(controller1.transform);
			}
		}

		if (device1.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
			if (_OnHand) {
				_OnHand.GetComponent<PistolScript>().Shoot();
			}
		}
	}
}
