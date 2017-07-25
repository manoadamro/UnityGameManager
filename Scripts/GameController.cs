using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


/// <summary>
/// Game controller: core.
/// </summary>
public class GameController : MonoBehaviour {

	// singleton
	static GameController instance = null;

	// is the game currently paused?
	[SerializeField] bool paused = false;


	/// <summary>
	/// Called by unity when script is loaded
	/// </summary>
	void Awake() {

		// make sure this is the only instance, error if not
		if (instance == null) { instance = this; }
		else { Debug.LogError ("Only 1 instance of GameController can exist per scene!"); }
	}
		
	// stores callbacks for when game pauses
	event Notification onPauseGame;

	// adds callbacks to onPauseGame
	public static event Notification onPause { add { instance.onPauseGame += value; } remove { instance.onPauseGame -= value; } }

	// stores callbacks for when game resumes
	public event Notification onResumeGame;

	// adds callbacks to onResumeGame
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