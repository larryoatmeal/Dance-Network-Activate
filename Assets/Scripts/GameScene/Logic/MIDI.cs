using System;
using UnityEngine;
using NAudio.Midi;
using System.IO;
using System.Linq;
using System.Collections.Generic;
public class MIDI{


	int mMicroSecondsPerQuarterNote;
	int mDeltaTicks;
	IEnumerable<NoteOnEvent> noteOns; 
	IEnumerable<MetaEvent> metaEvents;
	IEnumerable<TempoEvent> tempos;

	public static List<MusicEvent> heldNotesTest(){
		int numNotes = 20;

		List<MusicEvent> events = new List<MusicEvent> ();
		for (int i = 0; i < numNotes; i++) {
			var start = i * 4000;
			var duration = 2000;
			MusicEvent e = new MusicEvent (MusicEventTypes.Down, start, start + duration);
			events.Add (e);
		}
		return events;
	}

	public MIDI(string name){
		TextAsset asset = Resources.Load ("MIDI/" + name) as TextAsset;
		init(asset.bytes);
	}

	private void init(byte[] bytes){
		Stream s = new MemoryStream (bytes);

		MidiFile midiFIle = new MidiFile (s);

		mDeltaTicks = midiFIle.DeltaTicksPerQuarterNote;

		MidiEventCollection events = midiFIle.Events;
		IEnumerable<MidiEvent> flattened = events.SelectMany (i => i);

		noteOns = flattened
			.Where (midiEvent => midiEvent.CommandCode == MidiCommandCode.NoteOn)
			.Select (m => m as NoteOnEvent);

		metaEvents = flattened
			.Where (midiEvent => midiEvent.CommandCode == MidiCommandCode.MetaEvent)
			.Select (m => m as MetaEvent);

		tempos = metaEvents
			.Where (meta => meta.MetaEventType == MetaEventType.SetTempo)
			.Select (m => m as TempoEvent);

		//let's assume one tempo for now
		//		TempoEvent firstTempo;
		if (tempos.Count() > 0) {
			mMicroSecondsPerQuarterNote = tempos.ToList()[0].MicrosecondsPerQuarterNote;
		} else {
			//120 bpm
			//minute / 120 quarter * microseconds/minute
			mMicroSecondsPerQuarterNote = 60000000/120;
		}
	}
		
	public MIDI(byte[] bytes){
		init (bytes);
	}

	public List<MusicEvent> dumbMapping(){
		List<MusicEvent> musicEvents = noteOns.Select (noteOn => {
			long ms = absTimeToMs(noteOn.AbsoluteTime);
			//			long ms = noteOn.AbsoluteTime;
			int noteNumber = noteOn.NoteNumber;
			MusicEventTypes musicEventType;
			if(noteNumber < 30){
				musicEventType = MusicEventTypes.Down;
			}
			else if(noteNumber < 60){
				musicEventType = MusicEventTypes.Up;
			}
			else if(noteNumber < 80){
				musicEventType = MusicEventTypes.Left;
			}
			else{
				musicEventType = MusicEventTypes.Right;
			}


			return new MusicEvent(musicEventType, ms);
		}).ToList();

		//sort by time
		musicEvents.Sort ((first, second) => {
			return (int)(first.startTime - second.startTime);
		});

		//remove duplicates within certain time
		List<MusicEvent> cleandUpEvents = new List<MusicEvent> ();
		const long duplicateThresh = 50;
		MusicEvent prevMusicEvent = musicEvents[0];
		foreach (MusicEvent musicEvent in musicEvents.Skip(1)) {
			if (musicEvent.startTime - prevMusicEvent.startTime < duplicateThresh && musicEvent.eventType == prevMusicEvent.eventType) {
				//don't add
			} else {
				cleandUpEvents.Add (musicEvent);
				prevMusicEvent = musicEvent;
			}
		}

		return cleandUpEvents;
	}

