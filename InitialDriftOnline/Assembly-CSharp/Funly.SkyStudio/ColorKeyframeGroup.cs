using System;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class ColorKeyframeGroup : KeyframeGroup<ColorKeyframe>
{
	public ColorKeyframeGroup(string name)
		: base(name)
	{
	}

	public ColorKeyframeGroup(string name, ColorKeyframe frame)
		: base(name)
	{
		AddKeyFrame(frame);
	}

	public Color ColorForTime(float time)
	{
		time -= (float)(int)time;
		if (keyframes.Count == 0)
		{
			Debug.LogError("Can't return color since there aren't any keyframes.");
			return Color.white;
		}
		if (keyframes.Count == 1)
		{
			return GetKeyframe(0).color;
		}
		GetSurroundingKeyFrames(time, out int beforeIndex, out int afterIndex);
		ColorKeyframe keyframe = GetKeyframe(beforeIndex);
		ColorKeyframe keyframe2 = GetKeyframe(afterIndex);
		float t = KeyframeGroup<ColorKeyframe>.ProgressBetweenSurroundingKeyframes(time, keyframe, keyframe2);
		float t2 = CurveAdjustedBlendingTime(keyframe.interpolationCurve, t);
		return Color.Lerp(keyframe.color, keyframe2.color, t2);
	}
}
