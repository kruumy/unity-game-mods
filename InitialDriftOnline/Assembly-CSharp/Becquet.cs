using System;
using System.Collections;
using System.IO;
using CodeStage.AntiCheat.Storage;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class Becquet : MonoBehaviour
{
	public GameObject MyAilerons;

	public GameObject MyAileronsDorig;

	private GameObject AileronsLocationOK;

	public GameObject NoSpoilerGo;

	public AudioClip Unlock;

	public AudioClip NoMoney;

	public Text Money;

	[SerializeField]
	public bool EnableUI;

	public GameObject UICommand;

	public GameObject Menu;

	public GameObject MenuGeneral;

	public GameObject ControllerBtnImg;

	public GameObject UiText;

	public GameObject CoveringMen;

	public GameObject SpoilerMenu;

	public GameObject Home;

	public string TradOpenMenu;

	public string TradSpoilerShop;

	public Sprite Xbox;

	public Sprite Keyboard;

	public Sprite PS4;

	public Text SpoilerShopTitle;

	public GameObject SelectedButton;

	[Space]
	public GameObject[] Becquets;

	public GameObject[] AllBecquetsButton;

	public int[] AileronsPrice;

	private string usedctrl;

	public Button HomeBtnSelected;

	private void Start()
	{
	}

	private void Update()
	{
		usedctrl = PlayerPrefs.GetString("ControllerTypeChoose");
		if ((Input.GetKeyDown(KeyCode.Joystick1Button0) && usedctrl == "LogitechSteeringWheel") || (Input.GetKeyDown(KeyCode.Joystick1Button0) && usedctrl == "Xbox360One") || (Input.GetButtonDown("PS4_X") && usedctrl == "PS4"))
		{
			if (!MenuGeneral.activeSelf && EnableUI && !Menu.activeSelf)
			{
				SetPrice();
				Menu.SetActive(value: true);
				CoveringMen.SetActive(value: false);
				SpoilerMenu.SetActive(value: false);
				Home.SetActive(value: true);
			}
		}
		else if (Input.GetKeyDown(KeyCode.E) && !MenuGeneral.activeSelf && EnableUI && !Menu.activeSelf && ObscuredPrefs.GetInt("ONTYPING") == 0 && !MenuGeneral.activeSelf && EnableUI && !Menu.activeSelf)
		{
			SetPrice();
			Menu.SetActive(value: true);
			CoveringMen.SetActive(value: false);
			SpoilerMenu.SetActive(value: false);
			Home.SetActive(value: true);
		}
	}

	public void UICMD(bool jack)
	{
		SpoilerShopTitle.text = TradSpoilerShop;
		MenuGeneral.SetActive(value: false);
		UICommand.SetActive(jack);
		EnableUI = jack;
		if (!jack && Menu.activeSelf)
		{
			Menu.SetActive(value: false);
		}
		if (jack)
		{
			Money.text = ObscuredPrefs.GetInt("MyBalance") + "¥";
			if (PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One")
			{
				UiText.GetComponent<Text>().text = TradOpenMenu;
				ControllerBtnImg.GetComponent<Image>().sprite = Xbox;
			}
			else if (PlayerPrefs.GetString("ControllerTypeChoose") == "PS4")
			{
				UiText.GetComponent<Text>().text = TradOpenMenu;
				ControllerBtnImg.GetComponent<Image>().sprite = PS4;
			}
			else
			{
				UiText.GetComponent<Text>().text = TradOpenMenu;
				ControllerBtnImg.GetComponent<Image>().sprite = Keyboard;
			}
		}
	}

	public void SpawnAilerons(int NumDuBecquet)
	{
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().SetSpoiler(NumDuBecquet);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SetSpoilerFor(NumDuBecquet);
	}

	public void NoSpoiler(GameObject btn)
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().SetSpoiler(-2);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SetSpoilerFor(-2);
		ObscuredPrefs.SetInt("UsedAileronsForFutureSpawn" + text, 100);
		Menu.SetActive(value: false);
		SelectedButton = btn;
		EnableUI = false;
		StartCoroutine(EnUI());
	}

	public void OrigineSpoiler(GameObject btn)
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerFonction>().SetSpoiler(-1);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SetSpoilerFor(-1);
		ObscuredPrefs.SetInt("UsedAileronsForFutureSpawn" + text, 333);
		Menu.SetActive(value: false);
		SelectedButton = btn;
		EnableUI = false;
		StartCoroutine(EnUI());
	}

	public void SetPrice()
	{
		int num = 0;
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		StartCoroutine(HomeSelected());
		GameObject[] allBecquetsButton = AllBecquetsButton;
		foreach (GameObject gameObject in allBecquetsButton)
		{
			if (ObscuredPrefs.GetInt("BuyAilerons" + text + gameObject.name) == 0)
			{
				gameObject.GetComponentInChildren<Text>().text = AileronsPrice[num] + "¥";
				num++;
			}
			else
			{
				gameObject.GetComponentInChildren<Text>().text = "";
			}
		}
	}

	private IEnumerator HomeSelected()
	{
		HomeBtnSelected.GetComponent<Button>().Select();
		HomeBtnSelected.GetComponent<Button>().interactable = false;
		yield return new WaitForSeconds(0.05f);
		HomeBtnSelected.GetComponent<Button>().interactable = true;
		HomeBtnSelected.GetComponent<Button>().Select();
	}

	public void SetBtnSelectedSpoiler()
	{
		if (!SelectedButton)
		{
			SelectedButton = NoSpoilerGo;
		}
		StartCoroutine(SelectBtn());
	}

	private IEnumerator SelectBtn()
	{
		SelectedButton.GetComponent<Button>().Select();
		SelectedButton.GetComponent<Button>().interactable = false;
		yield return new WaitForSeconds(0.05f);
		SelectedButton.GetComponent<Button>().interactable = true;
		SelectedButton.GetComponent<Button>().Select();
	}

	public void BuyAilerons(GameObject btn)
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		int num = Convert.ToInt32(btn.name.Split('(')[1].Replace(")", ""));
		int num2 = 100000;
		if (btn.GetComponentInChildren<Text>().text.Contains("¥"))
		{
			num2 = Convert.ToInt32(btn.GetComponentInChildren<Text>().text.Replace("¥", ""));
		}
		Money.text = ObscuredPrefs.GetInt("MyBalance") + "¥";
		// OLD: if (ObscuredPrefs.GetInt("BuyAilerons" + text + btn.name) == 0 && ObscuredPrefs.GetInt("MyBalance") >= num2)
		if (ObscuredPrefs.GetInt("BuyAilerons" + text + btn.name) == 0 && (ObscuredPrefs.GetInt("MyBalance") >= num2 || File.ReadAllLines("Settings.txt")[2].Split('=')[1] == "true"))
		{
			btn.GetComponentInChildren<Text>().text = "";
			ObscuredPrefs.SetInt("BuyAilerons" + text + btn.name, 10);
			ObscuredPrefs.SetInt("BuyAileronsNumber" + text + num, 10);
			GetComponent<AudioSource>().PlayOneShot(Unlock);
			if(File.ReadAllLines("Settings.txt")[2].Split('=')[1] != "true")
				ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") - num2);
			// OLD: ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") - num2);
			SteamUserStats.SetAchievement("BUYSPOILER");
			SteamUserStats.StoreStats();
		}
		else if (ObscuredPrefs.GetInt("BuyAilerons" + text + btn.name) == 10)
		{
			EnableUI = false;
			StartCoroutine(EnUI());
			SelectedButton = btn;
			Menu.SetActive(value: false);
			ObscuredPrefs.SetInt("UsedAileronsForFutureSpawn" + text, num);
			SpawnAilerons(num);
		}
		// OLD: else if (ObscuredPrefs.GetInt("BuyAilerons" + text + btn.name) == 0 && ObscuredPrefs.GetInt("MyBalance") < num2)
		else if (ObscuredPrefs.GetInt("BuyAilerons" + text + btn.name) == 0 && (ObscuredPrefs.GetInt("MyBalance") < num2 || File.ReadAllLines("Settings.txt")[2].Split('=')[1] == "true"))
		{
			StartCoroutine(NoMoneyColor(btn));
			btn.GetComponentInChildren<Text>().color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
			GetComponent<AudioSource>().PlayOneShot(NoMoney);
		}
		Money.text = ObscuredPrefs.GetInt("MyBalance") + "¥";
	}

	private IEnumerator NoMoneyColor(GameObject btn)
	{
		yield return new WaitForSeconds(1f);
		btn.GetComponentInChildren<Text>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}

	private IEnumerator EnUI()
	{
		yield return new WaitForSeconds(0.3f);
		EnableUI = true;
	}

	public void SetAfterSpawn()
	{
		string text = RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.name.Split(')')[0];
		int @int = ObscuredPrefs.GetInt("UsedAileronsForFutureSpawn" + text);
		if (ObscuredPrefs.GetInt("BuyAileronsNumber" + text + @int) == 10)
		{
			SpawnAilerons(@int);
			return;
		}
		switch (@int)
		{
		case 100:
			SpawnAilerons(-2);
			break;
		case 0:
			if (ObscuredPrefs.GetInt("BuyAileronsNumber" + text + @int) != 0)
			{
				break;
			}
			goto case 333;
		case 333:
			SpawnAilerons(-1);
			break;
		}
	}
}
