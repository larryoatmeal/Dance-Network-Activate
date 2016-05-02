using UnityEngine;
using System.Collections;

public class LoadProgress : MonoBehaviour {
	// Use this for initialization
	void Awake(){
		Messenger.AddListener (MessengerKeys.LOAD_PROGRESS, display);
//		Messenger.AddListener (MessengerKeys.HIDE_PROGRESS, display);

	}

	void Start () {

		this.gameObject.SetActive (false);
	}

	void display(){
		this.gameObject.SetActive (true);
	}


	void OnDestroy(){
		Messenger.RemoveListener (MessengerKeys.LOAD_PROGRESS, display);
//		Messenger.AddListener (MessengerKeys.HIDE_PROGRESS, display);

	}

	// Update is called once per frame
	void Update () {
	
	}
}
