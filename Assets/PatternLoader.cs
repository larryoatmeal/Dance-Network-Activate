using System;
using UnityEngine;
using System.Collections.Generic;
public class PatternLoader
{

	public const string CALIBRATION = "calibration"; 

	public int numQuartersCalibration = 100;

	public PatternLoader ()
	{
		
	}
	public Pattern loadPattern(string midifile){
		if (midifile == CALIBRATION) {
			return new Pattern (new BeatGenerator ().quarters (numQuartersCalibration), GameManager.Instance.lookAhead);
		} else {
			//load file here
			MIDI midi = new MIDI();
			List<MusicEvent> musicEvents = midi.processMidi (midifile);

			foreach (MusicEvent m in musicEvents) {
				Debug.Log (m);
			}

			return new Pattern (musicEvents, GameManager.Instance.lookAhead);
//			return new Pattern (new BeatGenerator ().quarters (numQuartersCalibration), GameManager.Instance.lookAhead);
		}
	}
}


