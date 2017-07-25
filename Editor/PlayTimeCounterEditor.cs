using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(PlayTimeCounter))]
public class PlayTimeCounterEditor : Editor {

	public override void OnInspectorGUI() {

		if (Application.isPlaying) {
			EditorGUILayout.LabelField (((PlayTimeCounter)target).ToString ());
		}
		base.OnInspectorGUI ();
	}
}


