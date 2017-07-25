using UnityEngine;
using System;
using System.Collections.Generic;


public partial class GameClock : MonoBehaviour {

	[Serializable]
	public struct Date {
		public int year;
		public int month;
		public int day;
		public int hour;
		public int minute;
		public int second;
	}


	// singleton
	static GameClock instance = null;

	// should time accumulate while game is paused?
	[SerializeField] bool stopOnPause = true;

	// X faster than real world time
	[SerializeField] float baseMultiplier = 10f;

	// in game speed settings
	[SerializeField] float[] speedMultipliers = new float[] {0f, 1f, 2f, 3f, 5f};

	// current speed setting (index of speedMultipliers)
	int gameSpeedIndex = 1;

	// in game starting date
	[SerializeField] Date startDate = new Date () { year = 2010, month = 1, day = 1, hour = 9, minute = 0, second = 0};

	// this is needed because unity will not serialize DateTime
	DateTime startDT;

	// accumulated seconds
	float elapsedTime = 0f;


	/// <summary>
	/// number of seconds (float) since start
	/// </summary>
	/// <value>The number of seconds since start</value>
	public static TimeSpan Epoch { get { return new TimeSpan (TimeSpan.TicksPerSecond * (long)instance.elapsedTime); } } 

	/// <summary>
	/// gets the current game time as a System.DateTime
	/// </summary>
	/// <value>The date time.</value>
	public static DateTime DateTime { get { return instance.startDT + Epoch; } }


	/// <summary>
	/// Called by unity when script is loaded
	/// </summary>
	void Awake() {

		// make sure this is the only instance, error if not
		if (instance == null) { instance = this; }
		else { Debug.LogError ("Only 1 instance of GameClock can exist per scene!"); }

		// convert the start Date to System.DateTime
		startDT = new DateTime (startDate.year, startDate.month, startDate.day, startDate.hour, startDate.minute, startDate.second);
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

	// Called by unity every frame. Adds accumulated time 
	void Update() { elapsedTime += UnityEngine.Time.deltaTime * baseMultiplier * speedMultipliers [gameSpeedIndex]; }

	/// <summary>
	/// Returns a fully formated date and time eg. Monday 01/01/03 04:05:06
	/// </summary>
	/// <returns>fully formated date and time.</returns>
	public override string ToString () {

		DateTime dateTime = DateTime;
		return string.Format ("{0} {1}/{2}/{3} {4}:{5}:{6}", dateTime.DayOfWeek, dateTime.Day, dateTime.Month, dateTime.Year, dateTime.Hour, dateTime.Minute, dateTime.Second);
	}
		
	[Serializable]
	/// <summary>
	/// Contains anything relating to GameClock that needs to be saved and loaded
	/// </summary>
	struct SaveData {
		public Date startDate;
		public DateTime startDT;
		public float elapsedTime;
	}

	/// <summary>
	/// Called as game pauses
	/// </summary>
	void OnPause() { if (stopOnPause) this.enabled = false; }

	/// <summary>
	/// Called as game resumes from pause
	/// </summary>
	void OnResume() { if (stopOnPause) this.enabled = true; }

	/// <summary>
	/// Called just before game data saves
	/// </summary>
	void OnSave (Dictionary<string, byte[]> data) {

		// create struct of things to save
		SaveData saveData = new SaveData () { elapsedTime = this.elapsedTime, startDate = this.startDate, startDT = this.startDT  };

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

	/// <summary>
	/// Called just after game data is loaded
	/// </summary>
	void OnLoad (Dictionary<string, byte[]> data) {

		SaveData saveData = PersistenceManager.DeSerialize<SaveData> (data [GetType ().ToString ()]);
		this.elapsedTime = saveData.elapsedTime;
		this.startDate = saveData.startDate;
		this.startDT = saveData.startDT;
	}
}