using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(PersistanceManager))]
[CanEditMultipleObjects]
public class PersistanceManagerEditor : Editor {

	SerializedProperty saveFileName;

	void OnEnable() {
		saveFileName = serializedObject.FindProperty ("saveFileName");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update ();

		EditorGUILayout.PropertyField (saveFileName, new GUIContent ("Save File Name"));

		if (Application.isPlaying) {

			if (GUILayout.Button("Save Game")) {
				if (!PersistanceManager.SaveGame ("Editor")) {
					PersistanceManager.NewGameSave ("Editor");
					PersistanceManager.SaveGame("Editor");
				}
			}
			if (GUILayout.Button("Load Game")) {
				PersistanceManager.LoadGame("Editor");
			}
		}
		serializedObject.ApplyModifiedProperties();
	}
}
