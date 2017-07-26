using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class PersistenceManager : MonoBehaviour {

	[Serializable]
	/// <summary>
	/// A struct containing the information for specific game saves
	/// </summary>
	public class GameSave {

		/// <summary>
		/// The name of the same save (unique)
		/// </summary>
		public string name;

		/// <summary>
		/// The date and time that the save was first created
		/// </summary>
		public DateTime creationTime;

		/// <summary>
		/// The date and time that the save was last modified
		/// </summary>
		public DateTime lastModified;

		/// <summary>
		/// The serialized data <data id, data byte array>
		/// </summary>
		public Dictionary<string, byte[]> data;
	}

	/// <summary>
	/// singleton
	/// </summary>
	static PersistenceManager instance = null;

	/// <summary>
	/// Used for serializing and deserializing
	/// </summary>
	static BinaryFormatter binForm = new BinaryFormatter();

	/// <summary>
	/// The file to store the data in (at Application.DataPath)
	/// </summary>
	[SerializeField] string saveFileName;

	/// <summary>
	/// The loaded game data.
	/// </summary>
	List<GameSave> gameData = new List<GameSave>();

	/// <summary>
	/// Called by unity when script is loaded
	/// </summary>
	void Awake() {

		// make sure this is the only instance, error if not
		if (instance == null) { instance = this; }
		else { Debug.LogError ("Only 1 instance of PersistanceManager can exist per scene!"); }
	}

	/// <summary>
	/// Serializes anything that is System.Serializable
	/// </summary>
	/// <returns>Serialized object as byte array</returns>
	/// <param name="obj">Object to serialize</param>
	/// <typeparam name="T">The objects type</typeparam>
	public static byte[] Serialize<T> (T obj) {
		
		// define a byte array
		byte[] bytes;

		// create a memory stream
		using (var ms = new MemoryStream())
		{
			// serialize using bin formatter
			binForm.Serialize(ms, obj);

			// turn into byte array
			bytes = ms.ToArray();
		}

		// return byte array
		return bytes;
	}

	/// <summary>
	/// Deserializes anything that is System.Serializable
	/// </summary>
	/// <returns>deserialized object</returns>
	/// <param name="bytes">Bytes to deserialize</param>
	/// <typeparam name="T">The type to cast to</typeparam>
	public static T DeSerialize<T>(byte[] bytes) {

		// define a byte array
		T data;

		// create a memory stream
		using (var memStream = new MemoryStream())
		{
			// write bytes to memory
			memStream.Write(bytes, 0, bytes.Length);

			// find origin
			memStream.Seek(0, SeekOrigin.Begin);

			// cast to T
			data = (T)binForm.Deserialize(memStream);
		}

		// return object
		return data;
	}

	/// <summary>
	/// Creates a new game save file
	/// </summary>
	/// <returns><c>true</c>, if game save was created, <c>false</c> otherwise.</returns>
	/// <param name="name">The name of the new game save (unique)</param>
	public static bool NewGameSave (string name) {

		// make sure the name is unique
		foreach (GameSave data in instance.gameData) { if (data.name == name) return false; }

		// create a new save structure
		instance.gameData.Add (new GameSave () {
			name = name,
			creationTime = DateTime.Now,
			lastModified = DateTime.Now,
			data = new Dictionary<string, byte[]> ()
		});

		// save it
		SaveGame (name);

		// all went well
		return true;
	}


	/// <summary>
	/// stores callbacks for when game data is saved
	/// </summary>
	event SaveLoadCallback onSaveGame;

	/// <summary>
	/// Adds callback to onSaveGame
	/// </summary>
	public static event SaveLoadCallback OnSave { add { instance.onSaveGame += value; } remove { instance.onSaveGame -= value; } }

	/// <summary>
	/// stores callbacks for when game data is loaded
	/// </summary>
	event SaveLoadCallback onLoadGame;

	/// <summary>
	/// Adds callback to onLoadGame
	/// </summary>
	public static event SaveLoadCallback OnLoad { add { instance.onLoadGame += value; } remove { instance.onLoadGame -= value; } }


	/// <summary>
	/// Saves the game.
	/// </summary>
	/// <returns><c>true</c>, if game was saved, <c>false</c> otherwise.</returns>
	/// <param name="saveName">Save name.</param>
	public static bool SaveGame(string saveName) { return instance._SaveGame (saveName); }
	public bool _SaveGame(string saveName) {

		// find the save by name
		for (int i = 0; i < instance.gameData.Count; i++) {

			// if it is found
			if (instance.gameData[i].name == saveName) {

				// call onSaveGame and let all the components fill the dictionary
				if (onSaveGame != null) onSaveGame (instance.gameData[i].data);

				// serialize and write to file
				File.WriteAllBytes (Application.persistentDataPath + saveFileName, Serialize (gameData));

				// all went well
				return true;
			}
		}

		// game save does not exist
		return false;
	}

	/// <summary>
	/// Loads the game.
	/// </summary>
	/// <returns><c>true</c>, if game was loaded, <c>false</c> otherwise.</returns>
	/// <param name="saveName">Save name.</param>
	public static bool LoadGame(string saveName) { return instance._LoadGame (saveName); }
	public bool _LoadGame (string saveName) {

		// find the save by name
		for (int i = 0; i < instance.gameData.Count; i++) {

			// if it is found
			if (instance.gameData[i].name == saveName) {

				// deserialize the dictionary
				instance.gameData = DeSerialize<List<GameSave>> (File.ReadAllBytes(Application.persistentDataPath + saveFileName));

				// call onLoadGame and let all the components get their data from the dictionary
				if (onLoadGame != null) { onLoadGame (instance.gameData[i].data); }

				// all good
				return true;
			}
		}

		// game save does not exist
		return false;
	}
}