using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	void Awake(){
		Messenger.AddListener (MessengerKeys.TOGGLE_MENU, Toggle);
//		this.gameObject.SetActive (t);
	}
	void Toggle(){
		this.gameObject.SetActive (!gameObject.activeSelf);
	}
	void OnDestroy(){
		Messenger.RemoveListener (MessengerKeys.TOGGLE_MENU, Toggle);
	}

}
