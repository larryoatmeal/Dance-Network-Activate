using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
public class DebugPanel : MonoBehaviour {

	public Dictionary<string, string> entries = new Dictionary<string, string>();
	private Text textMesh;
	private bool dirty = false;

	public void log(string key, string entry){
		if (entries.ContainsKey (key)) {
			entries [key] = entry;
		} else {
			entries.Add (key, entry);
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
