using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Threading;
using System.ComponentModel;


public class AudioStreamReader: IDisposable{
	BinaryReader reader;
	public readonly AudioMeta audioMeta;
	int samplesLeft;

	private AudioStreamReader(){
	}
	public AudioStreamReader (Stream stream)
	{
		reader = new BinaryReader (stream);
		audioMeta = obtainMetaData ();
		//		bytesLeft = audioMeta.numBytes;
		samplesLeft = audioMeta.numSamples;
	}
	//must be called exactly once before reading data
	private AudioMeta obtainMetaData(){
		int chunkID = reader.ReadInt32();
		int fileSize = reader.ReadInt32();
		int riffType = reader.ReadInt32();
		int fmtID = reader.ReadInt32();
		int fmtSize = reader.ReadInt32();
		int fmtCode = reader.ReadInt16();
		int channels = reader.ReadInt16();
		int sampleRate = reader.ReadInt32();
		int fmtAvgBPS = reader.ReadInt32();
		int fmtBlockAlign = reader.ReadInt16();
		int bitDepth = reader.ReadInt16();

		if (fmtSize == 18) {
			int fmtExtraSize = reader.ReadInt16 ();
			reader.ReadBytes (fmtExtraSize);
		}

		int dataID = reader.ReadInt32 ();
		int dataSize = reader.ReadInt32 ();
		return new AudioMeta (bitDepth, sampleRate, channels, dataSize);
	}

	public float readSample(){
		samplesLeft -= 1;
		if (audioMeta.bitDepth == 16) {
			short value = reader.ReadInt16 ();
			return (float)value / 32768;//2^15
		} else if (audioMeta.bitDepth == 24) {
			return read24bit ();
		} else {
			Debug.LogErrorFormat ("Bit depth {0} not supported", audioMeta.bitDepth);
			return 0f;
		}
	}

	public float readSampleOneChannel(){
		float channelOne = readSample ();
		skipSamples (audioMeta.numChannels - 1);
		return channelOne;
	}

	public void skipSamples(int numSamples){
		reader.ReadBytes(audioMeta.bytesPerSample * numSamples);
	}

	private float read24bit(){
		byte[] bytes = reader.ReadBytes (3);
		int shifted;
		//multply 2^8
		if (System.BitConverter.IsLittleEndian) {
			shifted = bytes[0] << 8 | bytes[1] << 16 | bytes[2] << 24;
		}else{
			shifted = bytes[2] << 8 | bytes[1] << 16 | bytes[0] << 24;
		}
		//since we already multiplied 2^8, divide by 2^31 to get scaled
		return (float)shifted / 2147483648;
	}

	public void Dispose ()
	{
		reader.Close();
	}
}


