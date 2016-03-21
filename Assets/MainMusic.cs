using UnityEngine;
using System.Collections;

public class MainMusic : MonoBehaviour {

	private AudioSource audioSource;

	float ratio;
	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();

		AudioClip clip = Resources.Load(GameManager.Instance.musicFile) as AudioClip;
		Debug.Log (GameManager.Instance.musicFile);


		audioSource.clip = clip;

		Debug.Log (audioSource.clip.name);
		ratio = 1000f / audioSource.clip.frequency;
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

	public void playFromBeginning(){
		if (!audioSource.isPlaying) {
//			audioSource.clip.
			audioSource.Play ();
			audioSource.timeSamples = 0;
		} else {
			audioSource.timeSamples = 0;
		}
	}



}
