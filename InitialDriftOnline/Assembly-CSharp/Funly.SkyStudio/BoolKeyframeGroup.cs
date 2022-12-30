using System;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class BoolKeyframeGroup : KeyframeGroup<BoolKeyframe>
{
	public BoolKeyframeGroup(string name)
		: base(name)
	{
	}

	public BoolKeyframeGroup(string name, BoolKeyframe keyframe)
		: base(name)
	{
		AddKeyFrame(keyframe);
	}

	public bool BoolForTime(float time)
	{
		if (keyframes.Count == 0)
		{
			Debug.LogError("Can't sample bool without any keyframes");
			return false;
		}
		if (keyframes.Count == 1)
		{
			return keyframes[0].value;
		}
		if (time < keyframes[0].time)
		{
			return keyframes[keyframes.Count - 1].value;
		}
		int index = 0;
		for (int i = 1; i < keyframes.Count && keyframes[i].time <= time; i++)
		{
			index = i;
		}
		return keyframes[index].value;
	}
}
