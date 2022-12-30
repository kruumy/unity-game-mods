using System;
using UnityEngine;

[Serializable]
public class MSACC_SettingsCameraPlaneX_Z
{
	public enum PlaneXZRotationType
	{
		KeepTheRotationFixed,
		LookAt,
		OptimizedLookAt
	}

	[Header("Limits")]
	[Tooltip("The smallest position on the X axis that the camera can reach.")]
	public float minXPosition = -100f;

	[Tooltip("The largest position on the X axis that the camera can reach.")]
	public float maxXPosition = 100f;

	[Space(5f)]
	[Tooltip("The smallest position on the Z axis that the camera can reach.")]
	public float minZPosition = -100f;

	[Tooltip("The largest position on the Z axis that the camera can reach.")]
	public float maxZPosition = 100f;

	[Header("Camera Height")]
	[Tooltip("The normal position of the camera on the Y axis, when the player is far from the edges of the scene.")]
	public float normalYPosition = 40f;

	[Tooltip("The lowest position the camera can reach on the Y axis when the player is close to an edge.")]
	public float limitYPosition = 22f;

	[Tooltip("The distance of the player in relation to any edge of movement of the camera, so that the camera begins to descend in relation to the ground.")]
	public float edgeDistanceToStartDescending = 50f;

	[Header("Movement")]
	[Range(0.5f, 20f)]
	[Tooltip("The speed at which the camera can follow the player.")]
	public float displacementSpeed = 2f;

	[Header("Rotation")]
	[Tooltip("Here it is possible to define the type of rotation that the camera will have.")]
	public PlaneXZRotationType SelectRotation = PlaneXZRotationType.OptimizedLookAt;

	[Range(0.1f, 20f)]
	[Tooltip("The speed at which the camera rotates as it follows and looks at the player. This variable only has an effect on the 'Optimized LookAt' option.")]
	public float spinSpeedCustomLookAt = 1f;
}
