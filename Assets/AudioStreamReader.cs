using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Threading;
using System.ComponentModel;





public class AudioStreamReader: IDisposable{
	Stream stream;
	BinaryReader reader;
	public readonly AudioMeta audioMeta;
	//	int bytesLeft;
	int samplesLeft;

	private AudioStreamReader(){
	}
	public AudioStreamReader (Stream stream)
	{
		this.stream = stream;
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

	public void runAsyncPrintBytes(){
		var bw = new BackgroundWorker ();
		bw.DoWork += new DoWorkEventHandler (printBytes);
		bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler (printBytesCompleted);
		bw.ProgressChanged += new ProgressChangedEventHandler (printProgressChanged);
		bw.WorkerReportsProgress = true;
		bw.RunWorkerAsync ();
	}

	private void printBytesCompleted(object sender, RunWorkerCompletedEventArgs e){
		Debug.Log ("Bytes completed printing");
	}

	private void printProgressChanged(object sender, ProgressChangedEventArgs e){
		Debug.LogFormat ("Progress {0}%", e.ProgressPercentage);
	}

	private void printBytes(object sender, DoWorkEventArgs e){
		BackgroundWorker worker = sender as BackgroundWorker;

		Debug.LogFormat ("Number of samples {0}", audioMeta.numSamplesPerChannel);
		int samplesPerPercent = audioMeta.numSamplesPerChannel / 100;
		int counter = 0;
		int percent = 0;
		worker.ReportProgress (0);
		for (int i = 0; i < audioMeta.numSamplesPerChannel; i++) {
			readSampleOneChannel();
			counter += 1;
			if (counter > samplesPerPercent) {
				percent += 1;
				counter = 0;
				worker.ReportProgress (percent);
			}
		}
	}

	public static void test(){
		Stream stream = AudioLoader.loadFileFromStreamingAssets ("gravy_final_final_16.wav");
		AudioStreamReader testReader = new AudioStreamReader (stream);
		testReader.runAsyncPrintBytes();
	}
}


