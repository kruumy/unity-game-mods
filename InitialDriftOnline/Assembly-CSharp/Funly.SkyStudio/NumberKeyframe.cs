using System;

namespace Funly.SkyStudio;

[Serializable]
public class NumberKeyframe : BaseKeyframe
{
	public float value;

	public NumberKeyframe(float time, float value)
		: base(time)
	{
		this.value = value;
	}

	public NumberKeyframe(NumberKeyframe keyframe)
		: base(keyframe.time)
	{
		value = keyframe.value;
		base.interpolationCurve = keyframe.interpolationCurve;
		base.interpolationDirection = keyframe.interpolationDirection;
	}
}
