using System;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class ColorKeyframe : BaseKeyframe
{
	public Color color = Color.white;

	public ColorKeyframe(Color c, float time)
		: base(time)
	{
		color = c;
	}

	public ColorKeyframe(ColorKeyframe keyframe)
		: base(keyframe.time)
	{
		color = keyframe.color;
		base.interpolationCurve = keyframe.interpolationCurve;
		base.interpolationDirection = keyframe.interpolationDirection;
	}
}
