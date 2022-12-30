using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.Demo;

public class ToggleInvertActive : MonoBehaviour
{
	public Toggle toggle;

	public GameObject icon;

	private void Start()
	{
		icon.SetActive(!toggle.isOn);
		toggle.onValueChanged.AddListener(handleChange);
	}

	private void handleChange(bool arg0)
	{
		icon.SetActive(!arg0);
	}
}
