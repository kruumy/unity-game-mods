using System;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class SpherePointKeyframe : BaseKeyframe
{
	public SpherePoint spherePoint;

	public SpherePointKeyframe(SpherePoint spherePoint, float time)
		: base(time)
	{
		if (spherePoint == null)
		{
			Debug.LogError("Passed null sphere point, created empty point");
			this.spherePoint = new SpherePoint(0f, 0f);
		}
		else
		{
			this.spherePoint = spherePoint;
		}
		base.interpolationDirection = InterpolationDirection.Auto;
	}

	public SpherePointKeyframe(SpherePointKeyframe keyframe)
		: base(keyframe.time)
	{
		spherePoint = new SpherePoint(keyframe.spherePoint.horizontalRotation, keyframe.spherePoint.verticalRotation);
		base.interpolationCurve = keyframe.interpolationCurve;
		base.interpolationDirection = keyframe.interpolationDirection;
	}
}
