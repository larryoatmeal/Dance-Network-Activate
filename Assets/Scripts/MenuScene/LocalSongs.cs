using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LocalSongs : MonoBehaviour {

	PreloadSong[] preloadedSongs;

	void Awake(){
		preloadedSongs = gameObject.GetComponentsInChildren<PreloadSong> ();
	}

	// Use this for initialization
	void Start () {
		

	}


	public PreloadSong[] getPreloadedSongs(){
		return preloadedSongs;
	}


	// Update is called once per frame
	void Update () {
	
	}
}
