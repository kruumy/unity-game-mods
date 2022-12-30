using System;
using System.Collections.Generic;
using System.ComponentModel;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

public class SteamworksVoiceManager : MonoBehaviour
{
	public enum SampleRateMethod
	{
		Optimal,
		Native,
		Custom
	}

	public AudioSource OutputSource;

	public SampleRateMethod sampleRateMethod;

	[Range(11025f, 48000f)]
	public uint customSampleRate = 28000u;

	public bool useAudioStreaming = true;

	[Range(0f, 1f)]
	public float bufferLength = 0.25f;

	[ReadOnly(true)]
	[SerializeField]
	private bool isRecording;

	public UnityEvent StopedOnChatRestricted;

	public ByteArrayEvent VoiceStream;

	private int sampleRate;

	private Queue<float> audioBuffer = new Queue<float>(48000);

	private Queue<AudioClip> clipBuffer = new Queue<AudioClip>();

	private float packetCounter;

	public double encodingTime;

	public bool IsRecording => isRecording;

	private void Start()
	{
		OutputSource.loop = true;
		packetCounter = bufferLength;
	}

	private void Update()
	{
		int num = ((sampleRateMethod == SampleRateMethod.Optimal) ? ((int)SteamUser.GetVoiceOptimalSampleRate()) : ((sampleRateMethod == SampleRateMethod.Native) ? AudioSettings.outputSampleRate : ((int)customSampleRate)));
		if (num != sampleRate)
		{
			sampleRate = num;
			OutputSource.Stop();
			if (OutputSource.clip != null)
			{
				UnityEngine.Object.Destroy(OutputSource.clip);
			}
			if (useAudioStreaming)
			{
				OutputSource.clip = AudioClip.Create("VOICE", sampleRate * 2, 1, sampleRate, stream: true, OnAudioRead);
				OutputSource.Play();
			}
			else
			{
				OutputSource.clip = AudioClip.Create("VOICE", sampleRate * 2, 1, sampleRate, stream: false);
			}
		}
		if (!useAudioStreaming && OutputSource.loop)
		{
			OutputSource.loop = false;
			OutputSource.clip = AudioClip.Create("VOICE", sampleRate * 2, 1, sampleRate, stream: false);
		}
		else if (useAudioStreaming && !OutputSource.loop)
		{
			OutputSource.loop = true;
			OutputSource.clip = AudioClip.Create("VOICE", sampleRate * 2, 1, sampleRate, stream: true, OnAudioRead);
			OutputSource.Play();
		}
		if (!useAudioStreaming && clipBuffer.Count > 0 && !OutputSource.isPlaying)
		{
			OutputSource.clip = clipBuffer.Dequeue();
			OutputSource.Play();
		}
		packetCounter -= Time.unscaledDeltaTime;
		if (!(packetCounter <= 0f))
		{
			return;
		}
		packetCounter = bufferLength;
		if (!isRecording)
		{
			return;
		}
		uint pcbCompressed;
		switch (SteamUser.GetAvailableVoice(out pcbCompressed))
		{
		case EVoiceResult.k_EVoiceResultOK:
		{
			byte[] array = new byte[pcbCompressed];
			SteamUser.GetVoice(bWantCompressed: true, array, pcbCompressed, out var nBytesWritten);
			if (nBytesWritten != 0)
			{
				VoiceStream.Invoke(array);
			}
			break;
		}
		case EVoiceResult.k_EVoiceResultNotInitialized:
			Debug.LogError("The Steam Voice systemis not initalized and will be stoped.");
			SteamUser.StopVoiceRecording();
			break;
		case EVoiceResult.k_EVoiceResultNotRecording:
			SteamUser.StartVoiceRecording();
			break;
		case EVoiceResult.k_EVoiceResultRestricted:
			StopedOnChatRestricted.Invoke();
			SteamUser.StopVoiceRecording();
			break;
		case EVoiceResult.k_EVoiceResultNoData:
		case EVoiceResult.k_EVoiceResultBufferTooSmall:
		case EVoiceResult.k_EVoiceResultDataCorrupted:
			break;
		}
	}

	public void StartRecording()
	{
		isRecording = true;
		SteamUser.StartVoiceRecording();
	}

	public void StopRecording()
	{
		isRecording = false;
		SteamUser.StopVoiceRecording();
	}

	public void PlayVoiceData(byte[] buffer)
	{
		byte[] array = new byte[20000];
		uint nBytesWritten;
		EVoiceResult eVoiceResult = SteamUser.DecompressVoice(buffer, (uint)buffer.Length, array, (uint)array.Length, out nBytesWritten, (uint)sampleRate);
		DateTime now = DateTime.Now;
		if (eVoiceResult == EVoiceResult.k_EVoiceResultBufferTooSmall)
		{
			array = new byte[nBytesWritten];
			eVoiceResult = SteamUser.DecompressVoice(buffer, (uint)buffer.Length, array, (uint)array.Length, out nBytesWritten, (uint)sampleRate);
		}
		if (nBytesWritten != 0)
		{
			if (useAudioStreaming)
			{
				for (int i = 0; i < nBytesWritten; i += 2)
				{
					audioBuffer.Enqueue((float)(short)(array[i] | (array[i + 1] << 8)) / 32768f);
				}
			}
			else
			{
				float[] array2 = new float[2 + nBytesWritten / 2u];
				int num = 1;
				for (int j = 0; j < nBytesWritten; j += 2)
				{
					array2[num] = (float)(short)(array[j] | (array[j + 1] << 8)) / 32768f;
					num++;
				}
				num++;
				if (!OutputSource.isPlaying && OutputSource.clip != null)
				{
					UnityEngine.Object.Destroy(OutputSource.clip);
					OutputSource.clip = AudioClip.Create("VOICE", num, 1, sampleRate, stream: false);
					OutputSource.clip.SetData(array2, 0);
					OutputSource.Play();
				}
				else
				{
					AudioClip audioClip = AudioClip.Create("VOICE " + now.ToBinary(), num, 1, sampleRate, stream: false);
					audioClip.SetData(array2, 0);
					clipBuffer.Enqueue(audioClip);
				}
			}
			double totalMilliseconds = (DateTime.Now - now).TotalMilliseconds;
			if (totalMilliseconds > encodingTime)
			{
				encodingTime = totalMilliseconds;
			}
		}
		else
		{
			Debug.LogWarning("Unknown result message: " + eVoiceResult);
		}
	}

	private void OnAudioRead(float[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			if (audioBuffer.Count > 0)
			{
				data[i] = audioBuffer.Dequeue();
			}
			else
			{
				data[i] = 0f;
			}
		}
	}
}
