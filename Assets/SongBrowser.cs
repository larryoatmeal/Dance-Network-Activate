using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class SongBrowser : MonoBehaviour {

	public List<SongEntry> entries;
	public SongList songList;

	int index = 0;

//	bool dirty = true; //initially dirty since need to draw

	public void Next(){
		index = (index + 1) % songList.songs.Count;

//
//		if (index + 1 + entries.Count - 1 < songList.songs.Count) {
//			index += 1;
//			dirty = true;
//		}
	}
	public void Prev(){



		index -= 1;
		if (index < 0) {
			index = songList.songs.Count - 1;
		}
//
//		if (index - 1 >= 0) {
//			index -= 1;
//			dirty = true;
//		}
	}

	// Use this for initialization
	void Start () {
		if (songList.songs.Count < entries.Count) {
			Debug.Log ("Not enough songs");
		}
	}
	
	// Update is called once per frame
	void Update () {
//		if (dirty) {
			for (int i = 0; i < entries.Count; i++) {
				string songName;
//				if (i + index < songList.songs.Count) {
				songName = songList.songs [(i + index)%songList.songs.Count];
				entries [i].SetText (songName);
				entries [i].SetPath (songName);
			}
//			dirty = false;
//		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Prev ();
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Next ();
		}


	}

	public void Calibrator(){
		SceneManager.LoadScene ("calibrator");
	}

}
