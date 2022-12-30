using System;
using UnityEngine;

[Serializable]
public class MSACC_SettingsCameraETS_StyleCamera
{
	[Header("Settings")]
	[Range(1f, 20f)]
	[Tooltip("Horizontal camera rotation sensitivity.")]
	public float sensibilityX = 10f;

	[Range(1f, 20f)]
	[Tooltip("Vertical camera rotation sensitivity.")]
	public float sensibilityY = 10f;

	[Range(0.5f, 3f)]
	[Tooltip("The distance the camera will move to the left when the mouse is also shifted to the left. This option applies only to cameras that have the 'ETS_StyleCamera' option selected.")]
	public float ETS_CameraShift = 1f;

	[Range(0f, 40f)]
	[Tooltip("The maximum the camera can approximate your vision.")]
	public float maxScroolZoom = 30f;

	[Range(0f, 1f)]
	[Tooltip("The speed with which the camera can approach your vision through the mouseScrool.")]
	public float speedScroolZoom = 0.5f;

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
