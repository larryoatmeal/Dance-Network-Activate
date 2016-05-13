using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Coordinator : MonoBehaviour {

	public MainMusic music;
	public CalibratorV2 calibrator;

	public RealtimeInput input;
	public AudioClip calibrationClip;


	// Use this for initialization
	void Start () {
		Debug.Log ("Start");
//		music.setAudio ("120bpmTest");
		music.Init(calibrationClip);
		Play ();
	}

	// Update is called once per frame
	void Update () {
		long time = input.GetKeyDown (KeyMappings.controlToKey (StandardControls.DOWN));

		if (time > 0) {
			calibrator.KeyDown (time);
		}
	}

	public void Play(){
		Debug.Log ("Playing");
		TimeMaster time = TimeMaster.Instance;

		int delay = 1;
		music.playFromBeginning (delay);
		calibrator.Play (delay * 1000 + time.GetTime());
	}

	public void Done(){
		music.pause ();
		calibrator.finish ();
		MainMenu ();
	}

	public void MainMenu(){
		SceneManager.LoadScene ("SongBrowser");
	}

}
