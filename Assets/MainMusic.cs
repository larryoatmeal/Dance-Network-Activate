using UnityEngine;
using System.Collections;

public class MainMusic : MonoBehaviour {

	private AudioSource audioSource;

	float ratio;
	// Use this for initialization
	void Awake () {
//		audioSource = gameObject.GetComponent<AudioSource> ();
//		string path = "Audio/" + GameManager.Instance.musicFile;
//		AudioClip clip = Resources.Load(path) as AudioClip;
//		Debug.Log (path);
//		audioSource.clip = clip;
//
//		Debug.Log (audioSource.clip.name);
//		ratio = 1000f / audioSource.clip.frequency;
		setAudio (GameManager.Instance.musicFile);
	}


	public void setAudioStream(string fileName){


		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "nexthouse.ogg");
		WWW request = new WWW (filePath);

		Debug.Log (filePath);
		Debug.Log (request);
		audioSource = gameObject.GetComponent<AudioSource> ();
		AudioClip clip = request.GetAudioClip (false, true);;
		audioSource.clip = clip;
	}

	public void setAudio(string fileName){
		audioSource = gameObject.GetComponent<AudioSource> ();
		string path = "Audio/" + fileName;
		AudioClip clip = Resources.Load(path) as AudioClip;
		Debug.Log (path);
		audioSource.clip = clip;

		Debug.Log (audioSource.clip.name);
		ratio = 1000f / audioSource.clip.frequency;
//		setAudioStream (fileName);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public int timeSamples(){
		return audioSource.timeSamples;
	}
		
	public int timeMillis(){

		return (int)(audioSource.timeSamples * ratio); 

	}

	public void pause(){
		audioSource.Pause ();
	}
	public void play(){
		if (!audioSource.isPlaying) {
			audioSource.Play ();
		}
	}

	public void playFromBeginning(float delay){
		Debug.Log ("PLAYING");
		audioSource.Stop ();
		audioSource.timeSamples = 0;
		audioSource.PlayDelayed (delay);

//		if (!audioSource.isPlaying) {
////			audioSource.clip.
////			audioSource.Play ();
////			audioSource.Stop();
//		} else {
//			audioSource.timeSamples = 0;
//		}
	}
}
