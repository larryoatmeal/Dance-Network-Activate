using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ConditionalText : MonoBehaviour {

	public string mobile;
	public string web;
	public string desktop;

	public Text mobileText;

	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();

		#if UNITY_STANDALONE
		text.text = desktop;

		#elif UNITY_EDITOR
		text.text = desktop;

		#elif UNITY_IOS
		text.text = mobile;

		#elif UNITY_ANDROID
		text.text = mobile;

		#endif



	}

}
