using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayTimeCounter : MonoBehaviour {

	static PlayTimeCounter instance = null;

	[SerializeField] bool stopOnPause = true;
	float elapsedTime = 0f;

	void Awake() {
		if (instance == null) { instance = this; }
		else { Debug.LogError ("Only 1 instance of PlayTimeCounter can exist per scene!"); }
	}
	void Start() {

		PersistenceManager.OnSave += OnSave;
		PersistenceManager.OnLoad += OnLoad;
		GameController.OnPause += OnPause;
		GameController.OnResume += OnResume;
	}
		
	void Update () { elapsedTime += UnityEngine.Time.deltaTime; }

	void OnPause() { if (stopOnPause) this.enabled = false; }
	void OnResume() { if (stopOnPause) this.enabled = true; }

	public static TimeSpan PlayTime { get { return new TimeSpan (TimeSpan.TicksPerSecond * (long)instance.elapsedTime); } }

	public override string ToString () {
		return PlayTime.ToString();
	}

	[Serializable]
	struct SaveData {
		public float elapsedTime;
	}

	void OnSave (Dictionary<string, byte[]> data) {
		SaveData saveData = new SaveData () { elapsedTime = this.elapsedTime };
		byte[] bytes = PersistenceManager.Serialize (saveData);

		if (data.ContainsKey (GetType ().ToString ())) {
			data [GetType ().ToString ()] = bytes;
		}
		else {
			data.Add (GetType ().ToString (), bytes);
		}
	}
	void OnLoad (Dictionary<string, byte[]> data) {
		SaveData saveData = PersistenceManager.DeSerialize<SaveData> (data [GetType ().ToString ()]);
		this.elapsedTime = saveData.elapsedTime;
	}
}

