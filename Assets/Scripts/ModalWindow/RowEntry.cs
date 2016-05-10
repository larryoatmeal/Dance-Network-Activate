using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RowEntry : MonoBehaviour {

	Text[] texts;
	Image image;

	Text key;
	Text val;

	// Use this for initialization
	void Awake () {
		texts = GetComponentsInChildren<Text> ();
		image = GetComponentInChildren<Image> ();

		if (image.tag != Tags.ModalImage) {
			Debug.LogWarning ("ModalImage not found");
		}
		for (int i = 0; i < texts.Length; i++) {
			Text text = texts [i];

			if (text.tag == Tags.ModalKey) {
				key = text;
			} else if (text.tag == Tags.ModalValue) {
				val = text;
			}

		}

		if (key == null) {
			Debug.LogWarning ("Key not found");
		}
		if (val == null) {
			Debug.LogWarning ("Key not found");
		}


		//default no image
		setImageEnabled(false);

		setKeyText ("Key");
		setValText ("Value");
	}

	public void setImageEnabled(bool enabled){
		image.gameObject.SetActive (enabled);
	}

	public void setTexture(Sprite tex){
		image.overrideSprite = tex;
	}

	public void setKeyText(string text){
//		Debug.Log (key);
		key.text = text;
	}

	public void setValText(string text){
		val.text = text;
	}


	// Update is called once per frame
	void Update () {
	
	}
}
