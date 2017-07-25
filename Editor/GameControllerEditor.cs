using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(GameController))]
[CanEditMultipleObjects]
public class GameControllerEditor : Editor {

	SerializedProperty paused;

	void OnEnable() {
		paused = serializedObject.FindProperty ("paused");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update ();

		if (Application.isPlaying) {

			if ( GUILayout.Button ( (paused.boolValue) ? "Resume" : "Pause")) {
				if (paused.boolValue)
					GameController.ResumeGame ();
				else
					GameController.PauseGame ();
			}
		}
		serializedObject.ApplyModifiedProperties();
	}
}

