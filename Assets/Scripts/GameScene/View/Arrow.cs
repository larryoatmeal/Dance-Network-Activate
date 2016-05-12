using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void randomizeZ(){
		transform.Translate (0, 0, Random.value);
	}

	public void SetDirection(MusicEventTypes type){
		if (type == MusicEventTypes.Down) {
			transform.localEulerAngles = new Vector3 (0, 0, 180);
		} else if (type == MusicEventTypes.Up) {
			transform.localEulerAngles = new Vector3 (0, 0, 0);
		} else if (type == MusicEventTypes.Left) {
			transform.localEulerAngles = new Vector3 (0, 0, 90);
		} else {
			transform.localEulerAngles = new Vector3 (0, 0, 270);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
