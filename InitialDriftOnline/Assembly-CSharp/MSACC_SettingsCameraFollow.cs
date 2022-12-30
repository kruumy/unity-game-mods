using System;
using UnityEngine;

[Serializable]
public class MSACC_SettingsCameraFollow
{
	[Header("Collision")]
	[Tooltip("If this variable is true, the camera ignores the colliders and crosses the walls freely.")]
	public bool ignoreCollision;

	[Header("Movement")]
	[Range(1f, 20f)]
	[Tooltip("The speed at which the camera can follow the player.")]
	public float displacementSpeed = 3f;

	[Header("Rotation")]
	[Tooltip("If this variable is true, the code makes a lookAt using quaternions.")]
	public bool customLookAt;

	[Range(1f, 30f)]
	[Tooltip("The speed at which the camera rotates as it follows and looks at the player.")]
	public float spinSpeedCustomLookAt = 15f;

	[Header("Use Scrool")]
	[Tooltip("If this variable is true, the 'FollowPlayer' camera style will have the player's distance affected by the mouse scrool. This will allow the player to zoom in or out of the camera.")]
	public bool useScrool;

	[Range(0.01f, 2f)]
	[Tooltip("The speed at which the player can zoom in and out of the camera.")]
	public float scroolSpeed = 1f;

	[Range(1f, 30f)]
	[Tooltip("The minimum distance the camera can be relative to the player.")]
	public float minDistance = 7f;

	[Range(1f, 200f)]
	[Tooltip("The maximum distance the camera can be relative to the player.")]
	public float maxDistance = 40f;
}
