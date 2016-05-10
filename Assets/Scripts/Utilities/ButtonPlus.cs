using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ButtonPlus : MonoBehaviour {

	Text buttonText;
	Button button;
	// Use this for initialization
	void Awake () {
		button = gameObject.GetComponent<Button> ();
		buttonText = gameObject.GetComponentInChildren<Text> ();
	}

	public void SetText(string text){
		buttonText.text = text;
	}

	public void AddListener(UnityEngine.Events.UnityAction action){
		button.onClick.AddListener (action);
	}

	public void RemoveListener(UnityEngine.Events.UnityAction action){
		button.onClick.RemoveListener (action);
	}

}
