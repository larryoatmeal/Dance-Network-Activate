using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Averager{
	float avg = 0;

	public void tick(float num){
		avg = (num + avg)/2.0f;
	}
}

public class LP {
	double avg = 0.0;

	readonly int interval;
	int counter;

	public LP (int interval)
	{
		this.interval = interval;
	}

	public void input(double n){


		avg = (n + avg) / 2.0;
		counter = (counter + 1)%interval;
	}

	public double output(){
		return avg;
	}

	public bool updateReady(){
		return counter == 0;
	}
}

public class PatternMaster : MonoBehaviour {
	public MainMusic rhythm;
	private TimeMaster timeMaster;
	private DebugPanel debugPanel;//
	public SoundEffect soundEffect;
	public PatternVisualizer patternVisualizer;
	public ScoreCalculator scoreCalculator;
	public LP deltaLp = new LP(100);
//	long startTime = 0;
//	List<MusicEvent> events = new BeatGenerator().quarters();
//	EventIterator iterator;
//	bool started = false;
	public long startTime;
	public Pattern pattern;
	public int numQuarters = 30;
	public bool started = false;

	public void Play(){
		started = true;
		pattern.Play (timeMaster.GetTime ());
		startTime = pattern.startTime;
		patternVisualizer.reset ();
		scoreCalculator.reset ();
	}

	public void Pause(){
		started = false;
	}
		
	public void Restart(){
		startTime = timeMaster.GetTime () - rhythm.timeMillis ();
		pattern.startTime = startTime;
		started = true;
	}
	void Stop(){
		started = false;
		pattern.Stop ();
	}
	// Use this for initialization
	void Start () {
		timeMaster = TimeMaster.Instance;
		debugPanel = DebugPanel.Instance;
		PatternLoader patternLoader = new PatternLoader ();
		pattern = patternLoader.loadPattern (GameManager.Instance.musicFile);
	}
		
	// Update is called once per frame
	void Update () {
		if (started) {
			int time = rhythm.timeMillis ();

			long adjustedStartTime = timeMaster.GetTime () - time;

			long delta = adjustedStartTime - pattern.startTime;
			deltaLp.input ((double)delta);

			float timeSeconds = time / 1000f;
			debugPanel.log ("MS", time.ToString ());
//			Debug.Log (timeMaster.GetTime());

			if (deltaLp.updateReady ()) {
				debugPanel.log ("Delta", deltaLp.output ().ToString ());
			}

//			Debug.LogFormat ("Delta: {0}", delta);
			//in case things go out of since
//			startTime = timeMaster.GetTime () - time;

			pattern.Process (timeMaster.GetTime (), processEvent);
//			if (pattern.isFinished ()) {
//				Messenger.Invoke (MessengerKeys.EVENT_PATTERN_FINISHED);
//				Stop ();
//			} else {
//			}
		}
	}

	void processEvent(MusicEvent e){
//		debugPanel.log ("Event", e.ToString());
		var delta = e.startTime + pattern.startTime -  timeMaster.GetTime ();
//		Debug.Log (delta/1000f);
//		soundEffect.PlayScheduled (delta / 1000f);
		if (e.eventType == MusicEventTypes.End) {
			Stop ();
			Messenger.Invoke (MessengerKeys.EVENT_PATTERN_FINISHED);
		} else {
			scoreCalculator.addEvent (e);
			patternVisualizer.addEvent (e);
		}
	}

	public long absTime(long songTime){
		return songTime + startTime;
	}

	public long currentSongTime(){
		return timeMaster.GetTime () - startTime;
	}
}
