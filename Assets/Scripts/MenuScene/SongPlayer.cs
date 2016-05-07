using UnityEngine;
using System.Collections;

public class SongPlayer : MonoBehaviour {

	AudioClip clip;

	AudioSource source;
	// Use this for initialization
	void Start () {
		Messenger<SongMeta>.AddListener (MessengerKeys.PLAY_SONG, playSong);
		source = gameObject.GetComponent<AudioSource> ();
		Debug.Log (source);
	}


	void playSong(SongMeta song){
		StartCoroutine (APICacheManager.Instance.downloadAudio(song.musicPath, onReady, song.local));
	}
	void onReady(AudioClip clip){
		if (source.isPlaying) {
			source.Stop ();
		}
		source.clip = clip;
		source.Play ();
	}


	void OnDestroy(){
		Messenger<SongMeta>.RemoveListener (MessengerKeys.PLAY_SONG, playSong);


	}
	// Update is called once per frame
	void Update () {

	}
}
