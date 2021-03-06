﻿using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class KeyEvent{
	public StandardKeyCodes keycode;
	public long time; 

	public KeyEvent (StandardKeyCodes keycode, long time)
	{
		this.keycode = keycode;
		this.time = time;
	}


	
}

public class Game : MonoBehaviourThreading {

	List<StandardControls> controls = new List<StandardControls> () {
		StandardControls.DOWN,
		StandardControls.UP,
		StandardControls.LEFT,
		StandardControls.RIGHT
	};

	public RealtimeInput inputManager;
	public TimeMaster timeMaster;
	public ScoreCalculator scoreCalculator;
	public PatternMaster patternMaster;
	public MainMusic music;
	public MobileInput mobileInput;
//	public List<GameObject> menuItems;


	public float preStart = 1.0f;
	int preStartMS;

	public void BeginGame(){
		music.playFromBeginning (preStart);
		patternMaster.Play (preStartMS);

		Messenger.AddListener (MessengerKeys.TOGGLE_MENU, MenuToggled);
	}
	
	public void MenuToggled(){
		if (patternMaster.isPlaying ()) {
			PauseGame ();
		} else if(patternMaster.isPaused()) {
			ResumeGame ();
		}
	}
		
	public void PauseGame(){
		music.pause ();
		patternMaster.Pause ();
	}
	public void ResumeGame(){
		music.play ();
		patternMaster.Restart ();
	}



	// Use this for initialization
	void Start () {
		preStartMS = (int)(preStart * 1000);

//		SongMeta song = GameManager.Instance.currentSong;
//		StartCoroutine (API.downloadAudio (song.musicPath, audioclip => {
//			Debug.Log("Finished downloading audio");
//			StartCoroutine (API.downloadMIDI (song.midiPath, midi => {
//				Debug.Log("Finished downloading midi");
//			}));
//
//		}));
	
		//do not immediately start playing if first time
//		BeginGame ();
//
//		if (GameManager.Instance.firstTime ()) {
//			GameManager.Instance.setNotFirstTime ();
//		} else {
//		}
	}





	void Update(){
		foreach (StandardControls control in controls) {
			StandardKeyCodes key = KeyMappings.controlToKey (control);

			long downTime = inputManager.GetKeyDown (key);
			if (downTime >= 0) {
				OnKeyDown (key, downTime);
			}
			long upTime = inputManager.GetKeyUp (key);
			if (upTime >= 0) {
				OnKeyUp (key, upTime);
			}
		}


		#if UNITY_IOS 
		//mobile only
		foreach (StandardControls control in controls) {
			StandardKeyCodes key = KeyMappings.controlToKey (control);

			long downTime = mobileInput.keyDown (key);
			if (downTime >= 0) {
				OnKeyDown (key, downTime);
			}
			long upTime = mobileInput.keyUp (key);
			if (upTime >= 0) {
				OnKeyUp (key, upTime);
			}
		}
		#endif
			

		//WARN: This code is far more complex then it has to be
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (!patternMaster.isPlaying()) {

				//order matters here. Once invoke is called, paused may be modified 
				bool paused = patternMaster.isPaused ();

				Messenger.Invoke (MessengerKeys.TOGGLE_MENU);

				if (paused) {
					ResumeGame ();
				} else {
					BeginGame ();
				}
			}
		};



	

	}

	public void OnKeyDown(StandardKeyCodes key, long time){
		Messenger<StandardControls>.Invoke (MessengerKeys.EVENT_PAD_PRESSED, KeyMappings.keyToControl (key));
		scoreCalculator.processKey (key, time, true);
	}

	public void OnKeyUp(StandardKeyCodes key, long time){
		Messenger<StandardControls>.Invoke (MessengerKeys.EVENT_PAD_RELEASED, KeyMappings.keyToControl (key));
		scoreCalculator.processKey (key, time, false);

	}

	public void OnDestroy(){
		Messenger.RemoveListener (MessengerKeys.TOGGLE_MENU, MenuToggled);
	}

	public void MainMenu(){
		SceneManager.LoadScene ("SongBrowser");
	}

}
