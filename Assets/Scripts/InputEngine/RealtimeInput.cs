using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;



public class RealtimeInput : MonoBehaviour {

	public bool UseRealTimeInput = true;


	#if UNITY_EDITOR_OSX
		[DllImport("ApplicationServices.framework/ApplicationServices")]
		public static extern long CGEventSourceFlagsState(int keyCode);

		[DllImport("ApplicationServices.framework/ApplicationServices")]
		public static extern bool CGEventSourceKeyState (int stateId, int key); 
	#endif


//	public List<RealtimeInputListener> listeners = new List<RealtimeInputListener>();

	public const int MAX_LISTENERS = 5;
	public int sleepTime = 2;
//	public RealtimeInputListener[] listeners = new RealtimeInputListener[MAX_LISTENERS];
//	private 
////	public RealtimeInputListener listener;
//
////	public int yo;
//
//
//	public void addListener(RealtimeInputListener listener){
//		
//	}
	public RealtimeInputListener listener;
	public TimeMaster timeMaster; 
	public AverageTimer averageTimer;
	public DebugPanel debugPanel;

	bool[] keyStates = new bool[0x80];
	#if UNITY_EDITOR_OSX
	void PollKeysMac(){
		while (UseRealTimeInput) {
			averageTimer.startRecording ();

//			UnityEngine.Debug.Log (timeMaster.GetTime());
//			bool aPressed = CGEventSourceKeyState (1, 0);
			//			UnityEngine.Debug.Log (CGEventSourceKeyState(1, 0));
			//			stopwatch.Reset();
			if (listener != null) {
				for (int i = 0; i < listener.keys.Length; i++) {
					int keyCode = listener.keys [i];
					bool keyPressed = CGEventSourceKeyState (1, keyCode);
					bool prevKeyStatus = keyStates [keyCode];
//					UnityEngine.Debug.Log (keyPressed);
					if (keyPressed && !prevKeyStatus) {//going from false to true
						listener.onKeyDown (keyCode, timeMaster.GetTime());
					} else if (!keyPressed && prevKeyStatus) {
						listener.onKeyUp (keyCode, timeMaster.GetTime());
					}
					keyStates [keyCode] = keyPressed;
				}
			}


			long avg = averageTimer.stopRecording ();
			if (avg != -1) {
//				UnityEngine.Debug.Log(avg);
				if (debugPanel) {
					debugPanel.log("Input", avg.ToString());
				}
//				debugPanel.log ("Input", avg.ToString());
			}
			Thread.Sleep (sleepTime);
		}
	}
	#endif

	// Use this for initialization


	void Start () {
		if (UseRealTimeInput) {
			timeMaster = FindObjectOfType<TimeMaster> ();
			averageTimer = timeMaster.CreateAverageTimer (5000, "input");

			#if UNITY_EDITOR_OSX
			Thread thread = new Thread (new ThreadStart (PollKeysMac));
			thread.Start ();
			#endif
		}
	}

	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR_OSX
		if(!UseRealTimeInput){
//			UnityEngine.Debug.Log("Here");
			foreach(int keyCode in listener.keys){
				KeyCode key = StandardKeyCodes.ToUnityKey(keyCode);
				if(Input.GetKeyDown(key)){
					listener.onKeyDown(keyCode, timeMaster.GetTime());
				}
				if(Input.GetKeyUp(key)){
					listener.onKeyUp(keyCode, timeMaster.GetTime());
				}
			}
		}
		#else
		foreach(int keyCode in listener.keys){
			KeyCode key = StandardKeyCodes.ToUnityKey(keyCode);
			if(Input.GetKeyDown(key)){
			listener.onKeyDown(keyCode, timeMaster.GetTime());
			}
			if(Input.GetKeyUp(key)){
			listener.onKeyUp(keyCode, timeMaster.GetTime());
			}
		}
		#endif
	}
}
