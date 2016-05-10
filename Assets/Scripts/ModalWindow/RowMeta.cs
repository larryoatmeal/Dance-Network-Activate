using System;
using UnityEngine;

public class RowMeta
{
	public string key;
	public string value;
	public Sprite sprite;

	public RowMeta (string key, string value, Sprite sprite)
	{
		this.key = key;
		this.value = value;
		this.sprite = sprite;
	}

	public RowMeta (string key, object value)
	{
		this.key = key;
		this.value = value.ToString();
		this.sprite = null;
	}
	
	
}


