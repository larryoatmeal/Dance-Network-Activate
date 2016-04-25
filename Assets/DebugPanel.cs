using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
public class DebugPanel : Singleton<DebugPanel> {

	public Dictionary<string, string> entries = new Dictionary<string, string>();
	private Text textMesh;
	private bool dirty = false;
	private bool showing = true;
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
		if (dirty && showing) {
//			Debug.Log (genString ());
			textMesh.text = genString ();
			dirty = false;
		}
	}
	public void Toggle(){
		if (showing) {
			Hide ();
		} else {
			Show ();
		}
	}


	public void Hide(){
		textMesh.enabled = false;
		showing = false;

	}
	public void Show(){
		textMesh.enabled = true;
		showing = true;
	}

}
