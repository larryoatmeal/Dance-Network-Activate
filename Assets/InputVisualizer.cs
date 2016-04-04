using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputVisualizer : MonoBehaviour {
	public List<ParticleSystem> particles;
	private Dictionary<StandardControls, int> keyCodeToParticleNumber = new Dictionary<StandardControls, int>();

	// Use this for initialization
	void Start () {

		Messenger<StandardControls>.AddListener (MessengerKeys.EVENT_PAD_PRESSED, padPressed);
		Messenger<StandardControls>.AddListener (MessengerKeys.EVENT_PAD_RELEASED, padReleased);

		keyCodeToParticleNumber.Add (StandardControls.LEFT, 0);
		keyCodeToParticleNumber.Add (StandardControls.RIGHT, 1);
		keyCodeToParticleNumber.Add (StandardControls.UP, 2);
		keyCodeToParticleNumber.Add (StandardControls.DOWN, 3);
		particles.ForEach (p => p.gameObject.SetActive (false));
	}

	void padPressed(StandardControls control){//integer
		//		int key = Convert.ToInt32(keycode);	
		//		Debug.LogFormat ("playing particle");
		int particleNum = keyCodeToParticleNumber [control];
		ParticleSystem particle = particles [particleNum];
		particle.gameObject.SetActive(true);
	} 

	void padReleased(StandardControls control){
		int particleNum = keyCodeToParticleNumber [control];
		ParticleSystem particle = particles [particleNum];
		particle.gameObject.SetActive(false);
	}

	void OnDispose(){
		Messenger<StandardControls>.RemoveListener (MessengerKeys.EVENT_PAD_PRESSED, padPressed);
		Messenger<StandardControls>.RemoveListener (MessengerKeys.EVENT_PAD_RELEASED, padReleased);
	}

	// Update is called once per frame
	void Update () {
		

	}
}
