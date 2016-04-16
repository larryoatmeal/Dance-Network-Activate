﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
public class DebugPanel : Singleton<DebugPanel> {

	public Dictionary<string, string> entries = new Dictionary<string, string>();
	private Text textMesh;
	private bool dirty = false;

	protected override void Init(){
		Debug.Log ("[DebugPanel] init");
		Persist = true;
	}

	public void log(string key, object entry){
		if (entries.ContainsKey (key)) {
			entries [key] = entry.ToString();
		} else {
			entries.Add (key, entry.ToString());
		}
		dirty = true;
	}

	string genString(){

		StringBuilder sb = new StringBuilder ();
		foreach (KeyValuePair<string, string> entry in entries) {
			sb.AppendFormat ("{0}:{1}", entry.Key, entry.Value); 
			sb.AppendLine ();
		}
		return sb.ToString ();
	}
		
	// Use this for initialization
	void Start () {
		textMesh = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (dirty) {
//			Debug.Log (genString ());
			textMesh.text = genString ();
			dirty = false;
		}
	}
}
