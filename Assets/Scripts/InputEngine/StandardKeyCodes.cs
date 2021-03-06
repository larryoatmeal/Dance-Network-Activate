﻿using System;
using System.Collections.Generic;
using UnityEngine;


public enum StandardKeyCodes{
	A,
	S,
	D,
	F,
	J,
	K,
	LEFT,
	UP,
	RIGHT,
	DOWN
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
		{StandardKeyCodes.F, KeyCode.F},
		{StandardKeyCodes.J, KeyCode.J},
		{StandardKeyCodes.K, KeyCode.K},
		{StandardKeyCodes.LEFT, KeyCode.LeftArrow},
		{StandardKeyCodes.UP, KeyCode.UpArrow},
		{StandardKeyCodes.RIGHT, KeyCode.RightArrow},
		{StandardKeyCodes.DOWN, KeyCode.DownArrow},

	};

	private static Dictionary<StandardKeyCodes, int> MacNative = new Dictionary<StandardKeyCodes, int>(){
		{StandardKeyCodes.A, 0},
		{StandardKeyCodes.S, 1},
		{StandardKeyCodes.D, 2},
		{StandardKeyCodes.F, 3},
		{StandardKeyCodes.J, 0x26},
		{StandardKeyCodes.K, 0x28}

	};
		
	public static KeyCode ToUnityKey(StandardKeyCodes key){
		return UnityKeyMap[key];
	}
	public static int ToMacNative(StandardKeyCodes key){
		return MacNative[key];
	}
}


//enum {
//	kVK_ANSI_A                    = 0x00,
//	kVK_ANSI_S                    = 0x01,
//	kVK_ANSI_D                    = 0x02,
//	kVK_ANSI_F                    = 0x03,
//	kVK_ANSI_H                    = 0x04,
//	kVK_ANSI_G                    = 0x05,
//	kVK_ANSI_Z                    = 0x06,
//	kVK_ANSI_X                    = 0x07,
//	kVK_ANSI_C                    = 0x08,
//	kVK_ANSI_V                    = 0x09,
//	kVK_ANSI_B                    = 0x0B,
//	kVK_ANSI_Q                    = 0x0C,
//	kVK_ANSI_W                    = 0x0D,
//	kVK_ANSI_E                    = 0x0E,
//	kVK_ANSI_R                    = 0x0F,
//	kVK_ANSI_Y                    = 0x10,
//	kVK_ANSI_T                    = 0x11,
//	kVK_ANSI_1                    = 0x12,
//	kVK_ANSI_2                    = 0x13,
//	kVK_ANSI_3                    = 0x14,
//	kVK_ANSI_4                    = 0x15,
//	kVK_ANSI_6                    = 0x16,
//	kVK_ANSI_5                    = 0x17,
//	kVK_ANSI_Equal                = 0x18,
//	kVK_ANSI_9                    = 0x19,
//	kVK_ANSI_7                    = 0x1A,
//	kVK_ANSI_Minus                = 0x1B,
//	kVK_ANSI_8                    = 0x1C,
//	kVK_ANSI_0                    = 0x1D,
//	kVK_ANSI_RightBracket         = 0x1E,
//	kVK_ANSI_O                    = 0x1F,
//	kVK_ANSI_U                    = 0x20,
//	kVK_ANSI_LeftBracket          = 0x21,
//	kVK_ANSI_I                    = 0x22,
//	kVK_ANSI_P                    = 0x23,
//	kVK_ANSI_L                    = 0x25,
//	kVK_ANSI_J                    = 0x26,
//	kVK_ANSI_Quote                = 0x27,
//	kVK_ANSI_K                    = 0x28,
//	kVK_ANSI_Semicolon            = 0x29,
//	kVK_ANSI_Backslash            = 0x2A,
//	kVK_ANSI_Comma                = 0x2B,
//	kVK_ANSI_Slash                = 0x2C,
//	kVK_ANSI_N                    = 0x2D,
//	kVK_ANSI_M                    = 0x2E,
//	kVK_ANSI_Period               = 0x2F,
//	kVK_ANSI_Grave                = 0x32,
//	kVK_ANSI_KeypadDecimal        = 0x41,
//	kVK_ANSI_KeypadMultiply       = 0x43,
//	kVK_ANSI_KeypadPlus           = 0x45,
//	kVK_ANSI_KeypadClear          = 0x47,
//	kVK_ANSI_KeypadDivide         = 0x4B,
//	kVK_ANSI_KeypadEnter          = 0x4C,
//	kVK_ANSI_KeypadMinus          = 0x4E,
//	kVK_ANSI_KeypadEquals         = 0x51,
//	kVK_ANSI_Keypad0              = 0x52,
//	kVK_ANSI_Keypad1              = 0x53,
//	kVK_ANSI_Keypad2              = 0x54,
//	kVK_ANSI_Keypad3              = 0x55,
//	kVK_ANSI_Keypad4              = 0x56,
//	kVK_ANSI_Keypad5              = 0x57,
//	kVK_ANSI_Keypad6              = 0x58,
//	kVK_ANSI_Keypad7              = 0x59,
//	kVK_ANSI_Keypad8              = 0x5B,
//	kVK_ANSI_Keypad9              = 0x5C
//};
//
//
