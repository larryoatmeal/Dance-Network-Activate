using UnityEngine;
using System.Collections;

public class LoadProgress : MonoBehaviour {

	// Use this for initialization

	void Awake(){
		Messenger.AddListener (MessengerKeys.LOAD_SCENE, display);
	}

	void Start () {
		
		this.gameObject.SetActive (false);
	}

	void display(){
		this.gameObject.SetActive (true);
		//this needs to be here, becuase Dispose is not being called for some reason
		Messenger.RemoveListener (MessengerKeys.LOAD_SCENE, display);
	}
	// Update is called once per frame
	void Update () {
	
	}

}
