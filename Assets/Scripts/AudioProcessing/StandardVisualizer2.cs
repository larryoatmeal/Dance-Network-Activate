using System;
using UnityEngine;
class StandardVisualizer2: AsyncTask{
	float[] mins;
	float[] maxs;

	DrawWaveform drawWaveForm;
	AudioStreamReader reader;
	public StandardVisualizer2(MonoBehaviourThreading caller, DrawWaveform drawFunction, AudioStreamReader reader):base(caller){
		this.drawWaveForm = drawFunction;
		this.reader = reader;
	}

	public override void onProgressOp (object progress)
	{
		int progressInt = Convert.ToInt32 (progress);
		Debug.LogFormat ("Processing progress yo {0}", progressInt);

	}
		
	public override void onCompleteOp ()
	{
		Debug.Log ("Calling drawWaveForm function");
		drawWaveForm (maxs, mins);
	}

	private int ceilDivision(int dividend, int divisor){
		return (dividend + divisor - 1) / divisor;   
	}

	//called in background thread
	public override void process ()
	{
		Debug.Log ("Processing audio vis 2");

		float block_seconds = 0.1f;

		int blockSize = (int) (block_seconds * reader.audioMeta.sampleRate);

		//integer division ceil
		int totalBlocks = reader.audioMeta.numSamplesPerChannel/ blockSize;

		mins = new float[totalBlocks];
		maxs = new float[totalBlocks];
		using (reader) {
			int samplesPerPecent = reader.audioMeta.numSamplesPerChannel / 100;

			int currentPercent = 0;
			int counter = 0;

			Debug.Log ("SUP");
	
			for (int block = 0; block < totalBlocks; block++) {
//				Debug.Log (block);
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
						notifyProgress (currentPercent);
					}else{
						counter += 1;
					}
				}
				mins [block] = min;
				maxs [block] = max;
//				Debug.Log (block);
			}
			Debug.Log ("HIHI");
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

		notifyComplete();
	}

//	private void op(){
//		drawWaveForm (maxs, mins);
//	}

}
