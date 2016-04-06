using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
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
//	public SoundEffect soundEffect;
	public PatternVisualizer patternVisualizer;
	public ScoreCalculator scoreCalculator;
	public LP deltaLp = new LP(100);
//	long startTime = 0;
//	List<MusicEvent> events = new BeatGenerator().quarters();
//	EventIterator iterator;
//	bool started = false;
//	public long startTime;
	private Pattern pattern;
//	public bool started = false;

//	public void PlayDelayed(){
//		long startTime = timeMaster.GetTime () + delay;
//
//		Invoke ("ReadyToPlay", delay/1000f);
//	}
	private bool playing = false;

	public int errorSynchThreshold = 15;

	public bool isPlaying(){
		return playing;
	}

	public void Play(int delay){
		pattern.Play (timeMaster.GetTime () + delay);
		patternVisualizer.reset ();
		scoreCalculator.reset ();
		playing = true;
	}
	public void Pause(){
		playing = false;
	}

	public void Restart(){
		pattern.startTime = timeMaster.GetTime () - rhythm.timeMillis ();
		playing = true;
	}
	void Stop(){
		playing = false;
	}
	// Use this for initialization
	void Start () {
		timeMaster = TimeMaster.Instance;
		debugPanel = DebugPanel.Instance;
		PatternLoader patternLoader = new PatternLoader ();
		pattern = patternLoader.loadPattern (GameManager.Instance.musicFile);
	}
		
	private long lastReportedTime = 0;


	// Update is called once per frame
	void Update () {
		if (playing) {
			int audioReportedTime = rhythm.timeMillis ();

			if (audioReportedTime != lastReportedTime) {
				long error = currentSongTime () - audioReportedTime;
//				Debug.Log (error);
				deltaLp.input ((double)error);
				if (Math.Abs(error) > errorSynchThreshold ) {
					pattern.startTime += error;
					Debug.LogFormat ("Adjusted time by {0}", error);
				}
			}


			debugPanel.log ("MS", audioReportedTime.ToString ());
//			Debug.Log (timeMaster.GetTime());

			if (deltaLp.updateReady ()) {
				debugPanel.log ("Delta", deltaLp.output ().ToString ());
			}

//			Debug.LogFormat ("Delta: {0}", delta);
			//in case things go out of since
//			startTime = timeMaster.GetTime () - time;

			pattern.Process (timeMaster.GetTime (), processEvent);


			lastReportedTime = audioReportedTime;
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
		return songTime + pattern.startTime;
	}

	public long currentSongTime(){
		return timeMaster.GetTime () - pattern.startTime;
	}
}
