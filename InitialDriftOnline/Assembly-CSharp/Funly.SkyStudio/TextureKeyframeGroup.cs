using System;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class TextureKeyframeGroup : KeyframeGroup<TextureKeyframe>
{
	public TextureKeyframeGroup(string name, TextureKeyframe keyframe)
		: base(name)
	{
		AddKeyFrame(keyframe);
	}

	public Texture TextureForTime(float time)
	{
		if (keyframes.Count == 0)
		{
			Debug.LogError("Can't return texture without any keyframes");
			return null;
		}
		if (keyframes.Count == 1)
		{
			return GetKeyframe(0).texture;
		}
		GetSurroundingKeyFrames(time, out int beforeIndex, out int _);
		return GetKeyframe(beforeIndex).texture;
	}
}
