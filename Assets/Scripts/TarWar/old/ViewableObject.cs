using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ViewableObject : MonoBehaviour {
	public bool isBeingViewed;
	public float triggerTime;
	public float currentTime;
	public bool enableTrigger;
	public UnityEvent onExecuteAfterTriggerTime = new UnityEvent();
	// Use this for initialization
	void Start () {
		enableTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (enableTrigger) {
			if (isBeingViewed) {
				currentTime+= Time.deltaTime;
				if(currentTime>=triggerTime)
				{
//					Debug.Log ("Trigger from: " + gameObject.name);
					if (onExecuteAfterTriggerTime != null && Application.isPlaying) onExecuteAfterTriggerTime.Invoke();
					enableTrigger = false;
					currentTime = 0;
				}
			} else {
				currentTime = 0;
			}
		}
	}
}
