using System;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class TextureKeyframe : BaseKeyframe
{
	public Texture texture;

	public TextureKeyframe(Texture texture, float time)
		: base(time)
	{
		this.texture = texture;
	}

	public TextureKeyframe(TextureKeyframe keyframe)
		: base(keyframe.time)
	{
		texture = keyframe.texture;
		base.interpolationCurve = keyframe.interpolationCurve;
		base.interpolationDirection = keyframe.interpolationDirection;
	}
}
