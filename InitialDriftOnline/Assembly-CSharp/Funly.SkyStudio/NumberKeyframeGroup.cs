using System;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class NumberKeyframeGroup : KeyframeGroup<NumberKeyframe>
{
	public float minValue;

	public float maxValue;

	public NumberKeyframeGroup(string name, float min, float max)
		: base(name)
	{
		minValue = min;
		maxValue = max;
	}

	public NumberKeyframeGroup(string name, float min, float max, NumberKeyframe frame)
		: base(name)
	{
		minValue = min;
		maxValue = max;
		AddKeyFrame(frame);
	}

	public float GetFirstValue()
	{
		return GetKeyframe(0).value;
	}

	public float ValueToPercent(float value)
	{
		return Mathf.Abs((value - minValue) / (maxValue - minValue));
	}

	public float ValuePercentAtTime(float time)
	{
		return ValueToPercent(NumericValueAtTime(time));
	}

	public float PercentToValue(float percent)
	{
		return Mathf.Clamp(minValue + (maxValue - minValue) * percent, minValue, maxValue);
	}

	public float NumericValueAtTime(float time)
	{
		time -= (float)(int)time;
		if (keyframes.Count == 0)
		{
			Debug.LogError("Keyframe group has no keyframes: " + base.name);
			return minValue;
		}
		if (keyframes.Count == 1)
		{
			return GetKeyframe(0).value;
		}
		GetSurroundingKeyFrames(time, out int beforeIndex, out int afterIndex);
		NumberKeyframe keyframe = GetKeyframe(beforeIndex);
		NumberKeyframe keyframe2 = GetKeyframe(afterIndex);
		return InterpolateFloat(keyframe.interpolationCurve, keyframe.interpolationDirection, time, keyframe.time, keyframe2.time, keyframe.value, keyframe2.value, minValue, maxValue);
	}
}
