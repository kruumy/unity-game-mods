using System;
using UnityEngine;

namespace Funly.SkyStudio;

[Serializable]
public class SpherePoint
{
	public float horizontalRotation;

	public float verticalRotation;

	public const float MinHorizontalRotation = -(float)Math.PI;

	public const float MaxHorizontalRotation = (float)Math.PI;

	public const float MinVerticalRotation = -(float)Math.PI / 2f;

	public const float MaxVerticalRotation = (float)Math.PI / 2f;

	public SpherePoint(float horizontalRotation, float verticalRotation)
	{
		this.horizontalRotation = horizontalRotation;
		this.verticalRotation = verticalRotation;
	}

	public SpherePoint(Vector3 worldDirection)
	{
		Vector2 vector = SphereUtility.DirectionToSphericalCoordinate(worldDirection);
		horizontalRotation = vector.x;
		verticalRotation = vector.y;
	}

	public void SetFromWorldDirection(Vector3 worldDirection)
	{
		Vector2 vector = SphereUtility.DirectionToSphericalCoordinate(worldDirection);
		horizontalRotation = vector.x;
		verticalRotation = vector.y;
	}

	public Vector3 GetWorldDirection()
	{
		return SphereUtility.SphericalCoordinateToDirection(new Vector2(horizontalRotation, verticalRotation));
	}
}
