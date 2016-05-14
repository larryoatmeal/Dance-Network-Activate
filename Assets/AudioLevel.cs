using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AudioLevel : MonoBehaviour {

	Slider slider;

	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();
		slider.value = AudioListener.volume;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeVolume(float percent){
		AudioListener.volume = percent;
	}

}
