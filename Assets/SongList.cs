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
	public List<string> songs = new List<string> ();


	void Awake(){

		TextAsset text = Resources.Load ("Audio/songList") as TextAsset;
		if (text == null) {
			Debug.LogError ("songList.json not found in Audio folder");
		} else {
			JSONObject obj = new JSONObject (text.text);
			List<string> songNames = obj.keys;
			songs = songNames;
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
	

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
