using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	public string musicFile = "Audio/wave";

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
