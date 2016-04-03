using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class AverageTimer{
	long runningAverage = 0;
	long startTime;

	long lastReportTime = 0;
	readonly int reportInterval;
	TimeMaster timeMaster;
	DebugPanel debugPanel;
	string name;
	int cycles = 0;

	public AverageTimer(TimeMaster timeMaster, int reportInterval, string name){
		this.timeMaster = timeMaster;
		this.reportInterval = reportInterval;
		this.name = name;
		this.lastReportTime = timeMaster.GetTime ();
	}

	public void startRecording(){
		startTime = timeMaster.GetTime ();
	}
	//return average or -1 if not ready yet
	public long stopRecording(){
		long time = timeMaster.GetTime ();
		cycles += 1;
//		UnityEngine.Debug.Log (cycles);
		long totalElapsed = time - lastReportTime;
		if (totalElapsed > reportInterval) {
			long average = totalElapsed / cycles;
//			UnityEngine.Debug.LogFormat ("{0}: {1}", name, average);
			lastReportTime = time;
			cycles = 0;
			return average;
//			runningAverage = 0;
		}
		return -1;
//		long elapsed = time - startTime;
//		UnityEngine.Debug.Log (elapsed);
//		if (runningAverage == 0) {
//			runningAverage = elapsed;
//		} else {
//			runningAverage = (runningAverage + elapsed)/2;
//		}
//		if (time - lastReportTime > reportInterval) {
//			UnityEngine.Debug.LogFormat ("{0}: {1}", name, runningAverage);
//			lastReportTime = time;
//			runningAverage = 0;
//		}

	}
}

public class TimeMaster : MonoBehaviour {
	
	Stopwatch stopwatch = new Stopwatch();
	// Use this for initialization
	void Start () {
		stopwatch.Start ();
	}

	public AverageTimer CreateAverageTimer(int interval, string name){
		return new AverageTimer (this, interval, name);
	}

	public long GetTime(){
		return stopwatch.ElapsedMilliseconds;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
