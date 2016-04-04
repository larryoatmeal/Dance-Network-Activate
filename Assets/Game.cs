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
//
//	List<StandardKeyCodes> gameKeys = new List<StandardKeyCodes> () {
//		StandardKeyCodes.A,
//		StandardKeyCodes.S,
//		StandardKeyCodes.D,
//		StandardKeyCodes.F
//	};


	public RealtimeInput inputManager;
	public TimeMaster timeMaster;
	public List<ParticleSystem> particles;
	private Dictionary<int, int> keyCodeToParticleNumber = new Dictionary<int, int>();
	public ScoreCalculator scoreCalculator;

//	public RealtimeInputListener listener;
//	void onKeyDown(int keyCode, long time){
//		callOnMainThread (new Lauren (particleDisplay, keyCode));
//		callOnMainThread (new Lauren (registerKeyDown, new KeyEvent(keyCode, time)));
//	}

//	void registerKeyDown(object keyEvent){
//		KeyEvent keyE = keyEvent as KeyEvent;
//		scoreCalculator.processKey (keyE.keycode, keyE.time);
//	}

	void registerKeyDown(KeyEvent keyEvent){
		scoreCalculator.processKey (keyEvent.keycode, keyEvent.time);
	}

//	void particleDisplay(object keycode){//integer
//		int key = Convert.ToInt32(keycode);	
////		Debug.LogFormat ("playing particle");
//		int particleNum = keyCodeToParticleNumber [key];
//		ParticleSystem particle = particles [particleNum];
//		particle.gameObject.SetActive(true);
//	}

	void particleDisplay(int keycode){//integer
//		int key = Convert.ToInt32(keycode);	
		//		Debug.LogFormat ("playing particle");
		int particleNum = keyCodeToParticleNumber [keycode];
		ParticleSystem particle = particles [particleNum];
		particle.gameObject.SetActive(true);
	}


//	void turnOffParticle(object keycode){
//		int key = Convert.ToInt32(keycode);
//
//		int particleNum = keyCodeToParticleNumber [key];
//		ParticleSystem particle = particles [particleNum];
//		particle.gameObject.SetActive(false);
//	}
	void turnOffParticle(int key){
		int particleNum = keyCodeToParticleNumber [key];
		ParticleSystem particle = particles [particleNum];
		particle.gameObject.SetActive(false);
	}
		
//	void onKeyUp(int keyCode, long time){
////		Debug.LogFormat ("Keyup {0}", keyCode);
//		callOnMainThread(new Lauren(turnOffParticle, keyCode));
////		Debug.LogFormat ("Time {0}", timeMaster.GetTime ());
//	}

	// Use this for initialization
	void Start () {
//		keyCodeToParticleNumber.Add (StandardKeyCodes.A, 0);
//		keyCodeToParticleNumber.Add (StandardKeyCodes.S, 1);
//		keyCodeToParticleNumber.Add (StandardKeyCodes.D, 2);
//		keyCodeToParticleNumber.Add (StandardKeyCodes.F, 3);
		particles.ForEach (p => p.gameObject.SetActive (false));

//		inputManager.listener = new RealtimeInputListener (
////			new int[] {RealtimeInputListener.A,
////				RealtimeInputListener.S,
////				RealtimeInputListener.D,
////				RealtimeInputListener.F
////			}, 
//			new int[] {StandardKeyCodes.A,
//								StandardKeyCodes.S,
//								StandardKeyCodes.D,
//								StandardKeyCodes.F
//							}, 
//			this.onKeyUp, 
//			this.onKeyDown);


//		inputManager.trackedKeys = gameKeys;

	}

	void Update(){
//		Debug.Log ("HI");
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
	}

	void OnKeyDown(StandardKeyCodes key, long time){
//		particleDisplay (key);
		Messenger<StandardControls>.Invoke (MessengerKeys.EVENT_PAD_PRESSED, KeyMappings.keyToControl (key));
		scoreCalculator.processKey (key, time);
	}

	void OnKeyUp(StandardKeyCodes key, long time){
//		turnOffParticle (key);
		Messenger<StandardControls>.Invoke (MessengerKeys.EVENT_PAD_RELEASED, KeyMappings.keyToControl (key));
	}

	public void MainMenu(){
		Application.LoadLevel ("SongBrowser");
	}
//	// Update is called once per frame
//	public override void Update () {
//		base.Update ();
//	}
}
