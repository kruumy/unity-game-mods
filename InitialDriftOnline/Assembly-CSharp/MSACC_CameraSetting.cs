using System;
using UnityEngine;

[Serializable]
public class MSACC_CameraSetting
{
	public enum UpdateMode
	{
		Update,
		FixedUpdate,
		LateUpdate
	}

	[Header("Configure Inputs")]
	[Tooltip("The input that will define the horizontal movement of the cameras.")]
	public string inputMouseX = "Mouse X";

	[Tooltip("The input that will define the vertical movement of the cameras.")]
	public string inputMouseY = "Mouse Y";

	[Tooltip("The input that allows you to zoom in or out of the camera.")]
	public string inputMouseScrollWheel = "Mouse ScrollWheel";

	[Tooltip("In this variable you can configure the key responsible for switching cameras.")]
	public KeyCode cameraSwitchKey = KeyCode.C;

	[Header("Update mode")]
	[Tooltip("Here it is possible to decide whether the motion of the cameras will be processed in the void Update, FixedUpdate or LateUpdate. The mode that best suits most situations is the 'LateUpdate'.")]
	public UpdateMode camerasUpdateMode = UpdateMode.LateUpdate;

	[Header("General settings")]
	[Tooltip("If this variable is checked, the script will automatically place the 'IgnoreRaycast' layer on the player when needed.")]
	public bool ajustTheLayers = true;

	[Tooltip("In this class you can configure the 'FirstPerson' style cameras.")]
	public MSACC_SettingsCameraFirstPerson firstPerson;

	[Tooltip("In this class you can configure the 'FollowPlayer' style cameras.")]
	public MSACC_SettingsCameraFollow followPlayer;

	[Tooltip("In this class you can configure the 'Orbital' style cameras.")]
	public MSACC_SettingsCameraOrbital orbital;

	[Tooltip("In this class you can configure the 'OrbitalThatFollows' style cameras.")]
	public MSACC_SettingsCameraOrbitalThatFollows OrbitalThatFollows;

	[Tooltip("In this class you can configure the 'ETS_StyleCamera' style cameras.")]
	public MSACC_SettingsCameraETS_StyleCamera ETS_StyleCamera;

	[Tooltip("In this class you can configure the 'FlyCamera' style cameras.")]
	public MSACC_SettingsFlyCamera FlyCamera_OnlyWindows;

	[Tooltip("In this class you can configure the 'PlaneX_Y' style cameras.")]
	public MSACC_SettingsCameraPlaneX_Z PlaneX_Z;
}
