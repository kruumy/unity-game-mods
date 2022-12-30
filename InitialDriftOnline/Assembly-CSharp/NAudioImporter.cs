using System;
using NAudio.Wave;
using UnityEngine;

[AddComponentMenu("AudioImporter/NAudio Importer")]
public class NAudioImporter : DecoderImporter
{
	private Mp3FileReader reader;

	private ISampleProvider sampleProvider;

	protected override void Initialize()
	{
		try
		{
			if (!base.uri.IsFile)
			{
				throw new FormatException("NAudioImporter does not support URLs");
			}
			reader = new Mp3FileReader(base.uri.LocalPath);
			sampleProvider = reader.ToSampleProvider();
		}
		catch (Exception ex)
		{
			OnError(ex.Message);
		}
	}

	protected override void Cleanup()
	{
		if (reader != null)
		{
			reader.Dispose();
		}
		reader = null;
		sampleProvider = null;
	}

	protected override AudioInfo GetInfo()
	{
		WaveFormat waveFormat = reader.WaveFormat;
		return new AudioInfo((int)reader.Length / (waveFormat.BitsPerSample / 8), waveFormat.SampleRate, waveFormat.Channels);
	}

	protected override int GetSamples(float[] buffer, int offset, int count)
	{
		return sampleProvider.Read(buffer, offset, count);
	}
}
