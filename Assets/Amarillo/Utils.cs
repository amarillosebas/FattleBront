using UnityEngine;
using System.Collections;

public static class Utils {
	public static float Remap (float value, float low1, float high1, float low2, float high2) {
		float r;
		r = low2 + (value - low1) * (high2 - low2) / (high1 - low1);
		return r;
	}
	public static int Remap (int value, int low1, int high1, int low2, int high2) {
		int r;
		r = low2 + (value - low1) * (high2 - low2) / (high1 - low1);
		return r;
	}

	//Calculations made in world coordinates
	public static Vector3 oldPos;
	public static Vector3 CalculateVelocity (Vector3 p) {
		Vector3 v;
			Vector3 newPos = p;
			Vector3 media = newPos - oldPos;
			v = media / Time.deltaTime;
			oldPos = newPos;
		return v;
	}

	public static Vector3 CalculateMovement (Transform t, Vector2 inputAxis, float speed) {
		Vector3 result;

		Vector3 move = t.forward;
		move = new Vector3 (move.x * inputAxis.y, 0f, move.z * inputAxis.y);
		Vector3 move2 = t.right;
		move2 = new Vector3 (move2.x * inputAxis.x, 0f, move2.z * inputAxis.x);

		result = t.position + ((move + move2) / 2) * speed;

		return result;
	}
}
