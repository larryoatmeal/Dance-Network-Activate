using UnityEngine;
using System.Collections;

public class SongPlayer : MonoBehaviour {

	AudioClip clip;

	AudioSource source;
	float defaultVolume = 0;

	PreloadSong lastPreloadedSong;

	// Use this for initialization
	public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
		float startVolume = audioSource.volume;

		while (audioSource.volume > 0) {
			audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
			yield return null;
		}

		audioSource.Stop ();
		audioSource.volume = startVolume;
	}
		
	void Start () {
		Messenger<SongMeta>.AddListener (MessengerKeys.PLAY_SONG, playSong);
		Messenger<PreloadSong>.AddListener(MessengerKeys.PLAY_LOCAL_SONG, playSongLocal); 
		source = gameObject.GetComponent<AudioSource> ();
		defaultVolume = source.volume;
		Debug.Log (source);
	}

	void StopWithFade(){
//		StopCoroutine ("FadeOut");
		StartCoroutine (FadeOut(source, 0.5f));
	}
	void StopImmediate(){
//		StopCoroutine ("FadeOut");
		source.Stop ();
	}

	void playSong(SongMeta song){
//		StopWithFade ();
		if (lastPreloadedSong != null) {
			lastPreloadedSong.audioSource.Stop ();
		}
		source.Stop();
		Debug.Log ("Attempting to play " + song.name);
		StartCoroutine (APICacheManager.Instance.downloadAudio(song.musicPath, onReady, song.local));
	}

	void playSongLocal(PreloadSong song){
		//		StopWithFade ();
		source.Stop();
		if (lastPreloadedSong != null) {
			lastPreloadedSong.audioSource.Stop();
		}
		lastPreloadedSong = song;
		song.audioSource.Play ();
//		StartCoroutine (APICacheManager.Instance.downloadAudio(song.musicPath, onReady, song.local));

	}

	void onReady(AudioClip clip){
		if (source.isPlaying) {
			StopImmediate ();
		}
		source.clip = clip;
		source.volume = defaultVolume;
		source.Play ();
	}


	void OnDestroy(){
		Messenger<SongMeta>.RemoveListener (MessengerKeys.PLAY_SONG, playSong);
		Messenger<PreloadSong>.RemoveListener (MessengerKeys.PLAY_LOCAL_SONG, playSongLocal);
	}
	// Update is called once per frame
	void Update () {

	}
}
