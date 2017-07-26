using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class GameController : MonoBehaviour {

	/// <summary>
	/// singleton
	/// </summary>
	static GameController instance = null;

	/// <summary>
	/// is the game currently paused?
	/// </summary>
	[SerializeField] bool paused = false;


	/// <summary>
	/// Called by unity when script is loaded
	/// </summary>
	void Awake() {

		// make sure this is the only instance, error if not
		if (instance == null) { instance = this; }
		else { Debug.LogError ("Only 1 instance of GameController can exist per scene!"); }
	}
		
	/// <summary>
	/// stores callbacks for when game pauses
	/// </summary>
	event Notification onPauseGame;

	/// <summary>
	/// adds callbacks to onPauseGame
	/// </summary>
	public static event Notification OnPause { add { instance.onPauseGame += value; } remove { instance.onPauseGame -= value; } }

	/// <summary>
	/// stores callbacks for when game resumes
	/// </summary>
	public event Notification onResumeGame;

	/// <summary>
	/// adds callbacks to onResumeGame
	/// </summary>
	public static event Notification OnResume { add { instance.onResumeGame += value; } remove { instance.onResumeGame -= value; } }


	/// <summary>
	/// Pauses the game.
	/// </summary>
	public static void PauseGame() { instance._PauseGame (); }
	public void _PauseGame() {

		// make sure the game is not already paused
		if (!paused) {

			// pause the game
			paused = true;

			// call the callbacks
			if (onPauseGame != null) { onPauseGame (); }
		}
	}

	/// <summary>
	/// Resumes the game.
	/// </summary>
	public static void ResumeGame() { instance._ResumeGame (); }
	public void _ResumeGame() {

		// make sure the game is paused
		if (paused) {

			// resume
			paused = false;

			// call the callbacks
			if (onResumeGame != null) { onResumeGame (); }
		}
	}
}