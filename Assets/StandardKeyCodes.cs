using System;
using System.Collections.Generic;
using UnityEngine;
public class StandardKeyCodes
{
	//these are standard key codes
	//(happen to be Mac key codes)
	public const int A = 0;
	public const int S = 1;
	public const int D = 2;
	public const int F = 3;


	private static Dictionary<int, KeyCode> UnityKeyMap = new Dictionary<int, KeyCode>(){
		{A, KeyCode.A},
		{S, KeyCode.S},
		{D, KeyCode.D},
		{F, KeyCode.F}
	};
		
	public static KeyCode ToUnityKey(int key){
		return UnityKeyMap[key];
	}
}


