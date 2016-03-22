using UnityEngine;
using System.Collections.Generic;
using System;

public class KeyEvent{
	public int keycode;
	public long time; 

	public KeyEvent (int keycode, long time)
	{
		this.keycode = keycode;
		this.time = time;
	}
	
}

public class Game : MonoBehaviourThreading {

	public RealtimeInput inputManager;
	public TimeMaster timeMaster;
	public List<ParticleSystem> particles;
	private Dictionary<int, int> keyCodeToParticleNumber = new Dictionary<int, int>();
	public ScoreCalculator scoreCalculator;


	public RealtimeInputListener listener;
	void printKeyDown(int keyCode, long time){
		
//		Debug.LogFormat ("Keydown {0}", keyCode);
//		Debug.LogFormat ("Time {0}", timeMaster.GetTime ());

		callOnMainThread (new Lauren (particleDisplay, keyCode));
		callOnMainThread (new Lauren (registerKeyDown, new KeyEvent(keyCode, time)));
	}

	void registerKeyDown(object keyEvent){
//		Debug.Log ("Here2");
		KeyEvent keyE = keyEvent as KeyEvent;

		scoreCalculator.processKey (keyE.keycode, keyE.time);
	}

	void particleDisplay(object keycode){//integer
		int key = Convert.ToInt32(keycode);	
//		Debug.LogFormat ("playing particle");

		int particleNum = keyCodeToParticleNumber [key];
		ParticleSystem particle = particles [particleNum];
		particle.gameObject.SetActive(true);
	}
	void turnOffParticle(object keycode){
		int key = Convert.ToInt32(keycode);

		int particleNum = keyCodeToParticleNumber [key];
		ParticleSystem particle = particles [particleNum];
		particle.gameObject.SetActive(false);
	}
		
	void printKeyUp(int keyCode, long time){
//		Debug.LogFormat ("Keyup {0}", keyCode);
		callOnMainThread(new Lauren(turnOffParticle, keyCode));
//		Debug.LogFormat ("Time {0}", timeMaster.GetTime ());
	}

	// Use this for initialization
	void Start () {
		keyCodeToParticleNumber.Add (RealtimeInputListener.A, 0);
		keyCodeToParticleNumber.Add (RealtimeInputListener.S, 1);
		keyCodeToParticleNumber.Add (RealtimeInputListener.D, 2);
		keyCodeToParticleNumber.Add (RealtimeInputListener.F, 3);


		particles.ForEach (p => p.gameObject.SetActive (false));

		inputManager.listener = new RealtimeInputListener (
			new int[] {RealtimeInputListener.A,
				RealtimeInputListener.S,
				RealtimeInputListener.D,
				RealtimeInputListener.F
			}, 
			this.printKeyUp, 
			this.printKeyDown);
	}

	public void MainMenu(){
		Application.LoadLevel ("SongBrowser");
	}







	
//	// Update is called once per frame
//	public override void Update () {
//		base.Update ();
//	}
}
