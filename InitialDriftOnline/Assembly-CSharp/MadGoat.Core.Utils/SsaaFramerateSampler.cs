using UnityEngine;

namespace MadGoat.Core.Utils;

public class SsaaFramerateSampler
{
	private float newPeriod;

	private int intervalTotalFrames;

	private int intervalFrameSum;

	public int CurrentFps { get; private set; }

	public float UpdateInterval { get; set; }

	public SsaaFramerateSampler()
	{
		CurrentFps = 0;
		UpdateInterval = 1f;
	}

	public SsaaFramerateSampler(float updateInterval)
	{
		CurrentFps = 0;
		UpdateInterval = updateInterval;
	}

	public void Update()
	{
		intervalTotalFrames++;
		intervalFrameSum += (int)(1f / Time.deltaTime);
		if (Time.time > newPeriod)
		{
			CurrentFps = intervalFrameSum / intervalTotalFrames;
			intervalTotalFrames = 0;
			intervalFrameSum = 0;
			newPeriod += UpdateInterval;
		}
	}
}
