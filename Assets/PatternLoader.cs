using System;
using UnityEngine;
using System.Collections.Generic;
public class PatternLoader
{

	public const string CALIBRATION = "calibration"; 
	public const string HELD_NOTE_TESTER = "heldNotes"; 

	public int numQuartersCalibration = 100;

	public PatternLoader ()
	{
		
	}
	public Pattern loadPattern(MIDI midi){
		return new Pattern (midi.regularMapping (), GameManager.Instance.lookAhead);
	}

//	public Pattern loadPattern(string midifile){
//
//		if (midifile == CALIBRATION) {
//			return new Pattern (new BeatGenerator ().quarters (numQuartersCalibration), GameManager.Instance.lookAhead);
//		} 
//		if (midifile == HELD_NOTE_TESTER) {
//			return new Pattern(MIDI.heldNotesTest(), GameManager.Instance.lookAhead);
//		}
//		TextAsset asset = Resources.Load ("MIDI/" + midifile) as TextAsset;
//		if (asset == null) {
//			Debug.LogWarningFormat ("MIDI file {0} could not be found. Using default instead", midifile);
//			return new Pattern (new BeatGenerator ().quarters (numQuartersCalibration), GameManager.Instance.lookAhead);
//		}
//
//		else {
//			//load file here
//			MIDI midi = new MIDI(midifile);
//			List<MusicEvent> musicEvents = midi.dumbMapping ();
//
////			foreach (MusicEvent m in musicEvents) {
////				Debug.Log (m);
////			}
//
//			return new Pattern (musicEvents, GameManager.Instance.lookAhead);
////			return new Pattern (new BeatGenerator ().quarters (numQuartersCalibration), GameManager.Instance.lookAhead);
//		}
//	}
}


