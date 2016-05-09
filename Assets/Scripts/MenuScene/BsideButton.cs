using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BsideButton : MonoBehaviour {

	public string AsideString = "A side";
	public string BSideString = "B side";

	public bool aside = true;
	public Text text;
	public 
	void Start(){
		text = GetComponentInChildren<Text> ();
		displayState ();
	}

	public void toggle(){
		aside = !aside;
		displayState ();
	}
	private void displayState(){
		if (aside) {
			text.text = AsideString;
		} else {
			text.text = BSideString;
		}
	}


}
