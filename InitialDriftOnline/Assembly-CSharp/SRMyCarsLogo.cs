using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class SRMyCarsLogo : MonoBehaviour
{
	public GameObject CarsPreviews;

	public GameObject Pistol;

	public GameObject ColorPistol;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void CopyText()
	{
		Application.OpenURL("https://discord.gg/JbdKKsX");
	}

	public void SetCarsIcon()
	{
		CarsPreviews.GetComponent<Image>().sprite = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().MyIcon;
		string carsPlayerPrefName = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().CarsPlayerPrefName;
		if (ObscuredPrefs.GetInt("SkinUse" + carsPlayerPrefName) == 0)
		{
			Pistol.SetActive(value: true);
			ColorPistol.SetActive(value: true);
			Color32 jack = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().jack;
			ColorPistol.GetComponent<Image>().color = jack;
		}
		else
		{
			Pistol.SetActive(value: false);
			ColorPistol.SetActive(value: false);
		}
	}
}
