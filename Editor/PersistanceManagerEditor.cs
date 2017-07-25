using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(PersistenceManager))]
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
				if (!PersistenceManager.SaveGame ("Editor")) {
					PersistenceManager.NewGameSave ("Editor");
					PersistenceManager.SaveGame("Editor");
				}
			}
			if (GUILayout.Button("Load Game")) {
				PersistenceManager.LoadGame("Editor");
			}
		}
		serializedObject.ApplyModifiedProperties();
	}
}
