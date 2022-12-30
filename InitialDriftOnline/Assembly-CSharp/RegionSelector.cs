using UnityEngine;
using UnityEngine.UI;

public class RegionSelector : MonoBehaviour
{
	public Dropdown dropdownRegion;

	private void Start()
	{
		if (PlayerPrefs.GetString("SelectedRegion") == "")
		{
			dropdownRegion.value = 1;
		}
		if (PlayerPrefs.GetString("SelectedRegion") == "eu")
		{
			dropdownRegion.value = 1;
		}
		if (PlayerPrefs.GetString("SelectedRegion") == "ru")
		{
			dropdownRegion.value = 1;
		}
		if (PlayerPrefs.GetString("SelectedRegion") == "us")
		{
			dropdownRegion.value = 2;
		}
		if (PlayerPrefs.GetString("SelectedRegion") == "asia")
		{
			dropdownRegion.value = 3;
		}
		if (PlayerPrefs.GetString("SelectedRegion") == "jp")
		{
			dropdownRegion.value = 3;
		}
		if (PlayerPrefs.GetString("SelectedRegion") == "sa")
		{
			dropdownRegion.value = 4;
		}
		if (PlayerPrefs.GetString("SelectedRegion") == "au")
		{
			dropdownRegion.value = 3;
		}
	}

	public void RegionSelectore()
	{
		if (dropdownRegion.value == 0)
		{
			PlayerPrefs.SetString("SelectedRegion", "eu");
		}
		else if (dropdownRegion.value == 1)
		{
			PlayerPrefs.SetString("SelectedRegion", "eu");
		}
		else if (dropdownRegion.value == 2)
		{
			PlayerPrefs.SetString("SelectedRegion", "us");
		}
		else if (dropdownRegion.value == 3)
		{
			PlayerPrefs.SetString("SelectedRegion", "asia");
		}
		else if (dropdownRegion.value == 4)
		{
			PlayerPrefs.SetString("SelectedRegion", "sa");
		}
		else if (dropdownRegion.value == 5)
		{
			PlayerPrefs.SetString("SelectedRegion", "ru");
		}
	}

	private void Update()
	{
	}
}
