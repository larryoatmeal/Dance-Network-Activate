using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

public class NativeTester : MonoBehaviour {

	[DllImport("ApplicationServices.framework/ApplicationServices")]
	public static extern long CGEventSourceFlagsState(int keyCode);
		
	[DllImport("ApplicationServices.framework/ApplicationServices")]
	public static extern bool CGEventSourceKeyState (int stateId, int key); 


	[DllImport("RealtimeKeyboard")]
	public static extern void setup ();

	private Stopwatch stopwatch = new Stopwatch();
	// Use this for initialization
	void Start () {
//		setup ();
		stopwatch.Start();

		Thread thread = new Thread (new ThreadStart (PollKeys));
		thread.Start ();
	}

	void PollKeys(){
		while (true) {
			UnityEngine.Debug.Log (stopwatch.ElapsedMilliseconds);
			bool aPressed = CGEventSourceKeyState (1, 0);
//			UnityEngine.Debug.Log (CGEventSourceKeyState(1, 0));
//			stopwatch.Reset();
			Thread.Sleep (5);
		}
	}

	// Update is called once per frame
	void Update () {
//		bool CapsLock = (CGEventSourceFlagsState (1) & 0x00010000) != 0;


	}
}
