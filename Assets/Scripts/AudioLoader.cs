using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Threading;
using System.ComponentModel;

public class AudioLoader 
{

	public static Stream loadFileFromStreamingAssets (string file){
		return loadFile (Application.streamingAssetsPath + "/" + file);
	}
	public static Stream loadFile (string path){
		return File.OpenRead (path);
	}
	public BinaryReader getBinaryReader(Stream stream){
		return new BinaryReader (stream);
	}
}

