using System.Collections;
using System.IO;
using CodeStage.AntiCheat.Storage;
using SickscoreGames.HUDNavigationSystem;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class SRNosManager : MonoBehaviour
{
	public GameObject NosIconUI;

	public GameObject NosIconStation;

	public GameObject refuelbtnui;

	public GameObject BG_btnrecharge;

	public GameObject WarnLvl;

	public GameObject HUDOFFNos;

	public int pricerefuel;

	public int Pourcentage;

	private int NoReapet;

	public Text refuelprice;

	public Text pourcentagetxt;

	public bool UsingNos;

	public bool BuyEnabled;

	[Space]
	public int NosLevelMax;

	public int Price31_80_Normal;

	public int Price01_30_Warning;

	public int Price00_OutOfFuel;

	public AudioClip NoMoney;

	public AudioClip Refuel;

	private string usedctrl;

	private void Start()
	{
		if (ObscuredPrefs.GetInt("FLBQ") == 0)
		{
			ObscuredPrefs.SetInt("BoostQuantity", NosLevelMax);
			ObscuredPrefs.SetInt("FLBQ", 1);
		}
		NoReapet = 0;
		refuelbtnui.gameObject.SetActive(value: false);
		BuyEnabled = false;
		if (ObscuredPrefs.GetInt("BoostQuantity") > 0 && ObscuredPrefs.GetInt("BoostQuantity") <= 50)
		{
			NoReapet = 0;
		}
		else if (ObscuredPrefs.GetInt("BoostQuantity") <= 0)
		{
			NoReapet = 1;
		}
		else if (ObscuredPrefs.GetInt("BoostQuantity") > 50)
		{
			NoReapet = 2;
		}
		StartCoroutine(RefreshUItmp());
	}

	private void Update()
	{
		usedctrl = PlayerPrefs.GetString("ControllerTypeChoose");
		if (((Input.GetKeyDown(KeyCode.Joystick1Button0) && usedctrl == "LogitechSteeringWheel") || (Input.GetKeyDown(KeyCode.Joystick1Button0) && usedctrl == "Xbox360One") || (Input.GetButtonDown("PS4_X") && usedctrl == "PS4")) && BuyEnabled)
		{
			RechargeBoostUIYes();
		}
		Pourcentage = ObscuredPrefs.GetInt("BoostQuantity") * 100 / NosLevelMax;
		WarnLvl.GetComponent<Image>().fillAmount = 1f - (float)Pourcentage / 100f;
		if (Pourcentage > 0 && Pourcentage <= 30 && NoReapet == 0)
		{
			NoReapet = 1;
			NosIconUI.SetActive(value: true);
			NosIconUI.GetComponent<Animator>().Play("NosWarning");
			if (PlayerPrefs.GetInt("NbrRefuelNos") < 3 && (bool)NosIconStation)
			{
				NosIconStation.GetComponent<HUDNavigationElement>().showIndicator = true;
			}
			refuelprice.text = Price01_30_Warning + "$";
			pricerefuel = Price01_30_Warning;
		}
		else if (Pourcentage <= 0 && NoReapet == 1)
		{
			NoReapet = 0;
			NosIconUI.SetActive(value: true);
			NosIconUI.GetComponent<Animator>().Play("NosOut");
			if (PlayerPrefs.GetInt("NbrRefuelNos") <= 3 && (bool)NosIconStation)
			{
				NosIconStation.GetComponent<HUDNavigationElement>().showIndicator = true;
			}
			refuelprice.text = Price00_OutOfFuel + "$";
			pricerefuel = Price00_OutOfFuel;
			StartCoroutine(UseNosOff(jack: false));
		}
		else if (Pourcentage > 30 && NoReapet == 2)
		{
			NoReapet = 0;
			NosIconUI.SetActive(value: false);
			if ((bool)NosIconStation)
			{
				NosIconStation.GetComponent<HUDNavigationElement>().showIndicator = false;
			}
		}
	}

	public void ShowUIRefuel(bool jack)
	{
		Pourcentage = ObscuredPrefs.GetInt("BoostQuantity") * 100 / NosLevelMax;
		if (jack && PlayerPrefs.GetInt("HUDOFF") == 1)
		{
			HUDOFFNos.SetActive(value: true);
		}
		if (jack && Pourcentage < 100)
		{
			BG_btnrecharge.SetActive(value: true);
			NosIconUI.SetActive(value: true);
			refuelbtnui.SetActive(value: true);
			BuyEnabled = true;
			if (Pourcentage <= 0)
			{
				pourcentagetxt.text = "0%";
				refuelprice.text = 100 + "$";
				pricerefuel = 100;
			}
			else
			{
				pourcentagetxt.text = Pourcentage + "%";
				refuelprice.text = 100 - Pourcentage + "$";
				pricerefuel = 100 - Pourcentage;
			}
			refuelbtnui.GetComponent<Animator>().Play("LvltxtOn");
		}
		else if (jack && Pourcentage == 100)
		{
			BuyEnabled = false;
			BG_btnrecharge.SetActive(value: false);
			pourcentagetxt.text = Pourcentage + "%";
			NosIconUI.SetActive(value: true);
			refuelbtnui.SetActive(value: true);
			refuelbtnui.GetComponent<Animator>().Play("LvltxtOnJustePourcentage");
		}
		else
		{
			HUDOFFNos.SetActive(value: false);
			BuyEnabled = false;
			pourcentagetxt.text = Pourcentage + "%";
			refuelprice.text = 100 - Pourcentage + "$";
			StartCoroutine(lol());
			if (BG_btnrecharge.activeSelf)
			{
				refuelbtnui.GetComponent<Animator>().Play("LvltxtOff");
			}
			else
			{
				refuelbtnui.GetComponent<Animator>().Play("LvltxtOffJustePourcentage");
			}
		}
		if (Pourcentage > 30 && Pourcentage <= 100 && jack)
		{
			NosIconUI.GetComponent<Animator>().Play("NosNormal");
			refuelprice.text = 100 - Pourcentage + "¥";
			pricerefuel = 100 - Pourcentage;
		}
		else if (Pourcentage > 30 && Pourcentage <= 100 && !jack)
		{
			NosIconUI.GetComponent<Animator>().Play("NosNormalOut");
			refuelprice.text = 100 - Pourcentage + "¥";
			pricerefuel = 100 - Pourcentage;
		}
	}

	public void RechargeBoostUIYes()
	{
		// OLD: if (ObscuredPrefs.GetInt("MyBalance") >= pricerefuel)
		if ((ObscuredPrefs.GetInt("MyBalance") >= pricerefuel) || System.IO.File.ReadAllLines("Settings.txt")[2].Split('=')[1] == "true")
		{
			PlayerPrefs.SetInt("NbrRefuelNos", PlayerPrefs.GetInt("NbrRefuelNos") + 1);
			// OLD: ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") - pricerefuel);
			if(System.IO.File.ReadAllLines("Settings.txt")[2].Split('=')[1] != "true")
				ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") - pricerefuel);
			NoReapet = 2;
			pourcentagetxt.text = 100 + "%";
			NosIconUI.GetComponent<Animator>().Play("NosRecharge");
			refuelbtnui.GetComponent<Animator>().Play("LvltxtOff");
			StartCoroutine(SetPreRecharge());
			StartCoroutine(lol());
			GetComponent<AudioSource>().PlayOneShot(Refuel);
			SteamUserStats.SetAchievement("REFUELNOS");
			SteamUserStats.StoreStats();
		}
		else
		{
			pourcentagetxt.text = Pourcentage + "%";
			StartCoroutine(Money());
			refuelbtnui.GetComponent<Animator>().enabled = false;
			refuelprice.color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
			GetComponent<AudioSource>().PlayOneShot(NoMoney);
		}
		
    }

	private IEnumerator lol()
	{
		yield return new WaitForSeconds(2f);
		GetComponent<AudioSource>().Stop();
		refuelbtnui.SetActive(value: false);
	}

	private IEnumerator Money()
	{
		yield return new WaitForSeconds(1f);
		refuelprice.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		refuelbtnui.GetComponent<Animator>().enabled = true;
	}

	private IEnumerator SetPreRecharge()
	{
		yield return new WaitForSeconds(4f);
		refuelbtnui.SetActive(value: false);
		ObscuredPrefs.SetInt("BoostQuantity", NosLevelMax);
	}

	private IEnumerator UseNosOff()
	{
		yield return new WaitForSeconds(0.2f);
		RCC_SceneManager.Instance.activePlayerVehicle.useNOS = true;
		yield return new WaitForSeconds(0.4f);
		RCC_SceneManager.Instance.activePlayerVehicle.useNOS = false;
		yield return new WaitForSeconds(1f);
		RCC_SceneManager.Instance.activePlayerVehicle.useNOS = false;
	}

	private IEnumerator UseNosOff(bool jack)
	{
		yield return new WaitForSeconds(3f);
		RCC_SceneManager.Instance.activePlayerVehicle.useNOS = jack;
	}

	private IEnumerator RefreshUItmp()
	{
		yield return new WaitForSeconds(5f);
		RefreshUI();
	}

	public void RefreshUI()
	{
		if (Pourcentage > 0 && Pourcentage <= 30)
		{
			NoReapet = 1;
			NosIconUI.SetActive(value: true);
			NosIconUI.GetComponent<Animator>().Play("NosWarning");
			if (PlayerPrefs.GetInt("NbrRefuelNos") < 3 && (bool)NosIconStation)
			{
				NosIconStation.GetComponent<HUDNavigationElement>().showIndicator = true;
			}
			refuelprice.text = Price01_30_Warning + "$";
			pricerefuel = Price01_30_Warning;
		}
		else if (Pourcentage <= 0)
		{
			NoReapet = 0;
			NosIconUI.SetActive(value: true);
			NosIconUI.GetComponent<Animator>().Play("NosOut");
			if (PlayerPrefs.GetInt("NbrRefuelNos") <= 3 && (bool)NosIconStation)
			{
				NosIconStation.GetComponent<HUDNavigationElement>().showIndicator = true;
			}
			refuelprice.text = Price00_OutOfFuel + "$";
			pricerefuel = Price00_OutOfFuel;
			RCC_SceneManager.Instance.activePlayerVehicle.useNOS = false;
		}
		else if (Pourcentage > 30)
		{
			NoReapet = 0;
			NosIconUI.SetActive(value: false);
			if ((bool)NosIconStation)
			{
				NosIconStation.GetComponent<HUDNavigationElement>().showIndicator = false;
			}
		}
	}
}
