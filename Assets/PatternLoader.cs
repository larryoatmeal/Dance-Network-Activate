using System;
using UnityEngine;
public class PatternLoader
{

	public const string CALIBRATION = "calibration"; 

	public int numQuartersCalibration = 10;

	public PatternLoader ()
	{
		
	}
	public Pattern loadPattern(string midifile){
		if (midifile == CALIBRATION) {
			return new Pattern (new BeatGenerator ().quarters (numQuartersCalibration), GameManager.Instance.lookAhead);
		} else {
			//load file here
			return new Pattern (new BeatGenerator ().quarters (numQuartersCalibration), GameManager.Instance.lookAhead);
		}
	}
}


