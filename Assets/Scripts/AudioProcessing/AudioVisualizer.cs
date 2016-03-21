using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Threading;
using System.ComponentModel;


public delegate void DrawWaveform(float[] maxs, float[] mins);

public interface AudioVisualizerHandler{
	void progressChanged (int progress);
	void process(AudioStreamReader reader, BackgroundWorker bw);
	void onComplete();
}


public class AudioVisualizer{

	readonly AudioVisualizerHandler handler;

	public AudioVisualizer (AudioVisualizerHandler handler){
		this.handler = handler;
	}

	public void analyzeFile(string file){
		Stream stream = AudioLoader.loadFileFromStreamingAssets (file);
		AudioStreamReader audioStream = new AudioStreamReader (stream); 
		runAsyncPrintBytes (audioStream);
	}

	private void runAsyncPrintBytes(AudioStreamReader audioStreamReader){
		var bw = new BackgroundWorker ();
		bw.DoWork += new DoWorkEventHandler (process);
		bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler (onComplete);
		bw.ProgressChanged += new ProgressChangedEventHandler (onProgressChange);
		bw.WorkerReportsProgress = true;
		bw.RunWorkerAsync (audioStreamReader);
	}

	//does not run on main thread as expected
	private void onComplete(object sender, RunWorkerCompletedEventArgs e){
		handler.onComplete ();
	}

	//does not run on main thread as expected
	private void onProgressChange(object sender, ProgressChangedEventArgs e){
		handler.progressChanged(e.ProgressPercentage);
	}

	private void process(object sender, DoWorkEventArgs e){
		BackgroundWorker worker = sender as BackgroundWorker;
		AudioStreamReader reader = e.Argument as AudioStreamReader;
		handler.process (reader, worker);
	}
}

