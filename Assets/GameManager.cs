using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {
	
	public string musicFile = "wave";
	public string patternFile = "demo";
	public int lookAhead = 3000;

	public void SetMusicFile(string fileName){
	}

	protected override void Init(){
		Debug.Log ("[GameManager] init");
		Persist = true;
	}
	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
	
	}
}
