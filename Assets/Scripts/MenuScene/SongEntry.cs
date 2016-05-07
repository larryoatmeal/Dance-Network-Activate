using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SongEntry : MonoBehaviour {
	Text text;
	RawImage image;
	Button imageButton;
//	string path;
	SongMeta songMeta;


	Texture2D defaultImage;

	public bool isReady = true;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text> ();
		image = GetComponentInChildren<RawImage> ();
//		text.text = "YO";
		imageButton = image.GetComponentInChildren<Button>();

		imageButton.onClick.AddListener (onClick);
		defaultImage = Resources.Load ("leafball") as Texture2D;
	}

	void onClick(){
//		Debug.Log (path);
//		GameManager.Instance.musicFile = path;
//		GameManager.Instance.midiFile = getAssociatedMidi ();	

		if (isReady) {
			GameManager.Instance.currentSong = songMeta;
			Messenger.Invoke (MessengerKeys.LOAD_PROGRESS);

			StartCoroutine(APICacheManager.Instance.downloadAudio(songMeta.musicPath, clip => {
				Debug.Log("Music ready");
				GameManager.Instance.currentAudio = clip;

				StartCoroutine(API.downloadMIDI(songMeta.midiPath, midi => {
					Debug.Log("MIDI ready");

					GameManager.Instance.currentMidi = midi;
					SceneManager.LoadScene ("rhythmTester");
				}, e => {
					Debug.LogWarning("MIDI not found using default");
					MIDI m = new MIDI("bike2");
					GameManager.Instance.currentMidi = m;
					SceneManager.LoadScene ("rhythmTester");
				}));
			}, songMeta.local));
		}
	}
		
	//for now just use same name
//	string getAssociatedMidi(){
//		return songMeta;
//	}

	public void SetSongMeta(SongMeta song){
		isReady = true;
		if (song != this.songMeta) {
			this.songMeta = song;
			this.text.text = song.name;

			if (song.local) {


			} else {
				if (song.thumbnail != "" && song.thumbnail != null) {
					StartCoroutine (APICacheManager.Instance.downloadAndCreateTexture (song.thumbnail, 
						(texture) => {
							this.image.texture = texture;
						}
					));
				} else {
					this.image.texture = defaultImage;
				}
			}
		}


	}


	public void setNotReady(){
		isReady = false;
		this.text.text = "Loading...";
	}

//
//	public void SetPath(string path){
//		this.path = path;
//	}
//
//	public void SetText(string text){
//		this.text.text = text;
//	}

	public void SetImage(string image){
		this.image.texture = Resources.Load (image) as Texture2D;
	}


	// Update is called once per frame
	void Update () {
	
	}
}
