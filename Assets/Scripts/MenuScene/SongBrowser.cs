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

	
//		preCache ();

//		if (aside || songList.isBsideReady ()) {
//			Messenger<SongMeta>.Invoke (MessengerKeys.PLAY_SONG, getMiddleSong ());
//		}
		if (aside) {
			Messenger<PreloadSong>.Invoke (MessengerKeys.PLAY_LOCAL_SONG, getMiddleLocal ());
		} else {
			if (songList.isBsideReady ()) {
				Messenger<SongMeta>.Invoke (MessengerKeys.PLAY_SONG, getMiddleOnline ());
			}
		}
	}
	public void preCache(){
		Debug.Log ("Precaching");
		var active = songList.onlineSongs;
		int lookahead = 4;

		for (int i = 1; i <= lookahead; i++) {
			preDownload (getSongCyclic (index + i, active));
			preDownload (getSongCyclic (index - i, active));
		}
	}

	private void preDownload(SongMeta song){
		Debug.LogFormat ("Predonwloading {0}", song);
		APICacheManager.Instance.downloadAudio (song.musicPath, (clip) => {
		});
	}


	private T getSongCyclic<T>(int index, List<T> songs){
		return songs [mod (index, songs.Count)];
	}

	private int mod(int x, int y){
		if (x < 0) {
			return y + x;
		} else {
			return x % y;
		}
	}



	private List<SongMeta> getActiveList(){
		List<SongMeta> activeList;

		if (aside) {
			activeList = songList.localSongs;
		} else {
			if (songList.isBsideReady ()) {
				activeList = songList.onlineSongs;
			} else {
				activeList = new List<SongMeta> ();
			}
		}
		return activeList;
	}
		
//	public SongMeta getMiddleSong(){
//		int midIndex = (index + entries.Count / 2);
//
//		if (aside) {
//			return songList.localSongs [midIndex % songList.localSongs.Count];
//		} else {
//			return songList.onlineSongs [midIndex % songList.onlineSongs.Count];
//		}
//	}

	public T getMiddle<T>(int startIndex, int viewSize, List<T> items){
		int midIndex = (startIndex + viewSize / 2);
		return items [midIndex % items.Count];
	}

	public SongMeta getMiddleOnline(){
		return getMiddle<SongMeta> (index, entries.Count, songList.onlineSongs);
	}

	public PreloadSong getMiddleLocal(){
		
		return getMiddle<PreloadSong> (index, entries.Count, songList.PreloadSongs);
	}


	// Use this for initialization
	IEnumerator Start () {
		if (songList.localSongs.Count < entries.Count) {
			Debug.Log ("Not enough songs");
		}

		if (!aside) {
			while (!songList.isBsideReady ()) {
				yield return new WaitForSeconds (0.1f);
			}
			playMiddle ();
		} else {
			playMiddle ();
		}
//		preCache ();

	}


	// Update is called once per frame
//	void Update () {
//		for (int i = 0; i < entries.Count; i++) {
//			if (aside) {
//				entries [i].SetSongMeta (songList.localSongs [(i + index)%songList.localSongs.Count]);
//			} else {//bside
//				if (songList.isBsideReady ()) {
//					entries[i].SetSongMeta (songList.onlineSongs [(i + index)%songList.onlineSongs.Count]);
//				} else {
//					entries [i].setNotReady ();
//				}
//			}
//		}
//		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
//			Prev ();
//		}
//		if (Input.GetKeyDown (KeyCode.RightArrow)) {
//			Next ();
//		}
//	}


	void Update(){
		for (int i = 0; i < entries.Count; i++) {
			var songEntry = entries [i];
			if (aside) {
//				Debug.Log ("HERE");
				var localSong = getSongCyclic<PreloadSong> (index + i, songList.PreloadSongs);
				songEntry.SetSongLocal (localSong);
			} else {
				if (songList.isBsideReady ()) {
//					Debug.Log ("THERE");
					var onlineSong = getSongCyclic<SongMeta> (index + i, songList.onlineSongs);
					songEntry.SetSongMeta (onlineSong);
				} else {
					songEntry.setNotReady ();
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
		index = 0;
		aside = !aside;
		playMiddle ();
	}


	public void Calibrator(){
		SceneManager.LoadScene ("calibrator");
	}

}