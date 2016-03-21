using UnityEngine;
using System.Collections;
using System.IO;
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


	// Use this for initialization
	void Start () {
	
		string fullPath = Application.dataPath + audioPath;

//		Debug.Log (fullPath);

		DirectoryInfo dir = new DirectoryInfo (fullPath);
		FileInfo[] info = dir
			.GetFiles ("*.*")
			.Where(file => supportedExtensions.Contains(file.Extension)).ToArray();

		foreach (FileInfo f in info){
			Debug.Log (f.Name);
			Debug.Log (f.Extension);
			songs.Add (f.Name);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
