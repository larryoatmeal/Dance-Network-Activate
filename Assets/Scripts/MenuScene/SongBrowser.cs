using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class SongBrowser : MonoBehaviour {

	public List<SongEntry> entries;
	public SongList songList;

	int index = 0;

	public bool aside = true;

	public void Next(){
		if (aside) {
			index = (index + 1) % songList.localSongs.Count;

		} else {
			if (songList.isBsideReady ()) {
				index = (index + 1) % songList.onlineSongs.Count;

			}
		}
		playMiddle ();
	}
	public void Prev(){
		index -= 1;
		if (index < 0) {
			if (aside) {
				index = songList.localSongs.Count - 1;

			} else {
				if (songList.isBsideReady ()) {
					index = songList.onlineSongs.Count - 1;
				} else {
					index = 0;
				}
			}
		}
		playMiddle ();
//

//		if (index - 1 >= 0) {
//			index -= 1;
//			dirty = true;
//		}

	}

	public void playMiddle(){
		if (aside || songList.isBsideReady ()) {
			Messenger<SongMeta>.Invoke (MessengerKeys.PLAY_SONG, getMiddleSong ());
		}

	}


	public SongMeta getMiddleSong(){
		int midIndex = (index + entries.Count / 2);

		if (aside) {
			return songList.localSongs [midIndex % songList.localSongs.Count];
		} else {
			return songList.onlineSongs [midIndex % songList.onlineSongs.Count];
		}
	}

	// Use this for initialization
	IEnumerator Start () {
		if (songList.localSongs.Count < entries.Count) {
			Debug.Log ("Not enough songs");
		}

		while (!songList.isBsideReady ()) {
			yield return new WaitForSeconds (0.1f);
		}

		playMiddle ();

	}


	// Update is called once per frame
	void Update () {
		for (int i = 0; i < entries.Count; i++) {
			if (aside) {
				entries [i].SetSongMeta (songList.localSongs [(i + index)%songList.localSongs.Count]);
			} else {//bside
				if (songList.isBsideReady ()) {
					entries[i].SetSongMeta (songList.onlineSongs [(i + index)%songList.onlineSongs.Count]);
				} else {
					entries [i].setNotReady ();
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Prev ();
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Next ();
		}



	}

	public void toggle(){
		aside = !aside;
		playMiddle ();
	}


	public void Calibrator(){
		SceneManager.LoadScene ("calibrator");
	}

}
