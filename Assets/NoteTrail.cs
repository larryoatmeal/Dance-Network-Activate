using UnityEngine;
using System.Collections;

public class NoteTrail : MonoBehaviour {

	LineRenderer lineRenderer;
	private float x = 0f;

	// Use this for initialization
	void Start () {
		lineRenderer = this.gameObject.GetComponent<LineRenderer> ();
	}
		
	public void setX(float x){
		this.x = x;
	}
	public void setTopY(Vector3 pos){
		lineRenderer.SetPosition (0, pos);
	}
	public void setBottomY(Vector3 pos){
		lineRenderer.SetPosition (1, pos);
	}
	public void setTopY(float y){
		lineRenderer.SetPosition (0, new Vector3(x, y, 0));
	}
	public void setBottomY(float y){
		lineRenderer.SetPosition (1, new Vector3(x, y, 0));
	}

	// Update is called once per frame
	void Update () {
	
	}
}