	public List<MusicEvent> regularMapping(){
		List<MusicEvent> musicEvents = noteOns.Select (noteOn => {
			long ms = absTimeToMs(noteOn.AbsoluteTime);
			long msEnd = absTimeToMs(noteOn.OffEvent.AbsoluteTime);

			if(noteOn.NoteNumber == 36){
				return new MusicEvent(MusicEventTypes.Left, ms);
			}
			else if(noteOn.NoteNumber == 37){
				return new MusicEvent(MusicEventTypes.Left, ms, msEnd);
			}
			if(noteOn.NoteNumber == 38){
				return new MusicEvent(MusicEventTypes.Down, ms);
			}
			else if(noteOn.NoteNumber == 39){
				return new MusicEvent(MusicEventTypes.Down, ms, msEnd);
			}
			if(noteOn.NoteNumber == 40){
				return new MusicEvent(MusicEventTypes.Up, ms);
			}
			else if(noteOn.NoteNumber == 41){
				return new MusicEvent(MusicEventTypes.Up, ms, msEnd);
			}
			if(noteOn.NoteNumber == 42){
				return new MusicEvent(MusicEventTypes.Right, ms);
			}
			else if(noteOn.NoteNumber == 43){
				return new MusicEvent(MusicEventTypes.Right, ms, msEnd);
			}else{
				return new MusicEvent(MusicEventTypes.Left, ms);
			}
		}).ToList();

		return musicEvents;
	}
//
//	public List<MusicEvent> processMidi(string name){
//
//		TextAsset asset = Resources.Load ("MIDI/" + name) as TextAsset;
//		Stream s = new MemoryStream (asset.bytes);
//
//		MidiFile midiFIle = new MidiFile (s);
//
//		int deltaTicks = midiFIle.DeltaTicksPerQuarterNote;
//		Debug.LogFormat ("DeltaTicks {0}", deltaTicks);
//
//		MidiEventCollection events = midiFIle.Events;
//
//		IEnumerable<MidiEvent> flattened = events.SelectMany (i => i);
//
//		IEnumerable<NoteOnEvent> noteOns = flattened
//			.Where (midiEvent => midiEvent.CommandCode == MidiCommandCode.NoteOn)
//			.Select (m => m as NoteOnEvent);
//
//		IEnumerable<MetaEvent> metaEvents = flattened
//			.Where (midiEvent => midiEvent.CommandCode == MidiCommandCode.MetaEvent)
//			.Select (m => m as MetaEvent);
//		
//
//		IEnumerable<TempoEvent> tempos = metaEvents
//			.Where (meta => meta.MetaEventType == MetaEventType.SetTempo)
//			.Select (m => m as TempoEvent);
//
//		
//		foreach (NoteOnEvent noteOn in noteOns) {
////			noteOn.CommandCode
////			Debug.Log (noteOn);
//
//		}
//		foreach (TempoEvent tempo in tempos) {
//			//			noteOn.CommandCode
////			Debug.Log (tempo);
//		}
//
//		//let's assume one tempo for now
////		TempoEvent firstTempo;
//
//		int microsecondsPerQuarterNote = 0;
//
//		if (tempos.Count() > 0) {
//			microsecondsPerQuarterNote = tempos.ToList()[0].MicrosecondsPerQuarterNote;
//		} else {
//			//120 bpm
//			//minute / 120 quarter * microseconds/minute
//			microsecondsPerQuarterNote = 60000000/120;
//		}
//			
//		mMicroSecondsPerQuarterNote = microsecondsPerQuarterNote;
//		mDeltaTicks = deltaTicks;
//
//		List<MusicEvent> musicEvents = noteOns.Select (noteOn => {
//			long ms = absTimeToMs(noteOn.AbsoluteTime, microsecondsPerQuarterNote, deltaTicks);
////			long ms = noteOn.AbsoluteTime;
//			int noteNumber = noteOn.NoteNumber;
//
////			int noteMod = noteNumber % 4;
////			MusicEventTypes musicEventType;
////			if(noteMod == 0){
////				musicEventType = MusicEventTypes.Down;
////			}
////			else if(noteMod == 1){
////				musicEventType = MusicEventTypes.Up;
////			}
////			else if(noteMod == 2){
////				musicEventType = MusicEventTypes.Left;
////			}
////			else{
////				musicEventType = MusicEventTypes.Right;
////			}
//			MusicEventTypes musicEventType;
//			if(noteNumber < 30){
//				musicEventType = MusicEventTypes.Down;
//			}
//			else if(noteNumber < 60){
//				musicEventType = MusicEventTypes.Up;
//			}
//			else if(noteNumber < 80){
//				musicEventType = MusicEventTypes.Left;
//			}
//			else{
//				musicEventType = MusicEventTypes.Right;
//			}
//
//
//			return new MusicEvent(musicEventType, ms);
//		}).ToList();
//
//		//sort by time
//		musicEvents.Sort ((first, second) => {
//			return (int)(first.startTime - second.startTime);
//		});
//			
//		//remove duplicates within certain time
//		List<MusicEvent> cleandUpEvents = new List<MusicEvent> ();
//		const long duplicateThresh = 50;
//		MusicEvent prevMusicEvent = musicEvents[0];
//		foreach (MusicEvent musicEvent in musicEvents.Skip(1)) {
//			if (musicEvent.startTime - prevMusicEvent.startTime < duplicateThresh && musicEvent.eventType == prevMusicEvent.eventType) {
//				//don't add
//			} else {
//				cleandUpEvents.Add (musicEvent);
//				prevMusicEvent = musicEvent;
//			}
//		}
//
//		return cleandUpEvents;
//
//	}

	public long absTimeToMs(long absTime, int MSPQ, int DeltaTicks){
		//ticks/quarter * quarter/microseconds * microseconds/ms 
		//ticks * quarter/ticks * microseconds/quarter * ms/microseconds
		return absTime * MSPQ / DeltaTicks / 1000; 
	}

	public long absTimeToMs(long absTime){
		//ticks/quarter * quarter/microseconds * microseconds/ms 
		//ticks * quarter/ticks * microseconds/quarter * ms/microseconds
		return absTimeToMs(absTime, mMicroSecondsPerQuarterNote, mDeltaTicks);
	}
}

