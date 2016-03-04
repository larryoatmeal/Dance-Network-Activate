using System;
using UnityEngine;
class StandardVisualizer: AudioVisualizerHandler{
	float[] mins;
	float[] maxs;

	MonoBehaviourThreading caller;
	DrawWaveform drawWaveForm;

	public StandardVisualizer(MonoBehaviourThreading caller, DrawWaveform drawFunction){
		this.caller = caller;
		this.drawWaveForm = drawFunction;
	}

	public void progressChanged (int progress)
	{
		Debug.LogFormat ("Processing progress {0}", progress);
	}

	private int ceilDivision(int dividend, int divisor){
		return (dividend + divisor - 1) / divisor;   
	}

	//called in background thread
	public void process (AudioStreamReader reader, System.ComponentModel.BackgroundWorker bw)
	{
		Debug.Log ("Processing audio");

		float block_seconds = 0.1f;

		int blockSize = (int) (block_seconds * reader.audioMeta.sampleRate);

		//integer division ceil
		int totalBlocks = ceilDivision(reader.audioMeta.numSamplesPerChannel, blockSize);

		mins = new float[totalBlocks];
		maxs = new float[totalBlocks];

		using (reader) {
			int samplesPerPecent = reader.audioMeta.numSamplesPerChannel / 100;

			int currentPercent = 0;
			int counter = 0;

			for (int block = 0; block < totalBlocks; block++) {
				float max = -1f;
				float min = 1f;

				for (int i = 0; i < blockSize; i++) {
					float sample = reader.readSampleOneChannel ();

					if (sample > max) {
						max = sample;
					}
					if (sample < min) {
						min = sample;
					}

					if (counter > samplesPerPecent) {
						currentPercent += 1;
						counter = 0;
						bw.ReportProgress (currentPercent);
					}else{
						counter += 1;
					}
				}
				mins [block] = min;
				maxs [block] = max;
			}
			//				for (int i = 0; i < reader.audioMeta.numSamplesPerChannel; i++) {
			//					float sample = reader.readSampleOneChannel ();
			//
			//					if (counter > samplesPerPecent) {
			//						currentPercent += 1;
			//						counter = 0;
			//						bw.ReportProgress (currentPercent);
			//					}else{
			//						counter += 1;
			//					}
			//
			//				}
		}
	}

	private void op(){
		drawWaveForm (maxs, mins);
	}

	//also called on background thread
	public void onComplete ()
	{
		caller.callOnMainThread (op);
	}
}