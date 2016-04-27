using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ComboText : MonoBehaviour {
	Text text;
	// Use this for initialization
	void Start () {
		text = gameObject.GetComponentInChildren<Text> ();
	}
	
	public void SetCombo(int combo){
		if (combo < 2) {
			text.enabled = false;
		} else {
			string comboString = "Combo " + combo;
			text.text = comboString;
			text.enabled = true;

		}
	}
}
