using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
public class Modal : MonoBehaviour {

	ButtonPlus buttonOkay;
	ButtonPlus buttonCancel;
	ButtonPlus buttonNo;
	Text title;
	RowManager rowManager;

	// Use this for initialization
	void Awake () {
//		GameObject.FindGameObjectWithTag()
		buttonOkay = GameObject.FindGameObjectWithTag(Tags.ModalOk).GetComponent<ButtonPlus>();
		buttonNo = GameObject.FindGameObjectWithTag(Tags.ModalNo).GetComponent<ButtonPlus>();
		buttonCancel = GameObject.FindGameObjectWithTag(Tags.ModalCancel).GetComponent<ButtonPlus>();
		title = GameObject.FindGameObjectWithTag (Tags.ModalTitle).GetComponent<Text>();
		rowManager = GetComponentInChildren<RowManager> ();

	}

	void Start(){
		SetCancelEnabled (false);
		SetNoEnabled (false);
		SetOkayText ("Continue");
		SetTitle ("Score Test");
//		rowManager.SetRows (
//			new List<RowMeta> () {
//				{ new RowMeta ("Key1", "Val1") },
//				{ new RowMeta ("Key2", "Val2") },
//				{ new RowMeta ("Key3", "Val3") },
//			});
	}

	public void SetOkayEnabled(bool enabled){
		buttonOkay.gameObject.SetActive (enabled);
	}
	public void SetCancelEnabled(bool enabled){
		buttonCancel.gameObject.SetActive (enabled);
	}
	public void SetNoEnabled(bool enabled){
		buttonNo.gameObject.SetActive (enabled);
	}
	public void SetTitle(string titleText){
		title.text = titleText;
	}

	public void SetOkayText(string text){
		buttonOkay.SetText (text);
	}
	public void SetNoText(string text){
		buttonNo.SetText (text);
	}
	public void SetCancelText(string text){
		buttonCancel.SetText (text);
	}
	public void PopulateRows(List<RowMeta> rows){
		rowManager.SetRows (rows);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
