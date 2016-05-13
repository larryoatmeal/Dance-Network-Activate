using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class MobileInput : MonoBehaviour {



	Game game;
	List<float> goalPosts = new List<float> ();
	TimeMaster timeMaster;

	#if UNITY_IOS
	// Use this for initialization
	void Start () {
		timeMaster = TimeMaster.Instance;
		for (int i = 1; i < pads.Length; i++) {
			goalPosts.Add ((pads [i].transform.position.x + pads [i - 1].transform.position.x) / 2);
//			Debug.Log (goalPosts [i - 1]);
		}
	}

	StandardControls getControl(float x){
		int greaterGoalPost = 0;
		for (int i = 0; i < goalPosts.Count; i++) {
			greaterGoalPost = i;
			if (x < goalPosts [i]) {
				if (greaterGoalPost == 0) {
					return StandardControls.LEFT;
				} else if (greaterGoalPost == 1) {
					return StandardControls.DOWN;
				} else if (greaterGoalPost == 2) {
					return StandardControls.UP;
				} 
			}
		};
		return StandardControls.RIGHT;
	}
		
	//should be in order
	//left, down, up, right
	public GameObject[] pads;

	Dictionary<StandardKeyCodes, long> downs = new Dictionary<StandardKeyCodes, long>(){
	};

	Dictionary<StandardKeyCodes, long> ups = new Dictionary<StandardKeyCodes, long>(){
	};


	// Update is called once per frame
	void Update () {
		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch (i);
			Vector2 pos = touch.position;
			float x = Camera.main.ScreenToWorldPoint (new Vector3 (pos.x, pos.y, 0)).x;

			StandardKeyCodes key = KeyMappings.reverseMap [getControl (x)];
			Debug.LogFormat ("Key {0}", key);

			if (touch.phase == TouchPhase.Began) {
				downs [key] = timeMaster.GetTime ();
				Debug.LogFormat ("X down {0}", x);
			}
			else if (touch.phase == TouchPhase.Ended) {
				ups [key] = timeMaster.GetTime ();
				Debug.LogFormat ("X up {0}", x);
			}
		}
	}

	public long keyDown(StandardKeyCodes keyCode){
		if (downs.ContainsKey (keyCode)) {
			long time = downs [keyCode];
			downs.Remove (keyCode);
			return time;
		} else {
			return -1;
		}
	}

	public long keyUp(StandardKeyCodes keyCode){
		if (ups.ContainsKey (keyCode)) {
			long time = ups [keyCode];
			ups.Remove (keyCode);
			return time;
		} else {
			return -1;
		}
	}

	#endif

}
