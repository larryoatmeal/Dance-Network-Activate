using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Tester : MonoBehaviourThreading {

	public RawImage rawImage;
	public void DrawToRawImage(float[] maxs, float[] mins){
		int numblocks = mins.Length;

		int width = 512;
		int height = 128;
		Texture2D texture2d = new Texture2D (width, height);

		int stepSize = numblocks / width;
		Debug.Log (stepSize);
		for (int i = 0; i < width; i++) {
			float min = mins [i * stepSize];
			float max = maxs [i * stepSize];

			int minPos = (int) (height / 2f * (1f + min));
			int maxPos = (int) (height / 2f * (1f + max));
			Debug.Log (i);
			Debug.Log (min);
			Debug.Log (minPos);

			for (int h = minPos; h <= maxPos; h++) {
				texture2d.SetPixel (i, h, Color.red);
			}
		}
		texture2d.Apply ();
		rawImage.texture = texture2d;
		Debug.Log ("Processing complete");
	}



	// Use this for initialization
	void Start () {
		print ("START");
//		AudioLoader audioLoader = new AudioLoader ();
////		audioLoader.loadFileFromStreamingAssets ("gravy_final.wav");
//		audioLoader.testReader("gravy_final.wav");
//		AudioStreamReader.test();





//
//
//		AudioVisualizer audioVisualizer = new AudioVisualizer (new StandardVisualizer(this, DrawToRawImage));
//		audioVisualizer.analyzeFile ("gravy_final_final_16.wav");
		Stream stream = AudioLoader.loadFileFromStreamingAssets ("gravy_final_final_16.wav");
		AudioStreamReader audioStreamReader = new AudioStreamReader (stream);

		StandardVisualizer2 vis = new StandardVisualizer2 (this, DrawToRawImage, audioStreamReader);
		vis.start ();

	}
}
