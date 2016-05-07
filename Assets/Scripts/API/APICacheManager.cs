using UnityEngine;
using System.Collections;
using LRUCache;
using System.Collections.Generic;
public class APICacheManager : Singleton<APICacheManager> {
	// Use this for initialization
	LRUCache<string, Texture2D> textureCache = new LRUCache<string, Texture2D>(20, Destroy);
	LRUCache<string, AudioClip> audioCache = new LRUCache<string, AudioClip>(5, Destroy);

	HashSet<string> activeAudioDownloads = new HashSet<string>();

	protected override void Init(){
		Debug.Log ("[GameManager] init");
		Persist = true;
	}
		
	public IEnumerator downloadAndCreateTexture(string imagePath, System.Action<Texture2D> callback){
		if (textureCache.contains (imagePath)) {
			Debug.LogFormat ("Cache contains {0}", imagePath);
			callback (textureCache.get(imagePath));
		} else {
			Debug.LogFormat ("Cache does not contain {0}", imagePath);

			yield return API.downloadAndCreateTexture (imagePath, texture => {
				textureCache.add (imagePath, texture);
				callback (texture);
			});
		}
	}

	Dictionary<string, List<System.Action<AudioClip>>> awaitingCallbacks = new Dictionary<string, List<System.Action<AudioClip>>> ();

	public AudioClip localAudio(string audioPath){
		return Resources.Load ("Audio/" + audioPath) as AudioClip;
	}

	//cool function
	//Can call as many times as you want on same audio path
	//Will not cause multiple downloads
	//All callbacks will be executed
	public IEnumerator downloadAudio(string audioPath, System.Action<AudioClip> callback, bool local = false){
		if (!awaitingCallbacks.ContainsKey (audioPath)) {
			awaitingCallbacks [audioPath] = new List<System.Action<AudioClip>> ();
			awaitingCallbacks [audioPath].Add (callback);
		} else {
			awaitingCallbacks [audioPath].Add (callback);
		}
		if (!activeAudioDownloads.Contains (audioPath)) {//don't download again, wait for previous download to finish and execute callback
			if (audioCache.contains (audioPath)) {
//				callback (audioCache.get (audioPath));
//				foreach(var c in awaitingCallbacks[audioPath]){
//					c (audioCache.get (audioPath));
//				}
//				awaitingCallbacks [audioPath].Clear ();
				executeQueuedCallbacks(audioPath, audioCache.get(audioPath));
			} else {
				activeAudioDownloads.Add (audioPath);
				Debug.LogFormat ("Downloading {0}", audioPath);

				if (local) {
//					AudioClip clip = localAudio (audioPath);
//					audioCache.add (audioPath, clip);
//					activeAudioDownloads.Remove(audioPath);
//					executeQueuedCallbacks(audioPath, clip);
					ResourceRequest req = Resources.LoadAsync ("Audio/" + audioPath);
					yield return req;

					AudioClip clip = req.asset as AudioClip;
					activeAudioDownloads.Remove(audioPath);
					executeQueuedCallbacks(audioPath, clip);


				} else {
					yield return API.downloadAudio (audioPath, audioClip => {
						audioCache.add (audioPath, audioClip);
						activeAudioDownloads.Remove(audioPath);

						Debug.LogFormat ("Finished downloading {0}", audioPath);

						executeQueuedCallbacks(audioPath, audioCache.get(audioPath));
						//					callback (audioClip);
					});
				}
			}
		}
	}

//	public IEnumerator downloadAudio(string audioPath){
//		if (!activeAudioDownloads.Contains (audioPath)) {//don't download again, wait for previous download to finish and execute callback
//			activeAudioDownloads.Add (audioPath);
//			yield return API.downloadAudio (audioPath, audioClip => {
//				audioCache.add (audioPath, audioClip);
//				activeAudioDownloads.Remove(audioPath);
//			});
//		}
//	}
//	public IEnumerator downloadMIDI(string midiPath, System.Action<MIDI> callback){
//		return downloadMIDI (midiPath, callback, logError);
//
//	}


	public void executeQueuedCallbacks(string audioPath, AudioClip clip){
		foreach(var c in awaitingCallbacks[audioPath]){
			c (clip);
		}
		awaitingCallbacks [audioPath].Clear ();
	}

}
