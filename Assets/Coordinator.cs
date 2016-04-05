using UnityEngine;
using System.Collections;

public class Coordinator : MonoBehaviour {

	public MainMusic music;
	public CalibratorV2 calibrator;

	public RealtimeInput input;

	// Use this for initialization
	void Start () {
		music.setAudio ("120bpmTest");
	}

	// Update is called once per frame
	void Update () {
		music.playFromBeginning (0);	
	}
}
