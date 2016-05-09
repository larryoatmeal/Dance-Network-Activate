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
	public LocalSongs preloadSongManager;
	private List<PreloadSong> preloadSongs;



	bool bsideReady = false;


	void Awake(){
		preloadSongs = preloadSongManager.getPreloadedSongs ().ToList ();
		Debug.Log (preloadSongs);

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
		
		StartCoroutine (APICacheManager.Instance.publicSongs (s => {
			onlineSongs = s;
			bsideReady = true;
//
//			StartCoroutine (APICacheManager.Instance.downloadAudio (song.musicPath, (a) => {}));
//
//
//			songBrowser.playMiddle();
			const int preDownloadRange = 6;
			for(int i = 0; i < Mathf.Min(onlineSongs.Count, preDownloadRange); i++){
				//half backwards, half forwards
				int index = i - preDownloadRange/2;
				if(index < 0){
					index = onlineSongs.Count + index;
				}
				APICacheManager.Instance.downloadAudio (onlineSongs[index].musicPath, (a) => {});
			}
		}));

	}

	public List<PreloadSong> PreloadSongs {
		get {
			return this.preloadSongs;
		}
	}


	public bool isBsideReady(){
		return bsideReady;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
