using System;
using UnityEngine;
public class PatternLoader: MonoBehaviour
{

	public const string CALIBRATION = "calibration"; 

	public int numQuartersCalibration = 10;

	public PatternLoader ()
	{
		
	}
	public Pattern loadPattern(string midifile){
		if (midifile == CALIBRATION) {
			return new Pattern (new BeatGenerator ().quarters (numQuartersCalibration));
		} else {
			//load file here
			return new Pattern (new BeatGenerator ().quarters (numQuartersCalibration));
		}
	}
}


