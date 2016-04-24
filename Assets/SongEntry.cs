using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SongEntry : MonoBehaviour {
	Text text;
	RawImage image;
	Button imageButton;
	string path;
	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text> ();
		image = GetComponentInChildren<RawImage> ();
//		text.text = "YO";
		imageButton = image.GetComponentInChildren<Button>();

		imageButton.onClick.AddListener (onClick);

	}

	void onClick(){
		Debug.Log (path);
		GameManager.Instance.musicFile = path;
		GameManager.Instance.midiFile = getAssociatedMidi ();	
		SceneManager.LoadScene ("rhythmTester");
	}
		
	//for now just use same name
	string getAssociatedMidi(){
		return path;
	}

	public void SetPath(string path){
		this.path = path;
	}

	public void SetText(string text){
		this.text.text = text;
	}

	public void SetImage(string image){
		this.image.texture = Resources.Load (image) as Texture2D;
	}


	// Update is called once per frame
	void Update () {
	
	}
}
