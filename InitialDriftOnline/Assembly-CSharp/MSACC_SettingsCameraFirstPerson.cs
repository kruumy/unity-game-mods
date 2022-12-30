using System;
using UnityEngine;

[Serializable]
public class MSACC_SettingsCameraFirstPerson
{
	[Header("Sensibility")]
	[Range(1f, 20f)]
	[Tooltip("Horizontal camera rotation sensitivity.")]
	public float sensibilityX = 10f;

	[Range(1f, 20f)]
	[Tooltip("Vertical camera rotation sensitivity.")]
	public float sensibilityY = 10f;

	[Range(0f, 1f)]
	[Tooltip("The speed with which the camera can approach your vision through the mouseScrool.")]
	public float speedScroolZoom = 0.5f;

	[Header("Limits")]
	[Range(0f, 360f)]
	[Tooltip("The highest horizontal angle that camera style 'FistPerson' camera can achieve.")]
	public float horizontalAngle = 65f;

	[Range(0f, 85f)]
	[Tooltip("The highest vertical angle that camera style 'FistPerson' camera can achieve.")]
	public float verticalAngle = 20f;

	[Range(0f, 40f)]
	[Tooltip("The maximum the camera can approximate your vision.")]
	public float maxScroolZoom = 30f;

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
