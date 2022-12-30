using System;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class SpherePointKeyframeGroup : KeyframeGroup<SpherePointKeyframe>
{
	public const float MinHorizontalRotation = -(float)Math.PI;

	public const float MaxHorizontalRotation = (float)Math.PI;

	public const float MinVerticalRotation = -(float)Math.PI / 2f;

	public const float MaxVerticalRotation = (float)Math.PI / 2f;

	public SpherePointKeyframeGroup(string name)
		: base(name)
	{
	}

	public SpherePointKeyframeGroup(string name, SpherePointKeyframe keyframe)
		: base(name)
	{
		AddKeyFrame(keyframe);
	}

	public SpherePoint SpherePointForTime(float time)
	{
		if (keyframes.Count == 1)
		{
			return keyframes[0].spherePoint;
		}
		if (!GetSurroundingKeyFrames(time, out int beforeIndex, out int afterIndex))
		{
			Debug.LogError("Failed to get surrounding sphere point for time: " + time);
			return null;
		}
		time -= (float)(int)time;
		SpherePointKeyframe keyframe = GetKeyframe(beforeIndex);
		SpherePointKeyframe keyframe2 = GetKeyframe(afterIndex);
		float t = KeyframeGroup<SpherePointKeyframe>.ProgressBetweenSurroundingKeyframes(time, keyframe.time, keyframe2.time);
		float t2 = CurveAdjustedBlendingTime(keyframe.interpolationCurve, t);
		return new SpherePoint(Vector3.Slerp(keyframe.spherePoint.GetWorldDirection(), keyframe2.spherePoint.GetWorldDirection(), t2).normalized);
	}
}
