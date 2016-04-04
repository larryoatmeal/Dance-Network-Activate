using System;
using System.Collections.Generic;
using UnityEngine;


public enum StandardKeyCodes{
	A,
	S,
	D,
	F
}

public class KeyConverter
{
	//these are standard key codes
	//(happen to be Mac key codes)
//	public const int A = 0;
//	public const int S = 1;
//	public const int D = 2;
//	public const int F = 3;


	private static Dictionary<StandardKeyCodes, KeyCode> UnityKeyMap = new Dictionary<StandardKeyCodes, KeyCode>(){
		{StandardKeyCodes.A, KeyCode.A},
		{StandardKeyCodes.S, KeyCode.S},
		{StandardKeyCodes.D, KeyCode.D},
		{StandardKeyCodes.F, KeyCode.F}
	};

	private static Dictionary<StandardKeyCodes, int> MacNative = new Dictionary<StandardKeyCodes, int>(){
		{StandardKeyCodes.A, 0},
		{StandardKeyCodes.S, 1},
		{StandardKeyCodes.D, 2},
		{StandardKeyCodes.F, 3}
	};
		
	public static KeyCode ToUnityKey(StandardKeyCodes key){
		return UnityKeyMap[key];
	}
	public static int ToMacNative(StandardKeyCodes key){
		return MacNative[key];
	}
}


