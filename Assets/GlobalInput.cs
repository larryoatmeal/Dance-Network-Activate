﻿using UnityEngine;
using System.Collections;

public class GlobalInput : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) {
			DebugPanel.Instance.Toggle ();
		}
	}
}
