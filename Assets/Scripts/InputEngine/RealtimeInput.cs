using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;



public class RealtimeInput : MonoBehaviour {
	[DllImport("ApplicationServices.framework/ApplicationServices")]
	public static extern long CGEventSourceFlagsState(int keyCode);

	[DllImport("ApplicationServices.framework/ApplicationServices")]
	public static extern bool CGEventSourceKeyState (int stateId, int key); 

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

	void PollKeys(){
		while (true) {
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

	// Use this for initialization
	void Start () {
		timeMaster = FindObjectOfType<TimeMaster> ();
//		debugPanel = FindObjectOfType<DebugPanel> ();
		averageTimer = timeMaster.CreateAverageTimer (5000, "input");


		Thread thread = new Thread (new ThreadStart (PollKeys));
		thread.Start ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
