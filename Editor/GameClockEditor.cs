using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(GameClock))]
public class GameClockEditor : Editor {

	public override void OnInspectorGUI() {

		if (Application.isPlaying) {
			EditorGUILayout.LabelField (GameClock.Epoch.ToString ());
			EditorGUILayout.LabelField (((GameClock)target).ToString ());
		}
		base.OnInspectorGUI ();
	}
}

