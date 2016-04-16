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
	public List<MusicEvent> events = new List<MusicEvent>();
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


	}
	void OnDisable(){
		Messenger<MusicEvent>.RemoveListener (MessengerKeys.EVENT_OUT_OF_RANGE, EventOutOfRange);
		Messenger.RemoveListener (MessengerKeys.EVENT_PATTERN_FINISHED, EventPatternFinished);
	}

	void EventOutOfRange(MusicEvent e){
//		Debug.Log ("Out of range");
		events.Remove (e);
	}

	void EventPatternFinished(){
		Debug.Log ("Pattern finished");
//		if (mode == ScoreCalculatorMode.Calibration) {
//			int offset = calibrator.finish ();
//		}
	}

	public void addEvent(MusicEvent e){
		events.Add (e);
	}
		
	public void reset(){
		events.Clear ();
//
//		if (mode == ScoreCalculatorMode.Calibration) {
//			calibrator.reset ();
//		}
	}

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
			StandardControls eventType = KeyMappings.keyToControl (keycode);

			//find first matching event
			int index = events.FindIndex (e => actionMatches(eventType, e));
			//		Debug.Log (events[0]);

			if (index != -1) {
				MusicEvent e = events[index];
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
					events.RemoveAt (index);

					Messenger<MusicEvent>.Invoke (MessengerKeys.EVENT_VANISH, e);
				}
			}
		}
	}

	void Update(){
		//if beyond missThreshold, no longer take into account for scoring
		while (events.Count > 0 && patternMaster.currentSongTime() - events [0].startTime > missThreshold) {
			Messenger<ScoreLevels>.Invoke (MessengerKeys.EVENT_SCORE, ScoreLevels.Miss);
			Messenger<MusicEvent>.Invoke (MessengerKeys.EVENT_NO_LONGER_ACTIVE, events[0]);
			events.RemoveAt (0);
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


