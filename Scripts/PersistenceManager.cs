using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


/// <summary>
/// Game controller: core.
/// </summary>
public class PersistanceManager : MonoBehaviour {

	[Serializable]
	public class GameSave {
		public string name;
		public DateTime creationTime;
		public DateTime lastModified;
		public Dictionary<string, byte[]> data;
	}

	static PersistanceManager instance = null;
	static BinaryFormatter binForm = new BinaryFormatter();

	[SerializeField] string saveFileName;

	List<GameSave> gameData = new List<GameSave>();

	void Awake() {

		if (instance == null) { instance = this; }
		else { Debug.LogError ("Only 1 instance of PersistanceManager can exist per scene!"); }
	}


	public static byte[] Serialize<T> (T obj) {
		byte[] bytes;
		using (var ms = new MemoryStream())
		{
			binForm.Serialize(ms, obj);
			bytes = ms.ToArray();
		}
		return bytes;
	}

	public static T DeSerialize<T>(byte[] bytes) {

		T data;
		using (var memStream = new MemoryStream())
		{
			memStream.Write(bytes, 0, bytes.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			data = (T)binForm.Deserialize(memStream);
		}
		return data;
	}


	public static bool NewGameSave (string name) {

		foreach (GameSave data in instance.gameData) { if (data.name == name) return false; }
		instance.gameData.Add (new GameSave () {
			name = name,
			creationTime = DateTime.Now,
			lastModified = DateTime.Now,
			data = new Dictionary<string, byte[]> ()
		});
		SaveGame (name);
		return true;
	}


	event SaveLoadCallback onSaveGame;
	public static event SaveLoadCallback OnSave { add { instance.onSaveGame += value; } remove { instance.onSaveGame -= value; } }

	event SaveLoadCallback onLoadGame;
	public static event SaveLoadCallback OnLoad { add { instance.onLoadGame += value; } remove { instance.onLoadGame -= value; } }


	public static bool SaveGame(string saveName) { return instance._SaveGame (saveName); }
	public bool _SaveGame(string saveName) {

		for (int i = 0; i < instance.gameData.Count; i++) {
			if (instance.gameData[i].name == saveName) {
				if (onSaveGame != null) onSaveGame (instance.gameData[i].data);
				File.WriteAllBytes (Application.persistentDataPath + saveFileName, Serialize (gameData));
				return true;
			}
		}
		return false;
	}


	public static bool LoadGame(string saveName) { return instance._LoadGame (saveName); }
	public bool _LoadGame (string saveName) {

		for (int i = 0; i < instance.gameData.Count; i++) {
			if (instance.gameData[i].name == saveName) {
				instance.gameData = DeSerialize<List<GameSave>> (File.ReadAllBytes(Application.persistentDataPath + saveFileName));
				if (onLoadGame != null) { onLoadGame (instance.gameData[i].data); }
				return true;
			}
		}
		return false;
	}
}