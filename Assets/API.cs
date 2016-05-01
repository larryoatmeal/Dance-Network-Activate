using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class API {

	public static string endpoint = "http://dna-sogima.rhcloud.com/";

	void onLoad(List<SongMeta> songMetas){
		foreach (SongMeta meta in songMetas) {
			Debug.Log (meta);
		}
	}

	static IEnumerator publicSongs(System.Action<List<SongMeta>> callback){
		string url = endpoint + "publicsongs";
		WWW www = new WWW(url);
		yield return www;
		Debug.Log(www.text);

		List<SongMeta> metas = SongMeta.parseMultiple (new JSONObject (www.text));
		callback (metas);
	}

	static IEnumerator downloadAudio(string audioPath, System.Action<AudioClip> callback){
		WWW www = new WWW(audioPath);
		yield return www;
		callback (www.GetAudioClip (false, true));
	}

	static IEnumerator downloadImage(string audioPath, System.Action<Texture2D> callback){
		WWW www = new WWW(audioPath);
		yield return www;
		callback (www.texture);
	}

	static IEnumerator downloadAndSetImage(string audioPath, Texture2D texture, System.Action callback){
		WWW www = new WWW(audioPath);
		yield return www;
		www.LoadImageIntoTexture (texture);
		callback ();
	}

	static IEnumerator downloadMIDI(string midiPath, System.Action<MIDI> callback){
		WWW www = new WWW(midiPath);
		yield return www;
		callback (new MIDI(www.bytes));
	}

}
