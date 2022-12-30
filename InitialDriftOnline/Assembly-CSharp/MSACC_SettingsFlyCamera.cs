using System;
using UnityEngine;

[Serializable]
public class MSACC_SettingsFlyCamera
{
	[Header("Inputs")]
	[Tooltip("Here you can configure the 'Horizontal' inputs that should be used to move the camera 'CameraFly'.")]
	public string horizontalMove = "Horizontal";

	[Tooltip("Here you can configure the 'Vertical' inputs that should be used to move the camera 'CameraFly'.")]
	public string verticalMove = "Vertical";

	[Tooltip("Here you can configure the keys that must be pressed to accelerate the movement of the camera 'CameraFly'.")]
	public KeyCode speedKeyCode = KeyCode.LeftShift;

	[Tooltip("Here you can configure the key that must be pressed to move the camera 'CameraFly' up.")]
	public KeyCode moveUp = KeyCode.E;

	[Tooltip("Here you can configure the key that must be pressed to move the camera 'CameraFly' down.")]
	public KeyCode moveDown = KeyCode.Q;

	[Header("Settings")]
	[Range(1f, 20f)]
	[Tooltip("Horizontal camera rotation sensitivity.")]
	public float sensibilityX = 10f;

	[Range(1f, 20f)]
	[Tooltip("Vertical camera rotation sensitivity.")]
	public float sensibilityY = 10f;

	[Range(1f, 100f)]
	[Tooltip("The speed of movement of this camera.")]
	public float movementSpeed = 20f;

	[Space(7f)]
	[Tooltip("If this variable is true, the X-axis input will be inverted.")]
	public bool invertXInput;

	[Tooltip("If this variable is true, the X-axis input will be inverted.")]
	public bool invertYInput;
}
