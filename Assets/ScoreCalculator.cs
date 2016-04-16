using System;
using System.Collections.Generic;
using UnityEngine;



public interface ScoringModule<T>{
	void start();
	void addEvent(MusicEvent musicEvent, int delta);
	T finish();
}

//public class CalibrationScoringModule : ScoringModule<int>{
//	Calibrator calibrator;
//
//	List<int> deltas = new List<int> ();
//	public CalibrationScoringModule(Calibrator calibrator){
//		this.calibrator = calibrator;
//
//	}
//	public void start(){
//		deltas.Clear ();
//	}
//	public void addEvent (MusicEvent musicEvent, int delta)
//	{
//		deltas.Add (delta);
//	}
//	public int finish ()
//	{
//		return (int) calibrator.calculateOffset (deltas);
//	}
//}

public enum ScoreCalculatorMode{
	Calibration
}

public enum ScoreLevels{
	GoodShit,
	Perfect,
	Great,
	Good,
	Bad,
	Miss
}




public class ScoreCalculator: MonoBehaviour
{
	public int ScorableThreshold = 3000; 
//	public List<MusicEvent> events = new List<MusicEvent>();

	public Dictionary<MusicEventTypes, List<MusicEvent>> events;


	public DebugPanel panel;

	private int latencyAdjustment;

	public int goodShit = 20;
	public int perfect = 40;
	public int great = 60;
	public int good = 120;
	public int bad = 200;

	public string GoodShitString = "GoodShit";
	public string PerfectString = "Perfect";
	public string GreatString = "Great";
	public string GoodString = "Okay";
	public string BadString = "Coffin";


	public int missThreshold = 50;
	public Dictionary<ScoreLevels, string> scoreToString;


//	public Calibrator calibrator;
//	public CalibrationScoringModule calibrationScoringModule;
 
//	public ScoreCalculatorMode mode = ScoreCalculatorMode.Calibration;

//	public void setToCalibrationMode(){
//		mode = ScoreCalculatorMode.Calibration;
//	}

	public PatternMaster patternMaster;
//	public TimeMaster timeMaster;

//	public ScoreCalculator ()
//	{
//		
//		
//	}

	void Awake(){
		Messenger<MusicEvent>.AddListener (MessengerKeys.EVENT_OUT_OF_RANGE, EventOutOfRange);
		Messenger.AddListener (MessengerKeys.EVENT_PATTERN_FINISHED, EventPatternFinished);

		latencyAdjustment = PlayerPrefs.GetInt (PlayerPrefKeys.AudioLatencyOffset);
		Debug.LogFormat ("Using latency settings {0}", latencyAdjustment);
		DebugPanel.Instance.log ("Latency adj", latencyAdjustment.ToString());
		scoreToString = new Dictionary<ScoreLevels, string>(){
			{ScoreLevels.GoodShit, GoodShitString},
			{ScoreLevels.Perfect, PerfectString},
			{ScoreLevels.Great, GreatString},
			{ScoreLevels.Good, GoodString},
			{ScoreLevels.Bad, BadString}
		};
		events = new Dictionary<MusicEventTypes, List<MusicEvent>>(){
			{MusicEventTypes.Down, new List<MusicEvent>()},
			{MusicEventTypes.Up, new List<MusicEvent>()},
			{MusicEventTypes.Left, new List<MusicEvent>()},
			{MusicEventTypes.Right, new List<MusicEvent>()}
		};


	}
	void OnDisable(){
		Messenger<MusicEvent>.RemoveListener (MessengerKeys.EVENT_OUT_OF_RANGE, EventOutOfRange);
		Messenger.RemoveListener (MessengerKeys.EVENT_PATTERN_FINISHED, EventPatternFinished);
	}

	void EventOutOfRange(MusicEvent e){
//		Debug.Log ("Out of range");
		events[e.eventType].Remove(e);


//		events.Remove (e);
	}

	void EventPatternFinished(){
		Debug.Log ("Pattern finished");
//		if (mode == ScoreCalculatorMode.Calibration) {
//			int offset = calibrator.finish ();
//		}
	}

	public void addEvent(MusicEvent e){
//		Debug.Log (events[MusicEventTypes.Left]);
		Debug.Log (events.Count);

		events[e.eventType].Add(e);

//		events.Add (e);
	}
		
