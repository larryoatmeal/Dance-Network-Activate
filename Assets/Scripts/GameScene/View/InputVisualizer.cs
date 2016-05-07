	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputVisualizer : MonoBehaviour {
	public List<GameObject> particles;
	private Dictionary<StandardControls, int> keyCodeToParticleNumber = new Dictionary<StandardControls, int>();

	// Use this for initialization
	void Start () {

		Messenger<StandardControls>.AddListener (MessengerKeys.EVENT_PAD_PRESSED, padPressed);
		Messenger<StandardControls>.AddListener (MessengerKeys.EVENT_PAD_RELEASED, padReleased);


		Debug.Log ("START");
		keyCodeToParticleNumber.Add (StandardControls.LEFT, 0);
		keyCodeToParticleNumber.Add (StandardControls.RIGHT, 3);
		keyCodeToParticleNumber.Add (StandardControls.UP, 2);
		keyCodeToParticleNumber.Add (StandardControls.DOWN, 1);
		particles.ForEach (p => p.SetActive (false));
	}

	void padPressed(StandardControls control){//integer
		//		int key = Convert.ToInt32(keycode);	
		//		Debug.LogFormat ("playing particle");
		int particleNum = keyCodeToParticleNumber [control];

		GameObject particle = particles [particleNum];
//		Debug.Log (particle);


		if (particle != null) {
			particle.SetActive(true);
		}

	} 

	void padReleased(StandardControls control){
		int particleNum = keyCodeToParticleNumber [control];
		GameObject particle = particles [particleNum];
//		particle.gameObject.SetActive(false);

		if (particle != null) {
			particle.SetActive(false);
		}

	}

	void OnDispose(){
		Messenger<StandardControls>.RemoveListener (MessengerKeys.EVENT_PAD_PRESSED, padPressed);
		Messenger<StandardControls>.RemoveListener (MessengerKeys.EVENT_PAD_RELEASED, padReleased);
	}

	// Update is called once per frame
	void Update () {
		

	}
}
