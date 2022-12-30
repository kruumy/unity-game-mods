using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public abstract class DecoderImporter : AudioImporter
{
	protected class AudioInfo
	{
		public int lengthSamples { get; private set; }

		public int sampleRate { get; private set; }

		public int channels { get; private set; }

		public AudioInfo(int lengthSamples, int sampleRate, int channels)
		{
			this.lengthSamples = lengthSamples;
			this.sampleRate = sampleRate;
			this.channels = channels;
		}
	}

	private AudioInfo info;

	private int bufferSize;

	private float[] buffer;

	private AutoResetEvent waitForMainThread;

	private Thread import;

	private int index;

	private bool abort;

	private Queue<Action> executionQueue = new Queue<Action>();

	private object _lock = new object();

	public override void Abort()
	{
		if (!abort && import != null && import.IsAlive)
		{
			abort = true;
			if (!isInitialized)
			{
				UnityEngine.Object.Destroy(audioClip);
			}
			lock (_lock)
			{
				executionQueue.Clear();
			}
			waitForMainThread.Set();
			import.Join();
		}
	}

	protected override void Import()
	{
		bufferSize = 262144;
		buffer = new float[bufferSize];
		isDone = false;
		isInitialized = false;
		abort = false;
		index = 0;
		progress = 0f;
		waitForMainThread = new AutoResetEvent(initialState: false);
		import = new Thread(DoImport);
		import.Start();
	}

	private void DoImport()
	{
		Initialize();
		if (!isError)
		{
			info = GetInfo();
			Dispatch(CreateClip);
			Decode();
			Cleanup();
			progress = 1f;
			isDone = true;
		}
	}

	private void Decode()
	{
		while (index < info.lengthSamples)
		{
			int samples = GetSamples(buffer, 0, bufferSize);
			if (samples != 0 && !abort)
			{
				if (index + bufferSize >= info.lengthSamples)
				{
					Array.Resize(ref buffer, info.lengthSamples - index);
				}
				Dispatch(SetData);
				index += samples;
				progress = (float)index / (float)info.lengthSamples;
				continue;
			}
			break;
		}
	}

	private void CreateClip()
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(base.uri.LocalPath);
		audioClip = AudioClip.Create(fileNameWithoutExtension, info.lengthSamples / info.channels, info.channels, info.sampleRate, stream: false);
		waitForMainThread.Set();
	}

	private void SetData()
	{
		if (audioClip == null)
		{
			Abort();
			return;
		}
		audioClip.SetData(buffer, index / info.channels);
		if (!isInitialized)
		{
			isInitialized = true;
			OnLoaded();
		}
		waitForMainThread.Set();
	}

	protected void OnError(string error)
	{
		this.error = error;
		isError = true;
		progress = 1f;
	}

	private void Dispatch(Action action)
	{
		lock (_lock)
		{
			executionQueue.Enqueue(action);
		}
		waitForMainThread.WaitOne();
	}

	private void Update()
	{
		lock (_lock)
		{
			while (executionQueue.Count > 0)
			{
				executionQueue.Dequeue()();
			}
		}
	}

	protected abstract void Initialize();

	protected abstract void Cleanup();

	protected abstract int GetSamples(float[] buffer, int offset, int count);

	protected abstract AudioInfo GetInfo();
}
