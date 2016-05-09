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
	PreloadSong localSong;

	public Texture2D defaultImage;

	private bool local = false;
	public bool isReady = true;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text> ();
		image = GetComponentInChildren<RawImage> ();
//		text.text = "YO";
		imageButton = image.GetComponentInChildren<Button>();

		imageButton.onClick.AddListener (onClick);
//		defaultImage = Resources.Load ("leafball") as Texture2D;
	}

	void onClick(){
//		Debug.Log (path);
//		GameManager.Instance.musicFile = path;
//		GameManager.Instance.midiFile = getAssociatedMidi ();	

		if (isReady) {
			if (!local) {
				GameManager.Instance.currentSong = songMeta;
				APICacheManager.Instance.downloadAudio (songMeta.musicPath, clip => {
					Debug.Log ("Music ready");
					GameManager.Instance.currentAudio = clip;

					StartCoroutine (API.downloadMIDI (songMeta.midiPath, midi => {
						Debug.Log ("MIDI ready");

						GameManager.Instance.currentMidi = midi;
						SceneManager.LoadScene ("rhythmTester");
					}, e => {
						Debug.LogWarning ("MIDI not found using default");
						MIDI m = new MIDI ("bike2");
						GameManager.Instance.currentMidi = m;
						SceneManager.LoadScene ("rhythmTester");
					}));
				});
			} else {
				GameManager.Instance.currentAudio = localSong.getAudioClip ();
				GameManager.Instance.currentMidi = localSong.getMIDI ();
				GameManager.Instance.currentSong = localSong.getSongMeta ();

				Messenger.Invoke (MessengerKeys.LOAD_PROGRESS);
				SceneManager.LoadScene ("rhythmTester");

			}
		}
	}
		
	//for now just use same name
//	string getAssociatedMidi(){
//		return songMeta;
//	}

	public void SetSongMeta(SongMeta song){
		isReady = true;
		if (song != this.songMeta || local) {//new song or changed song
			this.songMeta = song;
			this.text.text = song.name;

			if (song.thumbnail != "" && song.thumbnail != null) {
//				StartCoroutine (APICacheManager.Instance.downloadAndCreateTexture (song.thumbnail, song.local, 
//					(texture) => {
//						this.image.texture = texture;
//					}
//				));
				APICacheManager.Instance.downloadTexture (song.thumbnail, (texture) => {
					this.image.texture = texture;
				});

			} else {
				this.image.texture = defaultImage;
			}
		} else {
			
		}

		local = false;

	}

	public void SetSongLocal(PreloadSong song){
		isReady = true;

		if (song != localSong || !local) {
			this.text.text = song.songName;
			if (song.thumbnail != null) {
				this.image.texture = song.thumbnail;
			} else {
				this.image.texture = defaultImage;
			}

		} else {

		}
		localSong = song;

		local = true;


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
