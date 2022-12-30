using UnityEngine;
using UnityEngine.UI;

public class SRViewDistanceManager : MonoBehaviour
{
	public Dropdown DistanceManager;

	public Camera RCCPlayerCam;

	private void Start()
	{
		if (PlayerPrefs.GetFloat("DistanceView") != 0f)
		{
			RCCPlayerCam.farClipPlane = PlayerPrefs.GetFloat("DistanceView");
		}
		DistanceManager.value = PlayerPrefs.GetInt("DistanceViewDropdownValue");
	}

	private void Update()
	{
	}

	public void UpdateValue()
	{
		if (DistanceManager.value == 0)
		{
			RCCPlayerCam.farClipPlane = 250f;
		}
		if (DistanceManager.value == 1)
		{
			RCCPlayerCam.farClipPlane = 400f;
		}
		if (DistanceManager.value == 2)
		{
			RCCPlayerCam.farClipPlane = 500f;
		}
		if (DistanceManager.value == 3)
		{
			RCCPlayerCam.farClipPlane = 750f;
		}
		if (DistanceManager.value == 4)
		{
			RCCPlayerCam.farClipPlane = 1000f;
		}
		if (DistanceManager.value == 5)
		{
			RCCPlayerCam.farClipPlane = 2000f;
		}
		PlayerPrefs.SetFloat("DistanceView", RCCPlayerCam.farClipPlane);
		PlayerPrefs.SetInt("DistanceViewDropdownValue", DistanceManager.value);
	}
}
