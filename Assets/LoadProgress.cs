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
		Messenger.RemoveListener (MessengerKeys.LOAD_SCENE, display);
	}
	// Update is called once per frame
	void Update () {
	
	}

}
