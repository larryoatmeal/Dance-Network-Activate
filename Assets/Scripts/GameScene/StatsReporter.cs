using UnityEngine;
using System.Collections;

public class StatsReporter : MonoBehaviour {
//	AverageTimer timer = new AverageTimer(TimeMaster.Instance, 2000, "FPS");

	Averager averager = new Averager(10);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float fps = 1.0f/Time.deltaTime;
		averager.tick (fps);

		if (averager.reportReady ()) {
			DebugPanel.Instance.log ("FPS", averager.output ());
		}


	}
}
