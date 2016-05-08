using UnityEngine;
using System.Collections;

public class PreloadSong : MonoBehaviour {

	private bool local = true;
//	public string musicPath;
//	public string midiPath;
//	public string thumbnail;
//	public string background;
	public string author;
	public string album;
	public string songName;
	// Use this for initialization

	public TextAsset midi;
	public Texture2D thumbnail;
	public Texture2D background;
	public AudioSource audioSource;

	public override string ToString ()
	{
		return string.Format ("[PreloadSong: local={0}, author={1}, album={2}, songName={3}]", local, author, album, songName);
	}

	void Start(){
		audioSource = gameObject.GetComponent<AudioSource> ();
	}

	public AudioClip getAudioClip(){
		Debug.Log (audioSource);
		return audioSource.clip;
	}

	public MIDI getMIDI(){
		if (midi != null) {
			return new MIDI (midi.bytes);
		} else {
			Debug.LogWarningFormat ("MIDI for {0} not set", songName);
			return new MIDI ((Resources.Load ("MIDI/bike2") as TextAsset).bytes);
		}
	}
		
	public SongMeta getSongMeta(){
		//use songname as path
		return new SongMeta(true, songName, null, null, null, null, author, album);	
	}
}
