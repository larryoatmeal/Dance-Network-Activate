using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
public class SongList : MonoBehaviour {
	public string audioPath = "/Resources/Audio/";
	public string[] supportedExtensions = new string[]{
		".mp3",
		".wav",
		".ogg"
	};
	public List<SongMeta> localSongs = new List<SongMeta> ();

	public List<SongMeta> onlineSongs = new List<SongMeta>();
	public SongBrowser songBrowser;

	bool bsideReady = false;


	void Awake(){
		TextAsset text = Resources.Load ("Audio/songList") as TextAsset;
		if (text == null) {
			Debug.LogError ("songList.json not found in Audio folder");
		} else {
			localSongs = SongMeta.parseString (text.text);
//			JSONObject obj = new JSONObject (text.text);
//			List<string> songNames = obj.keys;
//			localSongs = songNames;
		}
			
//		string fullPath = Application.dataPath + audioPath;
//
//		Debug.Log (fullPath);
//
//		DirectoryInfo dir = new DirectoryInfo (fullPath);
//		FileInfo[] info = dir
//			.GetFiles ("*.*")
//			.Where(file => supportedExtensions.Contains(file.Extension)).ToArray();
//
//		foreach (FileInfo f in info){
//			Debug.Log (f.Name);
//			Debug.Log (f.Extension);
//			songs.Add (Path.GetFileNameWithoutExtension(f.Name));
//		}
	}
		
	// Use this for initialization
	void Start () {
		StartCoroutine (API.publicSongs (s => {
			onlineSongs = s;
			bsideReady = true;
//
//			StartCoroutine (APICacheManager.Instance.downloadAudio (song.musicPath, (a) => {}));
//
//

//			songBrowser.playMiddle();

//			foreach (SongMeta song in onlineSongs) {
//				StartCoroutine (APICacheManager.Instance.downloadAudio (song.musicPath, (a) => {}));
//			}
		}));
			
	

	}


	public bool isBsideReady(){
		return bsideReady;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
