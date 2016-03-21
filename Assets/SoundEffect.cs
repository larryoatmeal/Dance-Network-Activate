using UnityEngine;
using System.Collections;

public class SoundEffect : MonoBehaviour {
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
//		audioSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Play(){
		audioSource.PlayOneShot (audioSource.clip);
	}
		
	public void PlayScheduled(float deltaS){
		audioSource.PlayScheduled(AudioSettings.dspTime + deltaS);
	}

	public void PlayDelay(float delay){
		audioSource.PlayDelayed (delay);
	}

}
