using System.Collections;
using CodeStage.AntiCheat.Storage;
using HSVPicker;
using UnityEngine;
using UnityEngine.UI;

public class SRCheckSkinNumberTuningShop : MonoBehaviour
{
	public GameObject ChangeSkin;

	public GameObject g1;

	public GameObject g2;

	public GameObject g3;

	public Sprite carsicon;

	public Image MySkin0Color;

	public Image StockColor;

	public RenderTexture lol;

	public Image ActualCars;

	public Button SwitchSkinBtn;

	private void Start()
	{
	}

	public void CheckSkin()
	{
		string carsPlayerPrefName = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().CarsPlayerPrefName;
		if (ObscuredPrefs.GetInt("SkinUse" + carsPlayerPrefName) != 0)
		{
			ChangeSkin.SetActive(value: true);
			g1.SetActive(value: false);
			g2.GetComponent<Image>().enabled = true;
			g3.SetActive(value: false);
			carsicon = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().IconSkin[0];
			ChangeSkin.GetComponent<Image>().sprite = carsicon;
			Color32 jack = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().jack;
			jack.a = 180;
			MySkin0Color.color = jack;
			ActualCars.sprite = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().MyIcon;
			StartCoroutine(SelectBtn());
		}
		else
		{
			ChangeSkin.SetActive(value: false);
			g1.SetActive(value: true);
			g2.GetComponent<Image>().enabled = false;
			g3.SetActive(value: true);
		}
		StockColor.color = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().ColorOrig;
	}

	private IEnumerator SelectBtn()
	{
		SwitchSkinBtn.GetComponent<Button>().Select();
		SwitchSkinBtn.GetComponent<Button>().interactable = false;
		yield return new WaitForSeconds(0.05f);
		SwitchSkinBtn.GetComponent<Button>().interactable = true;
		SwitchSkinBtn.GetComponent<Button>().Select();
	}

	public void SetSkin0()
	{
		string carsPlayerPrefName = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().CarsPlayerPrefName;
		ObscuredPrefs.SetInt("SkinNumberInUse", 0);
		ObscuredPrefs.SetInt("SkinUse" + carsPlayerPrefName, 0);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendMySkinModel(0);
		ChangeSkin.SetActive(value: false);
		g1.SetActive(value: true);
		g2.GetComponent<Image>().enabled = false;
		g3.SetActive(value: true);
	}

	public void SetOrigColor()
	{
		g1.GetComponent<ColorPicker>().CurrentColor = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<SkinManager>().ColorOrig;
	}
}
