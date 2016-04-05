using UnityEngine;
using System.Collections;

public class Coordinator : MonoBehaviour {

	public MainMusic music;
	public CalibratorV2 calibrator;

	public RealtimeInput input;

	// Use this for initialization
	void Start () {
		Debug.Log ("Start");
		music.setAudio ("120bpmTest");
		Play ();
	}

	// Update is called once per frame
	void Update () {
		long time = input.GetKeyDown (KeyMappings.controlToKey (StandardControls.DOWN));

		if (time > 0) {
			calibrator.KeyDown (time);
		}
	}

	void Play(){
		Debug.Log ("Playing");
		TimeMaster time = TimeMaster.Instance;

		int delay = 1;
		music.playFromBeginning (delay);
		calibrator.Play (delay * 1000 + time.GetTime());
	}


}
