using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {
	//set this number to the xScale needed to get bar to full
	public float xScaleForFull = 1.5f;

	public void setFraction(float fraction){
		this.transform.localScale = new Vector3 (xScaleForFull * fraction, transform.localScale.y, transform.localScale.z);
	}
}
