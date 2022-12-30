using System.Collections;
using CodeStage.AntiCheat.Storage;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using UnityEngine.UI;

public class SRTutoManager : MonoBehaviour
{
	public GameObject TutoManager;

	public GameObject UImanager;

	public GameObject DropdownLanguage;

	public GameObject TextDropdownLanguage;

	public Text Welcome;

	public Text openmenutxt;

	public Animator Anim;

	[Space]
	public GameObject Menu;

	public GameObject DrvierLicence;

	public GameObject Settings;

	public GameObject Help;

	public GameObject Credits;

	public GameObject Leaderboard;

	public GameObject lobby;

	public GameObject exit;

	public Button DrvierLicenceBTN;

	public Button SettingsBTN;

	public Button HelpBTN;

	public Button CreditsBTN;

	public Button LeaderboardBTN;

	[Space]
	[Space]
	public GameObject HudConcess;

	public GameObject HudGarage;

	public GameObject HudTofu;

	private int validation1;

	private int openmenu;

	private int nomenututotwo;

	private int tempodis;

	private bool MenuOK;

	[Space]
	public string InDriverLicence;

	[Space]
	public string InSettings;

	[Space]
	public string InHelp;

	[Space]
	public string InCredit;

	[Space]
	public string InLeaderboard;

	[Space]
	public string GoToTheCarsDealer;

	[Space]
	public string InfoCarsDealer;

	[Space]
	public string GoToTheGarage;

	[Space]
	public string InfoGarage;

	[Space]
	public string Tips;

	[Space]
	public string InfoTofuNeedTranslate;

	private void Start()
	{
		if (PlayerPrefs.GetInt("TutoOK") == 0)
		{
			MenuOK = false;
			if (PlayerPrefs.GetInt("LanguageFirstSet") == 0)
			{
				ObscuredPrefs.GetFloat("TOTALTIME");
				_ = 1800f;
			}
			if (ObscuredPrefs.GetInt("TutoConcessRuning") == 0)
			{
				HudConcess.GetComponent<HUDNavigationElement>().showIndicator = true;
			}
			if (ObscuredPrefs.GetInt("TutoGaragRuning") == 0)
			{
				HudGarage.GetComponent<HUDNavigationElement>().showIndicator = true;
			}
			if (ObscuredPrefs.GetInt("TutoTofuRuing") == 0)
			{
				HudTofu.GetComponent<HUDNavigationElement>().showIndicator = true;
			}
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (Menu.activeSelf && !MenuOK)
		{
			Debug.Log("MENU OK");
			MenuOK = true;
			openmenutxt.text = "";
			Anim.enabled = false;
			openmenutxt.text = "";
		}
	}

	private IEnumerator RCCOFF()
	{
		yield return new WaitForSeconds(0.6f);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;
	}

	public void LanguageOK()
	{
		Debug.Log("OK POUR CA");
		PlayerPrefs.SetInt("LanguageFirstSet", 1);
		Anim.enabled = true;
	}

	public void InConcess()
	{
		StopAllCoroutines();
		Debug.Log("CONCESS OK");
		openmenutxt.text = InfoCarsDealer;
		ObscuredPrefs.SetInt("TutoConcessRuning", 10);
		StartCoroutine(Concessok());
	}

	private IEnumerator Concessok()
	{
		yield return new WaitForSeconds(15f);
		openmenutxt.text = "";
		HudConcess.GetComponent<HUDNavigationElement>().showIndicator = false;
	}

	public void InGarage()
	{
		StopAllCoroutines();
		Debug.Log("GARAGE OK");
		openmenutxt.text = InfoGarage;
		ObscuredPrefs.SetInt("TutoGaragRuning", 10);
		StartCoroutine(GarageOK());
	}

	private IEnumerator GarageOK()
	{
		yield return new WaitForSeconds(15f);
		openmenutxt.text = "";
		HudGarage.GetComponent<HUDNavigationElement>().showIndicator = false;
		yield return new WaitForSeconds(4f);
		openmenutxt.text = Tips;
		yield return new WaitForSeconds(10f);
		openmenutxt.text = "";
	}

	public void InTofu()
	{
		StopAllCoroutines();
		Debug.Log("TOFU OK");
		openmenutxt.text = InfoTofuNeedTranslate;
		ObscuredPrefs.SetInt("TutoTofuRuing", 10);
		StartCoroutine(TofuOk());
	}

	private IEnumerator TofuOk()
	{
		yield return new WaitForSeconds(15f);
		openmenutxt.text = "";
		HudTofu.GetComponent<HUDNavigationElement>().showIndicator = false;
	}
}
