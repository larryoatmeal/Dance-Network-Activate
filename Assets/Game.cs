using UnityEngine;
using System.Collections.Generic;
using System;

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
	public List<GameObject> menuItems;


	public float preStart = 1.0f;
	int preStartMS;

	public void BeginGame(){
		music.playFromBeginning (preStart);
		patternMaster.Play (preStartMS);
	}

	public void PauseGame(){
		music.pause ();
		patternMaster.Pause ();
	}
	public void RestartGame(){
		music.play ();
		patternMaster.Restart ();
	}

	// Use this for initialization
	void Start () {
		preStartMS = (int)(preStart * 1000);
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
			
		if (Input.GetKeyDown (KeyCode.W)) {
			foreach(GameObject menuItem in menuItems) {
				menuItem.SetActive (!menuItem.activeSelf);
			}
		};




	}

	void OnKeyDown(StandardKeyCodes key, long time){
		Messenger<StandardControls>.Invoke (MessengerKeys.EVENT_PAD_PRESSED, KeyMappings.keyToControl (key));
		scoreCalculator.processKey (key, time, true);
	}

	void OnKeyUp(StandardKeyCodes key, long time){
		Messenger<StandardControls>.Invoke (MessengerKeys.EVENT_PAD_RELEASED, KeyMappings.keyToControl (key));
		scoreCalculator.processKey (key, time, false);

	}

	public void MainMenu(){
		Application.LoadLevel ("SongBrowser");
	}

}
