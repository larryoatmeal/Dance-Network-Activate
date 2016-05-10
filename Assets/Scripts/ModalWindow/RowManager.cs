using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RowManager : MonoBehaviour {

	RowEntry[] rowEntries;


	void Awake(){
		rowEntries = gameObject.GetComponentsInChildren<RowEntry> ();
	}

	// Use this for initialization
	void Start () {
		
	}

	public void SetRows(List<RowMeta> rows){
		for (int i = 0; i < rowEntries.Length; i++) {
			RowEntry rowEntry = rowEntries [i];

			if (i < rows.Count) {
				RowMeta meta = rows [i];
				rowEntry.setKeyText (meta.key);
				rowEntry.setValText (meta.value);

				if (meta.sprite != null) {
					rowEntry.setImageEnabled (true);
					rowEntry.setTexture (meta.sprite);
				} else {
					rowEntry.setImageEnabled (false);
				}
			} else {//disable unused
				rowEntry.gameObject.SetActive(false);
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