	public void reset(){

		foreach (var eventList in events.Values) {
			eventList.Clear ();
		}
//
//		if (mode == ScoreCalculatorMode.Calibration) {
//			calibrator.reset ();
//		}
	}

	Dictionary<StandardControls, MusicEventTypes> controlToEventType = new Dictionary<StandardControls, MusicEventTypes>(){
		{StandardControls.DOWN, MusicEventTypes.Down},
		{StandardControls.UP, MusicEventTypes.Up},
		{StandardControls.LEFT, MusicEventTypes.Left},
		{StandardControls.RIGHT, MusicEventTypes.Right},
	};

	bool actionMatches(StandardControls control, MusicEvent e){
		if (control == StandardControls.DOWN
		   && e.eventType == MusicEventTypes.Down) {
			return true;
		}
		if (control == StandardControls.UP
			&& e.eventType == MusicEventTypes.Up) {
			return true;
		}
		if (control == StandardControls.LEFT
			&& e.eventType == MusicEventTypes.Left) {
			return true;
		}
		if (control == StandardControls.RIGHT
		    && e.eventType == MusicEventTypes.Right) {
			return true;
		}
		return false;
	}

	public void processKey(StandardKeyCodes keycode, long downTime){
		if (patternMaster.isPlaying()) {
			StandardControls control = KeyMappings.keyToControl (keycode);
			MusicEventTypes musicEventType = controlToEventType [control];

			//find first matching event
//			int index = events[eventType].FindIndex (e => actionMatches(eventType, e));
			//		Debug.Log (events[0]);
			if (events [musicEventType].Count > 0) {
				MusicEvent e = events[musicEventType][0];
				long expectedTime = patternMaster.absTime (e.startTime);
				long delta = expectedTime - downTime;


				long adjustedError = delta + latencyAdjustment;


				//only consider if within scoring range
				if (delta < ScorableThreshold) {
					Debug.LogFormat ("Delta {0}", delta);
					Debug.LogFormat ("Adjusted delta {0}", adjustedError);

					ScoreLevels score = ReportQuality (adjustedError);

					Messenger<ScoreLevels>.Invoke (MessengerKeys.EVENT_SCORE, score);
					panel.log ("Score", scoreToString[score]);

					//					if (mode == ScoreCalculatorMode.Calibration) {
					//						calibrator.addDelta ((int)delta);
					//					}
					events[musicEventType].RemoveAt (0);

					Messenger<MusicEvent>.Invoke (MessengerKeys.EVENT_VANISH, e);
				}

			}

		}
	}

	void Update(){
		//if beyond missThreshold, no longer take into account for scoring
		foreach (List<MusicEvent> eventList in events.Values) {
			while (eventList.Count > 0 && patternMaster.currentSongTime() - eventList [0].startTime > missThreshold) {
				Messenger<ScoreLevels>.Invoke (MessengerKeys.EVENT_SCORE, ScoreLevels.Miss);
				Messenger<MusicEvent>.Invoke (MessengerKeys.EVENT_NO_LONGER_ACTIVE, eventList[0]);
				eventList.RemoveAt (0);
			}
		}


//		int i = 0;
//		while (i < events.Count && events [i].startTime + 700 < patternMaster.currentSongTime ()) {
////			Debug.LogFormat ("Removing {0}", events [i]);
//			i++;
//		}
//		for (int j = 0; j < i; j++) {
////			Messenger<MusicEvent>.Invoke (MessengerKeys.EVENT_VANISH, events[i]);
//			events.RemoveAt (0);
//		}
	}

//	string ReportQualityString(long delta){
//		return scoreToString [ReportQuality2 (delta)];
//	}
	ScoreLevels ReportQuality(long delta){
		int error = Math.Abs ((int)delta);

		if (error < goodShit) {
			return ScoreLevels.GoodShit;
		} else if (error < perfect) {
			return ScoreLevels.Perfect;
		} else if (error < great) {
			return ScoreLevels.Great;
		} else if (error < good) {
			return ScoreLevels.Good;
		} else{
			return ScoreLevels.Bad;
		} 
	}




}


