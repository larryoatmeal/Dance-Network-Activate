using System;


public delegate void onKey (int keycode, long time);

public class RealtimeInputListener{
	public int[] keys;
	public onKey onKeyUp;
	public onKey onKeyDown;

	public RealtimeInputListener (int[] keys, onKey onKeyUp, onKey onKeyDown)
	{
		this.keys = keys;
		this.onKeyUp = onKeyUp;
		this.onKeyDown = onKeyDown;
	}

//	public const int A = 0;
//	public const int S = 1;
//	public const int D = 2;
//	public const int F = 3;
}



