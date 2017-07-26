using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayTimeCounter : MonoBehaviour {

	/// <summary>
	/// singleton
	/// </summary>
	static PlayTimeCounter instance = null;

	/// <summary>
	/// should time accumulate while game is paused?
	/// </summary>
	[SerializeField] bool stopOnPause = true;

	/// <summary>
	/// accumulated seconds
	/// </summary>
	float elapsedTime = 0f;

	/// <summary>
	/// Called by unity when script is loaded
	/// </summary>
	void Awake() {

		// make sure this is the only instance, error if not
		if (instance == null) { instance = this; }
		else { Debug.LogError ("Only 1 instance of PlayTimeCounter can exist per scene!"); }
	}
		
	/// <summary>
	/// Called by unity just before script is started
	/// </summary>
	void Start() {

		// receive callback when game saves or loads
		PersistenceManager.OnSave += OnSave;
		PersistenceManager.OnLoad += OnLoad;

		// receive callback when game pauses or resumes
		GameController.OnPause += OnPause;
		GameController.OnResume += OnResume;
	}
		
	/// <summary>
	/// Called by unity every frame. Adds accumulated time 
	/// </summary>
	void Update () { elapsedTime += UnityEngine.Time.deltaTime; }

	/// <summary>
	/// disable the update loop when game pauses (assuming stopOnPause is true)
	/// </summary>
	void OnPause() { if (stopOnPause) this.enabled = false; }

	/// <summary>
	/// enable the update loop when game pauses (assuming stopOnPause is true)
	/// </summary>
	void OnResume() { if (stopOnPause) this.enabled = true; }

	/// <summary>
	/// time since start
	/// </summary>
	/// <value>Time since start</value>
	public static TimeSpan PlayTime { get { return new TimeSpan (TimeSpan.TicksPerSecond * (long)instance.elapsedTime); } }

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="PlayTimeCounter"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="PlayTimeCounter"/>.</returns>
	public override string ToString () {
		return PlayTime.ToString();
	}

	[Serializable]
	/// <summary>
	/// Contains anything relating to PlayTimeCounter that needs to be saved and loaded
	/// </summary>
	struct SaveData {
		public float elapsedTime;
	}

	/// <summary>
	/// Called just before game data saves
	/// </summary>
	void OnSave (Dictionary<string, byte[]> data) {

		// create struct of things to save
		SaveData saveData = new SaveData () { elapsedTime = this.elapsedTime };

		// serialize it
		byte[] bytes = PersistenceManager.Serialize (saveData);

		// if this has been saved before
		if (data.ContainsKey (GetType ().ToString ())) {

			// overwrite it
			data [GetType ().ToString ()] = bytes;
		}
		// if it hasn't been saved before
		else {
			
			// create a new key value pair for it
			data.Add (GetType ().ToString (), bytes);
		}
	}
	void OnLoad (Dictionary<string, byte[]> data) {
		SaveData saveData = PersistenceManager.DeSerialize<SaveData> (data [GetType ().ToString ()]);
		this.elapsedTime = saveData.elapsedTime;
	}
}

