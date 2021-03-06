﻿using System;
using System.Collections.Generic;
public class BeatGenerator{

	private double bpm = 120.0; 

	public List<MusicEvent> quarters(int numQuarters = 20){
		List<MusicEvent> events = new List<MusicEvent> ();
//		int numQuarters = 20;
		int offset = 4;
		for (int i = 0; i < numQuarters; i++) {
			long time = (i+offset) * bpmToMillisecondsPerBeat (bpm);

			int rand = UnityEngine.Random.Range (0, 4);
			MusicEventTypes type = MusicEventTypes.Down;
			if (rand == 0) {
				type = MusicEventTypes.Down;
			}
			else if (rand == 1){
				type = MusicEventTypes.Left;
			}
			else if (rand == 2){
				type = MusicEventTypes.Right;
			}
			else if (rand == 3){
				type = MusicEventTypes.Up;
			}

			//set all to down for now
			MusicEvent musicEvent = new MusicEvent (MusicEventTypes.Down, time);
			events.Add (musicEvent);
		}

		return events;
	}

	private long bpmToMillisecondsPerBeat(double bpm){
		int millisecondsPerBeat = 60 * 1000;
		return (long)(millisecondsPerBeat / bpm);
	}

}