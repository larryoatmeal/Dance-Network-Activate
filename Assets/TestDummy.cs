using UnityEngine;
using System.Collections;

public class TestDummy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MIDI midi = new MIDI ("bicycle-ride");
		midi.dumbMapping ();



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
