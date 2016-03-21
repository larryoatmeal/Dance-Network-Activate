using System;
using System.Collections.Generic;
using UnityEngine;


public class ScoreModule{


}
public class ScoreCalculator: MonoBehaviour
{
	public int ScorableThreshold = 3000; 
	public List<MusicEvent> events = new List<MusicEvent>();
	public DebugPanel panel;


	public PatternMaster patternMaster;
//	public TimeMaster timeMaster;

	public ScoreCalculator ()
	{

	}

	public void addEvent(MusicEvent e){
		events.Add (e);
	}
		
	public void reset(){
		events.Clear ();
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

					events.RemoveAt (index);

					Messenger<MusicEvent>.Invoke (MessengerKeys.EVENT_VANISH, e);
				}
			}
		}
	}

	void Update(){


		int i = 0;
		while (i < events.Count && events [i].startTime + 1000 < patternMaster.currentSongTime ()) {
//			Debug.LogFormat ("Removing {0}", events [i]);
			i++;
		}
		for (int j = 0; j < i; j++) {
			events.RemoveAt (0);
		}
	}



}


