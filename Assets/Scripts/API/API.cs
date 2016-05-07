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

	public static IEnumerator publicSongs(System.Action<List<SongMeta>> callback){
		string url = endpoint + "publicsongs";
		WWW www = new WWW(url);
		yield return www;
		Debug.Log(www.text);

		List<SongMeta> metas = SongMeta.parseMultiple (new JSONObject (www.text));
		callback (metas);
	}

	public static void logError(string err){
		Debug.LogWarning (err);
	}

	public static IEnumerator downloadAudio(string audioPath, System.Action<AudioClip> callback){
		return downloadAudio (audioPath, callback, logError);
	}

	public static IEnumerator downloadAudio(string audioPath, System.Action<AudioClip> callback, System.Action<string> error){
		WWW www = new WWW(audioPath);
		yield return www;
	
		if (www.error != null)	
			error ("Audio failed to download");
		else{
			try{
				Debug.Log(audioPath);
				AudioClip clip = www.GetAudioClip (false, true, AudioType.OGGVORBIS);
				callback (clip);
			}catch{
				error ("Audio failed to download");
			}
		}
	}

	public static IEnumerator downloadImage(string audioPath, System.Action<Texture2D> callback){
		WWW www = new WWW(audioPath);
		yield return www;
		callback (www.texture);
	}

	public static IEnumerator downloadAndCreateTexture(string audioPath, System.Action<Texture2D> callback){
		WWW www = new WWW(audioPath);
		yield return www;

		Texture2D newTexture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
		www.LoadImageIntoTexture (newTexture);

		www.Dispose();
		www = null;

		callback (newTexture);
	}

	public static IEnumerator downloadMIDI(string midiPath, System.Action<MIDI> callback){
		return downloadMIDI (midiPath, callback, logError);

	}

	public static IEnumerator downloadMIDI(string midiPath, System.Action<MIDI> callback, System.Action<string> error){
		WWW www = new WWW(midiPath);
		yield return www;

		if (www.error != null) {
			error ("Error downloading midi");
		}

		try{
			callback (new MIDI(www.bytes));
		}catch{
			error ("Error parsing midi");
		}

	}


}
