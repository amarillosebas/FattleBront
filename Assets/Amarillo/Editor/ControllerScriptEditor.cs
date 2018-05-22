using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(PlayerController))]
public class ControllerScriptEditor : Editor {
	private PlayerController _target;

	public override void OnInspectorGUI() {
		_target = (PlayerController) target;

		DrawDefaultInspector();
		DrawStatesForm();
	}

	void DrawStatesForm () {
		GUILayout.Label("== Controller States ==", EditorStyles.boldLabel);
		for (int i = 0; i < _target.states.Length; i++) {
			DrawState(i);
		}
		//DrawAddStateButton();
	}

	void DrawState (int index) {
		if (index >= _target.states.Length) return;

		GUILayout.BeginHorizontal(); {
			EditorGUI.BeginChangeCheck();

			int newState = System.Int32.Parse(GUILayout.TextField("" + _target.states[index].state, GUILayout.Width(40)));
			string newName = GUILayout.TextField(_target.states[index].name, GUILayout.Width(200));

			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(_target, "Modify State");
				_target.states[index].state = newState;
				_target.states[index].name = newName;
				EditorUtility.SetDirty(_target); //this saves: https://youtu.be/9bHzTDIJX_Q?t=1977
			}

			if (GUILayout.Button("Remove")) {
				Undo.RecordObject(_target, "Remove State");
				_target.states[index].state = -1;
				_target.states[index].name = "";
				EditorUtility.SetDirty(_target);
			}

			GUILayout.EndHorizontal();
		}
	}

	/*void DrawAddStateButton () {
		if (GUILayout.Button("Add State", GUILayout.Height(30))) {
			Undo.RecordObject(_target, "Add State");
			_target.states.
		}
	}*/
}
