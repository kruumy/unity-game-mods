using UnityEngine;
using UnityEngine.XR;

public class RCC_XRToggle : MonoBehaviour
{
	public bool XREnabled;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			ToggleXR();
		}
	}

	private void ToggleXR()
	{
		XRSettings.enabled = !XRSettings.enabled;
		XREnabled = XRSettings.enabled;
	}
}
