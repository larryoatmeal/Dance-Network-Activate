﻿using UnityEngine;
using System.Collections;
using LRUCache;
using System.Collections.Generic;

public class ImageDownloader : RerunnableOP<string, Texture2D>{

	public ImageDownloader (LRUCache<string, Texture2D> cache, MonoBehaviour owner) : base (cache, owner)
	{
	}

	protected override IEnumerator download (string imagePath, System.Action<Texture2D> callback)
	{
//		throw new System.NotImplementedException ();
		yield return API.downloadAndCreateTexture (imagePath, texture => {
			callback (texture);
		});
	}
}

public class AudioDownloader : RerunnableOP<string, AudioClip>{

	public AudioDownloader (LRUCache<string, AudioClip> cache, MonoBehaviour owner) : base (cache, owner)
	{
	}

	protected override IEnumerator download (string audioPath, System.Action<AudioClip> callback)
	{
		//		throw new System.NotImplementedException ();
		yield return API.downloadAudio (audioPath, audioClip => {
			Debug.LogFormat ("Finished downloading {0}", audioPath);
			callback(audioClip);
			//					callback (audioClip);
		});
	}
}



	
public class APICacheManager : Singleton<APICacheManager> {
	// Use this for initialization
	LRUCache<string, Texture2D> textureCache = new LRUCache<string, Texture2D>(20, Destroy);
	LRUCache<string, AudioClip> audioCache = new LRUCache<string, AudioClip>(15, Destroy);

	List<SongMeta> songsCache = new List<SongMeta> ();

	ImageDownloader imageDownloader; 
	AudioDownloader audioDownloader;

//	HashSet<string> activeAudioDownloads = new HashSet<string>();
//	HashSet<string> activeAudioDownloads = new HashSet<string>();

	protected override void Init(){
		Debug.Log ("[GameManager] init");
		Persist = true;
		imageDownloader = new ImageDownloader (textureCache, this);
		audioDownloader = new AudioDownloader (audioCache, this);
	}
		
	public void downloadTexture(string imgpath, System.Action<Texture2D> callback){
		imageDownloader.run (imgpath, callback);
	}

	public void downloadAudio(string audioPath, System.Action<AudioClip> callback){
		audioDownloader.run (audioPath, callback);
	}

	public IEnumerator publicSongs(System.Action<List<SongMeta>> callback){
		if (songsCache.Count > 0) {
			callback (songsCache);
		} else {
			yield return API.publicSongs (callback);
		}
	}



//	public IEnumerator downloadAndCreateTexture(string imagePath, bool local, System.Action<Texture2D> callback){
////		if (textureCache.contains (imagePath)) {
////			Debug.LogFormat ("Cache contains {0}", imagePath);
////			callback (textureCache.get(imagePath));
////		} else {
////			Debug.LogFormat ("Cache does not contain {0}", imagePath);
////			if (local) {
////				ResourceRequest req = Resources.LoadAsync ("Thumbnails/" + imagePath);
////				yield return req;
////
////				//need error checking here
////				Texture2D texture = Instantiate(req.asset) as Texture2D;
////				textureCache.add(imagePath, req.asset as Texture2D);
////				callback (texture);
////			} else {
////				yield return API.downloadAndCreateTexture (imagePath, texture => {
////					textureCache.add (imagePath, texture);
////					callback (texture);
////				});
////			}
////		}
////		imageDownloader.ru
//	}

//	Dictionary<string, List<System.Action<AudioClip>>> awaitingCallbacks = new Dictionary<string, List<System.Action<AudioClip>>> ();

//	public AudioClip localAudio(string audioPath){
//		return Resources.Load ("Audio/" + audioPath) as AudioClip;
//	}


		
	//cool function
	//Can call as many times as you want on same audio path
	//Will not cause multiple downloads
	//All callbacks will be executed
//	public IEnumerator downloadAudio(string audioPath, System.Action<AudioClip> callback, bool local = false){
////		Debug.Log (audioPath);
//		if (!awaitingCallbacks.ContainsKey (audioPath)) {
//			awaitingCallbacks [audioPath] = new List<System.Action<AudioClip>> ();
//			awaitingCallbacks [audioPath].Add (callback);
//		} else {
//			awaitingCallbacks [audioPath].Add (callback);
//		}
//		if (!activeAudioDownloads.Contains (audioPath)) {//don't download again, wait for previous download to finish and execute callback
//			if (audioCache.contains (audioPath)) {
////				callback (audioCache.get (audioPath));
////				foreach(var c in awaitingCallbacks[audioPath]){
////					c (audioCache.get (audioPath));
////				}
////				awaitingCallbacks [audioPath].Clear ();
//
////				Debug.LogFormat ("Already cached {0}", audioPath);
//
//				executeQueuedCallbacks(audioPath, audioCache.get(audioPath));
//			} else {
//				activeAudioDownloads.Add (audioPath);
////				Debug.LogFormat ("Downloading {0}", audioPath);
//
//				if (local) {
////					AudioClip clip = localAudio (audioPath);
////					audioCache.add (audioPath, clip);
////					activeAudioDownloads.Remove(audioPath);
////					executeQueuedCallbacks(audioPath, clip);
//
//
//					ResourceRequest req = Resources.LoadAsync ("Audio/" + audioPath);
//					yield return req;
//
//					AudioClip clip = Instantiate(req.asset) as AudioClip;
//					audioCache.add (audioPath, clip);
//
//					activeAudioDownloads.Remove(audioPath);
//					executeQueuedCallbacks(audioPath, clip);
//				} else {
//					yield return API.downloadAudio (audioPath, audioClip => {
//						audioCache.add (audioPath, audioClip);
//						activeAudioDownloads.Remove(audioPath);
//
//						Debug.LogFormat ("Finished downloading {0}", audioPath);
//
//						executeQueuedCallbacks(audioPath, audioCache.get(audioPath));
//						//					callback (audioClip);
//					});
//				}
//			}
//		}
//	}

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


//	public void executeQueuedCallbacks(string audioPath, AudioClip clip){
//		foreach(var c in awaitingCallbacks[audioPath]){
//			c (clip);
//		}
//		awaitingCallbacks [audioPath].Clear ();
//	}

}
