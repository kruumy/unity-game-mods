using System;
using UnityEngine;

namespace MadGoat.SSAA;

[Serializable]
public class SsaaProfile
{
	[Range(0.5f, 2f)]
	public float multiplier;

	public bool useFilter;

	[Tooltip("Which type of filtering to be used (only applied if useShader is true)")]
	public Filter filterType = Filter.BILINEAR;

	[Tooltip("The sharpness of the filtered image (only applied if useShader is true)")]
	[Range(0f, 1f)]
	public float sharpness;

	[Tooltip("The distance between the samples (only applied if useShader is true)")]
	[Range(0.5f, 2f)]
	public float sampleDistance;

	public SsaaProfile(float mul, bool useDownsampling)
	{
		multiplier = mul;
		useFilter = useDownsampling;
		sharpness = (useDownsampling ? 0.85f : 0f);
		sampleDistance = (useDownsampling ? 0.65f : 0f);
	}

	public SsaaProfile(float mul, bool useDownsampling, Filter filterType, float sharp, float sampleDist)
	{
		multiplier = mul;
		this.filterType = filterType;
		useFilter = useDownsampling;
		sharpness = (useDownsampling ? sharp : 0f);
		sampleDistance = (useDownsampling ? sampleDist : 0f);
	}
}
