using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Calibrator : MonoBehaviour {
	public float m_sdCutoff = 1;

	List<int> deltas = new List<int>();

	public int m_outlierCutoff = 100;
	public int m_acceptableSD = 50;
	public void reset(){
		deltas.Clear();
	}

	public void addDelta(int delta){
		
		deltas.Add (delta);
	}

	public int finish(){
		int offset = (int) calculateOffset (deltas);
		PlayerPrefs.SetInt (PlayerPrefKeys.AudioLatencyOffset, offset);
		return offset;
	}

	int avg(List<int> data){
		long sum = 0;
		foreach(int datum in data){
			sum += datum;
		}
		if (data.Count > 0) {
			return (int)(sum / data.Count);
		} else {
			return 0;
		}

	}

	float standardDeviation(List<int> data){
		int average = avg (data);
		int sum = 0;
		foreach (int datum in data) {
			int diff = average - datum;

			sum += diff * diff;
		}

		float scaledSum = sum/((float) data.Count);
		return Mathf.Sqrt (scaledSum);
	}
		
	double calculateOffset(List<int> data){
		int average = avg (data);
		float sd = standardDeviation (data);

		Debug.LogFormat ("Average {0}", average);
		Debug.LogFormat ("SD {0}", sd);

		//remove outliers
//		const float numSd = 1;

		double offset = avg (data
//			.Where (n => Mathf.Abs (n - average) > m_sdCutoff)
			.Where (n => Mathf.Abs (n) < m_outlierCutoff)
			.ToList ());
		Debug.LogFormat ("Offset {0}", offset);

		if (sd < m_acceptableSD) {
			Debug.Log ("Acceptable sd");
		}
		return offset;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
