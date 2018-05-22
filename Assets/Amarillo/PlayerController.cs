using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PlayerController : MonoBehaviour {
	[Header("== Player Movement ==")]
	[Space(5)]
	public Transform camera;
	private Rigidbody _rb;
	public float speed = 2f;
	public float sprint = 1f;
	private float canSprint = 1f;

	[Space(5)]
	[Header("== Player Logic ==")]
	private Text text_R;
	private Text text_L;

	[Space(5)]
	[Header("== Controllers ==")]
	public GameObject controllerR;
	public GameObject controllerL;
	private SteamVR_TrackedObject _trackedObj_R;
	private SteamVR_TrackedObject _trackedObj_L;
	private SteamVR_Controller.Device deviceR;
	private SteamVR_Controller.Device deviceL;
	private Transform _modelTransformR;
	private Transform _modelTransformL;

	[Space(5)]
	[Header("== Controllers Logic ==")]
	public int controllerRState;
	public int controllerLState;
	//[HideInInspector]
	public GenericState[] states;

	[Space(5)]
	[Header("== Weapons/Inventory ==")]
	public GameObject[] inventory;
	private int _onRightHandIndex;
	private int _onLeftHandIndex;
	private GameObject _OnRightHand;
	private GameObject _OnLeftHand;
	public float gunRotation;

	[Header("== Selection Logic ==")]
	[Space(5)]
	public GameObject selectionPoint;
	public float RaycastDistance = 25f;
	private LineRenderer line_R;
	private LineRenderer line_L;
	private Rigidbody _selectedRB_R;
	private Rigidbody _selectedRB_L;
	public Color normalColor;
	public Color selectColor;
	private float _rbDistance_R;
	private float _rbDistance_L;
	private GameObject _spR;
	private CharacterJoint _cjR;
	private GameObject _spL;
	private CharacterJoint _cjL;
	public float LetGoForce = 100f;

	void Awake () {
		_rb = GetComponent<Rigidbody>();
		_trackedObj_R = controllerR.GetComponent<SteamVR_TrackedObject>();
		_trackedObj_L = controllerL.GetComponent<SteamVR_TrackedObject>();
		line_R = controllerR.GetComponent<LineRenderer>();
		line_L = controllerL.GetComponent<LineRenderer>();
		text_R = controllerR.GetComponentInChildren<Text>();
		text_L = controllerL.GetComponentInChildren<Text>();
		_modelTransformR = controllerR.transform.GetChild(0).transform;
		_modelTransformL = controllerL.transform.GetChild(0).transform;

		text_R.text = states[controllerRState].name;
		text_L.text = states[controllerLState].name;
	}
	
	void FixedUpdate () {
		deviceR = SteamVR_Controller.Input((int)_trackedObj_R.index);
		deviceL = SteamVR_Controller.Input((int)_trackedObj_L.index);

		//= STATE CHANGES =
		if (deviceR.GetTouchDown(SteamVR_Controller.ButtonMask.Grip)) {
			if (controllerRState == 2) Destroy(_OnRightHand);
			if (controllerRState == 3) Destroy(_OnRightHand);
			if (controllerRState == 4) Destroy(_OnRightHand);

			controllerRState++;
			if (controllerRState >= states.Length) controllerRState = 0;
			text_R.text = states[controllerRState].name;
		}
		if (deviceL.GetTouchDown(SteamVR_Controller.ButtonMask.Grip)) {
			if (controllerLState == 2) Destroy(_OnLeftHand);
			if (controllerLState == 3) Destroy(_OnLeftHand);
			if (controllerLState == 4) Destroy(_OnLeftHand);

			controllerLState++;
			if (controllerLState >= states.Length) controllerLState = 0;
			text_L.text = states[controllerLState].name;
		}

		switch (controllerRState) {
			case 0:
				//= TOUCHPAD =
				if (deviceR.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) canSprint = sprint;
				if (deviceR.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) canSprint = 1f;
				if (deviceR.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
					_rb.MovePosition(CalculateMovement(controllerR.transform, deviceR.GetAxis()));
					_rb.velocity = Vector3.zero;
				}
			break;
			case 1:
				//= TOUCHPAD =
				if (deviceR.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
					SearchRBs(controllerR.transform, 1);
				}
				if (deviceR.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) {
					ClearRays(1);
				}
				//= TRIGGER =
				if (deviceR.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
					if (_selectedRB_R != null) {
						MoveRB(1);
					}
				}
				if (deviceR.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
					if (_selectedRB_R != null) {
						LetGoRB(1);
					}
				}
			break;
			case 2:
				if (!_OnRightHand) {
					_onRightHandIndex = 0;
					InstantiateItem(_onRightHandIndex, 1);
				} else if (deviceR.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
					_OnRightHand.GetComponent<PistolScript>().Shoot();
				}
			break;
			case 3:
				if (!_OnRightHand) {
					_onRightHandIndex = 1;
					InstantiateItem(_onRightHandIndex, 1);
				} else {
					if (deviceR.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
						_OnRightHand.GetComponent<SMGScript>().TriggerOn();
					}
					if (deviceR.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
						_OnRightHand.GetComponent<SMGScript>().TriggerOff();
					}
				}
			break;
			case 4:
				if (!_OnRightHand) {
					_onRightHandIndex = 2;
					InstantiateItem(_onRightHandIndex, 1);
				}
			break;
		}

		switch (controllerLState) {
			case 0:
				//= TOUCHPAD =
				if (deviceL.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) canSprint = sprint;
				if (deviceL.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) canSprint = 1f;
				if (deviceL.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
					_rb.MovePosition(CalculateMovement(controllerL.transform, deviceL.GetAxis()));
					_rb.velocity = Vector3.zero;
				}
			break;
			case 1:
				//= TOUCHPAD =
				if (deviceL.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
					SearchRBs(controllerL.transform, 0);
				}
				if (deviceL.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) {
					ClearRays(0);
				}
				//= TRIGGER =
				if (deviceL.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
					if (_selectedRB_L != null) {
						MoveRB(0);
					}
				}
				if (deviceL.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
					if (_selectedRB_L != null) {
						LetGoRB(0);
					}
				}
			break;
			case 2:
				if (!_OnLeftHand) {
					_onLeftHandIndex = 0;
					InstantiateItem(_onLeftHandIndex, 0);
				} else if (deviceL.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
					_OnLeftHand.GetComponent<PistolScript>().Shoot();
				}
			break;
			case 3:
				if (!_OnLeftHand) {
					_onLeftHandIndex = 1;
					InstantiateItem(_onLeftHandIndex, 0);
				} else {
					if (deviceL.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
						_OnLeftHand.GetComponent<SMGScript>().TriggerOn();
					}
					if (deviceL.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
						_OnLeftHand.GetComponent<SMGScript>().TriggerOff();
					}
				}
			break;
			case 4:
				if (!_OnLeftHand) {
					_onLeftHandIndex = 2;
					InstantiateItem(_onLeftHandIndex, 0);
				}
			break;
		}
	}

	Vector3 CalculateMovement (Transform t, Vector2 inputAxis) {
		Vector3 result;

		Vector3 move = t.forward;
		move = new Vector3 (move.x * inputAxis.y, 0f, move.z * inputAxis.y);
		Vector3 move2 = t.right;
		move2 = new Vector3 (move2.x * inputAxis.x, 0f, move2.z * inputAxis.x);

		result = transform.position + ((move + move2) / 2) * speed * Time.deltaTime * canSprint;

		return result;
	}

	void SearchRBs (Transform t, int c) {
		RaycastHit hit;
		if (Physics.Raycast(t.position, t.forward, out hit, RaycastDistance)) {
			if (GrabRBs (t, c, hit)) {
				if (c == 0) {
					line_L.SetColors(selectColor, selectColor);
					_rbDistance_L = hit.distance;
				}
				else 		{
					line_R.SetColors(selectColor, selectColor);
					_rbDistance_R = hit.distance;
				}
			} else {
				if (c == 0) line_L.SetColors(normalColor, normalColor);
				else 		line_R.SetColors(normalColor, normalColor);
				}
		} else {
			GrabRBs (t, c);
			if (c == 0) line_L.SetColors(normalColor, normalColor);
			else 		line_R.SetColors(normalColor, normalColor);
		}
	}

	bool GrabRBs (Transform t, int c, RaycastHit r) {
		bool result = false;

		Vector3[] pos = new Vector3[2];
		pos[0] = t.position;
		pos[1] = r.point;

		if (c == 0) {
			_selectedRB_L = r.transform.gameObject.GetComponent<Rigidbody>();
			if (_selectedRB_L) result = true;
		} else {
			_selectedRB_R = r.transform.gameObject.GetComponent<Rigidbody>();
			if (_selectedRB_R) result = true;
		}

		if (c == 0) line_L.SetPositions(pos);
		else line_R.SetPositions(pos);
		
		return result;
	}
	void GrabRBs (Transform t, int c) {
		Vector3[] pos = new Vector3[2];
		pos[0] = t.position;
		pos[1] = t.position + t.forward * RaycastDistance;

		if (c == 0) line_L.SetPositions(pos);
		else line_R.SetPositions(pos);
	}
	void ClearRays (int c) {
		Vector3[] pos = new Vector3[2];
		pos[0] = Vector3.zero;
		pos[1] = Vector3.zero;
		if (c == 0) {
			line_L.SetPositions(pos);
			line_L.SetColors(normalColor, normalColor);
		} else {
			line_R.SetPositions(pos);
			line_R.SetColors(normalColor, normalColor);
		}
	}

	void MoveRB (int c) {
		if (c == 0) {
			if (_spL == null) {
				_spL = Instantiate(selectionPoint, _modelTransformL.position + _modelTransformL.forward * _rbDistance_L, Quaternion.identity) as GameObject;
				_spL.transform.SetParent(_modelTransformL);
				Utils.oldPos = _selectedRB_L.transform.position;
			} else {
				if (_cjL == null) { 
					_cjL = _selectedRB_L.gameObject.AddComponent<CharacterJoint>();
					_cjL.connectedBody = _spL.GetComponent<Rigidbody>();
				}
				_spL.transform.position =  _modelTransformL.position + _modelTransformL.forward * _rbDistance_L;
				Vector3 v = Utils.CalculateVelocity(_selectedRB_L.transform.position);
			}
		} 

		else {
			if (_spR == null) {
				_spR = Instantiate(selectionPoint, _modelTransformR.position + _modelTransformR.forward * _rbDistance_R, Quaternion.identity) as GameObject;
				_spR.transform.SetParent(_modelTransformR);
				Utils.oldPos = _selectedRB_R.transform.position;
			} else {
				if (_cjR == null) { 
					_cjR = _selectedRB_R.gameObject.AddComponent<CharacterJoint>();
					_cjR.connectedBody = _spR.GetComponent<Rigidbody>();
				}
				_spR.transform.position =  _modelTransformR.position + _modelTransformR.forward * _rbDistance_R;
				Vector3 v = Utils.CalculateVelocity(_selectedRB_R.transform.position);
				//Debug.Log (v);
			}
		}
	}
	void LetGoRB (int c) {
		if (c == 0) {
			Destroy(_cjL);
			Destroy(_spL);
			//Vector3 v = Utils.CalculateVelocity(_selectedRB_L.transform.position);
			//v = _selectedRB_L.transform.InverseTransformPoint(v);
			//Vector3 v = _selectedRB_L.transform.TransformPoint(Utils.CalculateVelocity(_selectedRB_L.transform.position));
			//_selectedRB_L.AddForce(v * LetGoForce);
		}

		else {
			Destroy(_cjR);
			Destroy(_spR);
			//Vector3 v = Utils.CalculateVelocity(_selectedRB_R.transform.position);
			//v = _selectedRB_R.transform.InverseTransformPoint(v);
			//Vector3 v = _selectedRB_R.transform.TransformPoint(Utils.CalculateVelocity(_selectedRB_R.transform.position));
			/*_selectedRB_R.velocity = Vector3.zero;
			_selectedRB_R.AddForce(v * LetGoForce);
			Debug.Log (v);*/
		}
	}

	void InstantiateItem (int i, int c) {
		if (c == 0) {
			_OnLeftHand = Instantiate(inventory[i], _modelTransformL.position, _modelTransformL.rotation) as GameObject;
			_OnLeftHand.transform.SetParent(_modelTransformL);
			if (i != 2) _OnLeftHand.transform.Rotate(new Vector3 (gunRotation, 0, 0));
			else _OnLeftHand.transform.Rotate(new Vector3 (90, 0, 0));
		}
		else {
			_OnRightHand = Instantiate(inventory[i], _modelTransformR.position, _modelTransformR.rotation) as GameObject;
			_OnRightHand.transform.SetParent(_modelTransformR);
			if (i != 2) _OnRightHand.transform.Rotate(new Vector3 (gunRotation, 0, 0));
			else _OnRightHand.transform.Rotate(new Vector3 (90, 0, 0));
		}
	}
}
