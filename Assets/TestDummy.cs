using UnityEngine;
using System.Collections;

public class TestDummy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MIDI midi = new MIDI ();
		midi.processMidi ("bicycle-ride");



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
