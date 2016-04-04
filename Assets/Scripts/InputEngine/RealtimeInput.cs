using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;


//Dependencies
//

public class RealtimeInput : MonoBehaviour {

	public List<StandardKeyCodes> trackedKeys = new List<StandardKeyCodes> () {
		StandardKeyCodes.A,
		StandardKeyCodes.S,
		StandardKeyCodes.D,
		StandardKeyCodes.F

	};

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

	public Dictionary<StandardKeyCodes, long> keyUps = new Dictionary<StandardKeyCodes, long> ();
	public Dictionary<StandardKeyCodes, long> keyDowns = new Dictionary<StandardKeyCodes, long>();
	public Dictionary<int, bool> keyCurrentlyPressed = new Dictionary<int, bool>();

	public RealtimeInputListener listener;
	private TimeMaster timeMaster; 
	public AverageTimer averageTimer;
	private DebugPanel debugPanel;

	bool[] keyStates = new bool[0x80];
	#if UNITY_EDITOR_OSX
	void PollKeysMac(){
		while (UseRealTimeInput) {
			averageTimer.startRecording ();

//			UnityEngine.Debug.Log (timeMaster.GetTime());
//			bool aPressed = CGEventSourceKeyState (1, 0);
			//			UnityEngine.Debug.Log (CGEventSourceKeyState(1, 0));
			//			stopwatch.Reset();
//			if (listener != null) {
			for (int i = 0; i < trackedKeys.Count; i++) {
				StandardKeyCodes keyCodeStandard = trackedKeys [i];
				int keyCode = KeyConverter.ToMacNative (keyCodeStandard);
				bool keyPressed = CGEventSourceKeyState (1, keyCode);
				bool prevKeyStatus = keyStates [keyCode];
//					UnityEngine.Debug.Log (keyPressed);
				if (keyPressed && !prevKeyStatus) {//going from false to true
//						listener.onKeyDown (keyCode, timeMaster.GetTime());
				keyDowns[keyCodeStandard] = timeMaster.GetTime();
				} else if (!keyPressed && prevKeyStatus) {
//						listener.onKeyUp (keyCode, timeMaster.GetTime());
				keyUps[keyCodeStandard] = timeMaster.GetTime();
				}
				keyStates [keyCode] = keyPressed;
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
		debugPanel = DebugPanel.Instance;
		timeMaster = TimeMaster.Instance;

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
		keyUps.Clear ();
		keyDowns.Clear ();

		#if UNITY_EDITOR_OSX
		if(!UseRealTimeInput){
//			UnityEngine.Debug.Log("Here");
			foreach(StandardKeyCodes keyCode in trackedKeys){
				KeyCode key = KeyConverter.ToUnityKey(keyCode);
				if(Input.GetKeyDown(key)){
					long time = timeMaster.GetTime();
//					listener.onKeyDown(keyCode, time);
					keyDowns[keyCode] = time;
				}
				if(Input.GetKeyUp(key)){
					long time = timeMaster.GetTime();
//					listener.onKeyUp(keyCode, time);
					keyUps [keyCode] = time;
				}
			}
		}
		#else
		foreach(StandardKeyCodes keyCode in trackedKeys){
		KeyCode key = KeyConverter.ToUnityKey(keyCode);
		if(Input.GetKeyDown(key)){
		long time = timeMaster.GetTime();
		//					listener.onKeyDown(keyCode, time);
		keyDowns[keyCode] = time;
		}
		if(Input.GetKeyUp(key)){
		long time = timeMaster.GetTime();
		//					listener.onKeyUp(keyCode, time);
		keyUps [keyCode] = time;
		}
		}
		#endif


	}

	//Polling. Returns -1 if none
	public long GetKeyDown(StandardKeyCodes code){
		if (keyDowns.ContainsKey (code)) {
			return keyDowns [code];
		}else{
			return -1;
		}
	}
	public long GetKeyUp(StandardKeyCodes code){
		if (keyUps.ContainsKey (code)) {
			return keyUps [code];
		}else{
			return -1;
		}
	}
}
