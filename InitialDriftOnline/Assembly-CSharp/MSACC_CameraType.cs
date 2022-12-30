using System;
using UnityEngine;

[Serializable]
public class MSACC_CameraType
{
	public enum TipoRotac
	{
		LookAtThePlayer,
		FirstPerson,
		FollowPlayer,
		Orbital,
		Stop,
		StraightStop,
		OrbitalThatFollows,
		ETS_StyleCamera,
		FlyCamera_OnlyWindows,
		PlaneX_Z
	}

	[Tooltip("A camera must be associated with this variable. The camera that is associated here, will receive the settings of this index.")]
	public Camera _camera;

	[Tooltip("Here you must select the type of rotation and movement that camera will possess.")]
	public TipoRotac rotationType;

	[Range(0.01f, 1f)]
	[Tooltip("Here you must adjust the volume that the camera attached to this element can perceive. In this way, each camera can perceive a different volume.")]
	public float volume = 1f;
}
