using System;
using UnityEngine;

[Serializable]
public class MSACC_SettingsCameraOrbitalThatFollows
{
	public enum ResetTimeType
	{
		Time,
		Input_OnlyWindows
	}

	[Header("Settings(Follow)")]
	[Range(1f, 20f)]
	[Tooltip("The speed at which the camera can follow the player.")]
	public float displacementSpeed = 5f;

	[Tooltip("If this variable is true, the code makes a lookAt using quaternions.")]
	public bool customLookAt;

	[Range(1f, 30f)]
	[Tooltip("The speed at which the camera rotates as it follows and looks at the player.")]
	public float spinSpeedCustomLookAt = 15f;

	[Header("Settings(Orbital)")]
	[Range(0.01f, 2f)]
	[Tooltip("In this variable you can configure the sensitivity with which the script will perceive the movement of the X and Y inputs. ")]
	public float sensibility = 0.8f;

	[Range(0.01f, 2f)]
	[Tooltip("In this variable, you can configure the speed at which the orbital camera will approach or distance itself from the player when the mouse scrool is used.")]
	public float speedScrool = 1f;

	[Range(0.01f, 2f)]
	[Tooltip("In this variable, you can configure the speed at which the orbital camera moves up or down.")]
	public float speedYAxis = 0.5f;

	[Range(3f, 20f)]
	[Tooltip("In this variable, you can set the minimum distance that the orbital camera can stay from the player.")]
	public float minDistance = 5f;

	[Range(20f, 1000f)]
	[Tooltip("In this variable, you can set the maximum distance that the orbital camera can stay from the player.")]
	public float maxDistance = 50f;

	[Range(-85f, 0f)]
	[Tooltip("In this variable it is possible to define the minimum angle that the camera can reach on the Y axis")]
	public float minAngleY;

	[Range(0f, 85f)]
	[Tooltip("In this variable it is possible to define the maximum angle that the camera can reach on the Y axis")]
	public float maxAngleY = 80f;

	[Space(7f)]
	[Tooltip("If this variable is true, the camera will only rotate when the key selected in the 'KeyToRotate' variable is pressed. If this variable is false, the camera can rotate freely, even without pressing any key.")]
	public bool rotateWhenClick;

	[Tooltip("Here you can select the button that must be pressed in order to rotate the camera.")]
	public string keyToRotate = "mouse 0";

	[Tooltip("If this variable is true, the X-axis input will be inverted.")]
	public bool invertXInput;

	[Tooltip("If this variable is true, the X-axis input will be inverted.")]
	public bool invertYInput;

	[Header("Settings(General)")]
	[Tooltip("In this variable it is possible to define how the control will be redefined for the camera that follows the player, through input or through a time.")]
	public ResetTimeType ResetControlType;

	[Tooltip("If 'ResetControlType' is set to 'Input_OnlyWindows', the key that must be pressed to reset the control will be set by this variable.")]
	public KeyCode resetKey = KeyCode.Z;

	[Range(1f, 50f)]
	[Tooltip("If 'ResetControlType' is set to 'Time', the wait time for the camera to reset the control will be set by this variable.")]
	public float timeToReset = 8f;

	[Tooltip("If this variable is true, the camera ignores the colliders and crosses the walls freely.")]
	public bool ignoreCollision;
}
