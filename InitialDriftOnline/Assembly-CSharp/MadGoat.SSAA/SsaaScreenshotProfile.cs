using System;
using UnityEngine;

namespace MadGoat.SSAA;

[Serializable]
public class SsaaScreenshotProfile
{
	public bool takeScreenshot;

	[Range(1f, 4f)]
	public int screenshotMultiplier = 1;

	public Vector2 outputResolution = new Vector2(1920f, 1080f);

	public bool downsamplerEnabled = true;

	public Filter downsamplerFilter;

	[Range(0f, 1f)]
	public float downsamplerSharpness = 0.85f;

	public float downsamplerDistance = 1f;

	public PostAntiAliasingMode postAntiAliasing;
}
