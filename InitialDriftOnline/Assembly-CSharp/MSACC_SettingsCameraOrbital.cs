using System;
using UnityEngine;

[Serializable]
public class MSACC_SettingsCameraOrbital
{
	[Header("Settings")]
	[Range(0.01f, 2f)]
	[Tooltip("In this variable you can configure the sensitivity with which the script will perceive the movement of the X and Y inputs. ")]
	public float sensibility = 0.8f;

	[Range(0.01f, 2f)]
	[Tooltip("In this variable, you can configure the speed at which the orbital camera will approach or distance itself from the player when the mouse scrool is used.")]
	public float speedScrool = 1f;

	[Range(0.01f, 2f)]
	[Tooltip("In this variable, you can configure the speed at which the orbital camera moves up or down.")]
	public float speedYAxis = 0.5f;

	[Header("Limits")]
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

	[Tooltip("If this variable is true, the camera ignores the colliders and crosses the walls freely.")]
	public bool ignoreCollision;

	[Header("Custom Rotation Input")]
	[Tooltip("If this variable is true, the camera will only rotate when the key selected in the 'KeyToRotate' variable is pressed. If this variable is false, the camera can rotate freely, even without pressing any key.")]
	public bool rotateWhenClick;

	[Tooltip("Here you can select the button that must be pressed in order to rotate the camera.")]
	public string keyToRotate = "mouse 0";

	[Space(7f)]
	[Tooltip("If this variable is true, the X-axis input will be inverted.")]
	public bool invertXInput;

	[Tooltip("If this variable is true, the X-axis input will be inverted.")]
	public bool invertYInput;
}
