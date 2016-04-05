using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class CalibratorV2 : MonoBehaviour {

	private long startTime = 0;

	public double BPM = 120;

	public int maxSubsPerBeat = 2;

	public int minimumSamples = 10;
	public int minimumSD = 20;
	public int windowSize = 10;
	public int windowSum = 0;
//	public int windowSumSquared = 0;

	public DebugPanel debugPanel;

	public List<int> offsets = new List<int> ();

	public MainMusic mainMusic;
	public TimeMaster timeMaster;

	public long prevTime;

//	void Awake(){
//		Application.targetFrameRate = 1;
//		QualitySettings.vSyncCount = 0;
//	}
//

	// Use this for initialization
	void Start () {
		debugPanel = DebugPanel.Instance;
		timeMaster = TimeMaster.Instance;
	}

	int quantizationInterval(){
		return (int) (60000 / BPM / maxSubsPerBeat);
	}
		
	// Update is called once per frame
	void Update () {
		long musicTime = mainMusic.timeMillis ();
		if (prevTime != musicTime) {
			prevTime = musicTime;
			long songTime = timeMaster.GetTime() - startTime;
			long error = songTime - musicTime;

			startTime += error / 2;

			debugPanel.log ("Drift", error.ToString());
		}
	}

	public void Play(long time){
		startTime = time;
	}

	public void KeyDown(long time){
		long songTime = time - startTime;
		int quantInterval = quantizationInterval ();
//		Debug.Log (quantInterval);
		int drag = (int)(songTime % quantInterval);
		int rush = quantInterval - drag;

		int offset = Mathf.Min (drag, rush);

		debugPanel.log ("Offset", offset.ToString());

		Debug.Log (offset);
		offsets.Add (offset);
		windowSum += offset;
//		windowSumSquared += offset;

		if (offsets.Count > windowSize) {
			windowSum -= offsets [offsets.Count - windowSize];
			int average = windowSum / windowSize;
			int sd = SD (offsets, average, windowSize);
			debugPanel.log ("Avg", average.ToString());
			debugPanel.log ("SD", sd.ToString());

			int err = offset - average;

			debugPanel.log ("Adjusted error", err.ToString ());
		}

	}

	public int SD(List<int> numbers, int avg, int window){
		return (int) Mathf.Sqrt (
			numbers
			.Skip(numbers.Count() - window)
			.Select (n => Mathf.Pow (n - avg, 2))
			.Average ());
	}
	public int Avg(List<int> numbers, int window){
		if (numbers.Count > window) {
			return (int)numbers
				.Skip (numbers.Count () - window)
				.Average ();
		} else {
			if (numbers.Count > 0) {
				return (int) numbers.Average ();
			} else {
				return 0;
			}
		}
	}

	public void finish(){
		int offset = Avg(offsets, windowSize);
		debugPanel.log("Final offset", offset.ToString());
		PlayerPrefs.SetInt(PlayerPrefKeys.AudioLatencyOffset, offset);
	}
}
