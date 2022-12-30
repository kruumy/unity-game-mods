using System;

namespace Funly.SkyStudio;

[Serializable]
public class BoolKeyframe : BaseKeyframe
{
	public bool value;

	public BoolKeyframe(float time, bool value)
		: base(time)
	{
		this.value = value;
	}

	public BoolKeyframe(BoolKeyframe keyframe)
		: base(keyframe.time)
	{
		value = keyframe.value;
		base.interpolationCurve = keyframe.interpolationCurve;
		base.interpolationDirection = keyframe.interpolationDirection;
	}
}
