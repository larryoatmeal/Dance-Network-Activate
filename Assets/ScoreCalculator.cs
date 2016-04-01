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


public class ScoreCalculator: MonoBehaviour
{
	public int ScorableThreshold = 3000; 
	public List<MusicEvent> events = new List<MusicEvent>();
	public DebugPanel panel;

	public Calibrator calibrator;
//	public CalibrationScoringModule calibrationScoringModule;
 
	public ScoreCalculatorMode mode = ScoreCalculatorMode.Calibration;

	public void setToCalibrationMode(){
		mode = ScoreCalculatorMode.Calibration;
	}

	public PatternMaster patternMaster;
//	public TimeMaster timeMaster;

	public ScoreCalculator ()
	{
		
	}


	void Awake(){
		Messenger<MusicEvent>.AddListener (MessengerKeys.EVENT_OUT_OF_RANGE, EventOutOfRange);
		Messenger.AddListener (MessengerKeys.EVENT_PATTERN_FINISHED, EventPatternFinished);
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
		if (mode == ScoreCalculatorMode.Calibration) {
			int offset = calibrator.finish ();
		}
	}

	public void addEvent(MusicEvent e){
		events.Add (e);
	}
		
	public void reset(){
		events.Clear ();

		if (mode == ScoreCalculatorMode.Calibration) {
			calibrator.reset ();
		}
	}

	public void processKey(int keycode, long downTime){
		if (patternMaster.started) {
			MusicEventTypes eventType = KeyMappings.keyToEvent (keycode);

			//find first matching event
			int index = events.FindIndex (e => e.eventType == eventType);

			//		Debug.Log (events[0]);

			if (index != -1) {
				MusicEvent e = events[index];
				long expectedTime = patternMaster.absTime (e.startTime);
				long delta = expectedTime - downTime;
				//only consider if within scoring range
				if (delta < ScorableThreshold) {
					Debug.LogFormat ("Delta {0}", delta);

					if (mode == ScoreCalculatorMode.Calibration) {
						calibrator.addDelta ((int)delta);
					}
					events.RemoveAt (index);

					Messenger<MusicEvent>.Invoke (MessengerKeys.EVENT_VANISH, e);
				}
			}
		}
	}

	void Update(){
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



}


