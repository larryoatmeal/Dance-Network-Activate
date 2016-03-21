using UnityEngine;
using System.Collections;

public class ScenePasser : MonoBehaviour {

	public static ScenePasser instance;

	void Awake(){

		// if not initialized yet
		if (instance != null) {
			instance = new ScenePasser ();
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (this);
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
