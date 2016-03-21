using System;

public struct AudioMeta{
	public readonly int bitDepth;
	public readonly int sampleRate;
	public readonly int numChannels;
	public readonly int numBytes;
	public readonly int numSamples;
	public readonly int bytesPerSample;
	public readonly int numSamplesPerChannel;

	//Boilerplate
	public AudioMeta (int bitDepth, int sampleRate, int numChannels, int numBytes)
	{
		this.bitDepth = bitDepth;
		this.sampleRate = sampleRate;
		this.numChannels = numChannels;
		this.numBytes = numBytes;
		this.bytesPerSample = bitDepth / 8;
		this.numSamples = numBytes / bytesPerSample;
		this.numSamplesPerChannel = numSamples / numChannels;
	}
	public override string ToString ()
	{
		return string.Format ("[AudioMeta: bitDepth={0}, sampleRate={1}, numChannels={2}, numBytes={3}]", bitDepth, sampleRate, numChannels, numBytes);
	}
}


