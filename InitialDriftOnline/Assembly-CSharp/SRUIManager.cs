using System;
using System.Collections;
using System.IO;
using CodeStage.AntiCheat.Storage;
using HeathenEngineering.SteamApi.PlayerServices;
using Photon.Pun;
using SpielmannSpiel_Launcher;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SRUIManager : MonoBehaviour
{
	public GameObject MenuGeneral;

	public GameObject MenuDriverLicence;

	public GameObject MenuSettings;

	public GameObject MenuHelp;

	public GameObject MenuCredit;

	public GameObject MenuLeaderboard;

	public GameObject XboxSprite;

	public GameObject KeyboardSprite;

	public GameObject LevelProgessBar;

	public GameObject MenuExit;

	public GameObject FadeUI;

	public GameObject Concess;

	public GameObject Garage;

	public Button Btn_DriverLicence;

	public Button Btn_Settings;

	public Button Btn_Help;

	public Button Btn_Credit;

	public Button XBOX;

	public Button Keyboard;

	public Button Btn_CancelExit;

	public Button Btn_Leaderboard;

	public Text Money;

	public Text PlayingTime;

	public Text PourcentageText;

	public Text LevelText;

	public Text LevelTextLB;

	[Space]
	public Text AkinaRunCount2;

	public Text AkagiRunCount2;

	public Text IroRunCount2;

	public Text USUI_RUNCOUNT;

	[Space]
	public Text AkinaBestTime2;

	public Text AkagiBestTime2;

	public Text IroBestTime2;

	public Text USUI_BESTTIME;

	[Space]
	public Text AkinaBestTimeReverse;

	public Text AkagiBestTimeReverse;

	public Text IroBestTimeReverse;

	public Text USUI_BESTTIME_REVERSE;

	[Space]
	public Image HarunaDownHilleCarsIcon;

	public Image HarunaUpHillCarsIcon;

	public Image IroDownHillCarsIcon;

	public Image IroUpHillCarsIcon;

	public Image AkagiUpHillCarsIco;

	public Image AkagiDownHillCarsIcon;

	public Image USUIUpHillCarsIcon;

	public Image USUIDownHillCarsIcon;

	public Sprite[] CarsIcon;

	[Space]
	private int Level;

	[Space]
	public ProgressBar LVL;

	public GameObject Dlint;

	public GameObject SettingsInt;

	public GameObject HelpInt;

	public GameObject CreditInt;

	public GameObject BtnLobby;

	public GameObject BtnLobbyExt;

	public GameObject BtnExit;

	public GameObject BtnExitInt;

	public GameObject LB1;

	public GameObject LB2;

	public GameObject LB3;

	public GameObject LB4;

	public GameObject LB5;

	public GameObject LB6;

	public GameObject LB7;

	public GameObject LB8;

	public GameObject LB9;

	public GameObject LB10;

	public GameObject LB11;

	public GameObject LB12;

	public GameObject LB13;

	public GameObject LB14;

	public GameObject ButtonListGarage;

	public GameObject WarnLvl;

	public Camera RCCPlayerCam;

	[Space]
	public Text TotalBattleWin;

	[Space]
	public Text PourcentageNOS;

	[Space]
	public Text TotalBattleWinLB;

	public string BattleWintrad;

	public string Moneytrad;

	public string PlayingTimetrad;

	[Space]
	public GameObject TofuUI;

	[Space]
	public GameObject TuningShopUI;

	private float ActualTime;

	private float LastTime;

	private float AddingTime;

	private float TimetimeSheet;

	private int Minute;

	private int Hour;

	private int PourcentageNoss;

	private int TempoLogi;

	[Space]
	public GameObject LB3DUP_HILL;

	[Space]
	public GameObject LB3DDOWN_HILL;

	[Space]
	public GameObject PS4TUTO;

	[Space]
	public GameObject LOGITUTO;

	private string usedctrl;

	[Space]
	public GameObject TuningShopUIHome;

	public GameObject TuningShopUISpoiler;

	public GameObject TuningShopUICovering;

	[Space]
	public SteamDataLibrary datalib;

	[Space]
	public GameObject Crown;

	[Space]
	public GameObject Mp3FileBrowser;

	private void Start()
	{
		if (PlayerPrefs.GetFloat("DistanceView") != 0f)
		{
			RCCPlayerCam.farClipPlane = PlayerPrefs.GetFloat("DistanceView");
		}
		ObscuredPrefs.SetInt("MyLvl", ObscuredPrefs.GetInt("XP") / 100);
		Default();
		SetButtonBase();
		StartCoroutine(AchievementTimer());
		TimetimeSheet = Time.time;
		Crown.SetActive(value: false);
		LB3DUP_HILL.SetActive(value: true);
		LB3DDOWN_HILL.SetActive(value: true);
		LB3DDOWN_HILL.GetComponent<SR3DLB>().EnableLBB(jack: true);
		LB3DUP_HILL.GetComponent<SR3DLB>().EnableLBB(jack: true);
		TempoLogi = 0;
		PlayerPrefs.SetFloat("actualtimetimer", Time.time);
	}

	private void Update()
	{
		usedctrl = PlayerPrefs.GetString("ControllerTypeChoose");
		if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.Joystick1Button7) && usedctrl == "Xbox360One") || (Input.GetKeyDown(KeyCode.Joystick1Button7) && usedctrl == "Keyboard") || (RCC_LogitechSteeringWheel.GetKeyPressed(0, 6) && usedctrl == "LogitechSteeringWheel" && TempoLogi == 0) || (Input.GetButtonDown("PS4_Options") && TempoLogi == 0 && usedctrl == "PS4"))
		{
			MainCam(jack: true);
		}
		if ((Input.GetKeyDown(KeyCode.Joystick1Button1) && usedctrl == "Xbox360One") || (RCC_LogitechSteeringWheel.GetKeyPressed(0, 1) && usedctrl == "LogitechSteeringWheel") || (Input.GetButtonDown("PS4_Circle") && usedctrl == "PS4"))
		{
			MainCam(jack: false);
		}
		if (MenuExit.activeSelf && ((Input.GetKeyDown(KeyCode.Joystick1Button1) && usedctrl == "Xbox360One") || Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.Joystick1Button7) && usedctrl == "Xbox360One") || (Input.GetButtonDown("PS4_Options") && usedctrl == "PS4")))
		{
			ExitMenuNo();
		}
		if (ObscuredPrefs.GetInt("MyLvl") >= 1000 && ObscuredPrefs.GetInt("TOTALWINMONEY") < 5000)
		{
			ObscuredPrefs.SetInt("MyLvl", 0);
			ObscuredPrefs.SetInt("XP", 0);
		}
	}

	public void MainCam(bool jack)
	{
		if (!jack)
		{
			CloseMenu();
		}
		if (UnityEngine.Object.FindObjectOfType<RCC_Camera>().playerCar != RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>())
		{
			GetComponent<SRPlayerListRoom>().CaseParPlayer[0].GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().SetMineCam();
		}
		else if (!UnityEngine.Object.FindObjectOfType<RCC_Camera>().enabled)
		{
			UnityEngine.Object.FindObjectOfType<SRFreeCam>().SetFreemCam(jack: false);
		}
		else if (jack)
		{
			OpenMenu();
		}
	}

	private IEnumerator TempoForLigoMenu()
	{
		yield return new WaitForSeconds(0.4f);
		TempoLogi = 0;
	}

	public void OpenMenu()
	{
		StartCoroutine(TempoForLigoMenu());
		TempoLogi = 1;
		if (LOGITUTO.activeSelf || PS4TUTO.activeSelf || LB1.activeSelf || LB2.activeSelf || LB3.activeSelf || LB4.activeSelf || LB5.activeSelf || LB6.activeSelf || LB7.activeSelf || LB8.activeSelf || LB9.activeSelf || LB10.activeSelf || LB11.activeSelf || LB12.activeSelf || LB13.activeSelf || LB14.activeSelf)
		{
			Default();
		}
		else if (MenuGeneral.activeSelf)
		{
			MenuGeneral.SetActive(value: false);
			GetComponent<LauncherManager>().activateSettings();
		}
		else if (Concess.activeSelf || Garage.activeSelf || TuningShopUI.activeSelf)
		{
			Concess.SetActive(value: false);
			Garage.SetActive(value: false);
			TuningShopUI.SetActive(value: false);
		}
		else if (!TofuUI.activeSelf)
		{
			MenuGeneral.SetActive(value: true);
			StopAllCoroutines();
			Default();
			XpLvlSystem();
			SetValue();
			StartCoroutine(AchievementTimer());
			UpdateTime();
			UnityEngine.Object.FindObjectOfType<SRShadowManager>().SetMiniMapHigh();
		}
	}

	public void CloseMenu()
	{
		StartCoroutine(TempoForLigoMenu());
		if (LB1.activeSelf || LB2.activeSelf || LB3.activeSelf || LB4.activeSelf || LB5.activeSelf || LB6.activeSelf || LB7.activeSelf || LB8.activeSelf || LB9.activeSelf || LB10.activeSelf || LB11.activeSelf || LB12.activeSelf || LB13.activeSelf || LB14.activeSelf)
		{
			LeaderboardeMenu();
			LBMENU2();
		}
		else if (LOGITUTO.activeSelf || PS4TUTO.activeSelf || Mp3FileBrowser.activeSelf)
		{
			PS4TUTO.SetActive(value: false);
			LOGITUTO.SetActive(value: false);
			Mp3FileBrowser.SetActive(value: false);
		}
		else if (MenuGeneral.activeSelf)
		{
			MenuGeneral.SetActive(value: false);
			GetComponent<LauncherManager>().activateSettings();
		}
		else if (Garage.activeSelf)
		{
			Debug.Log("1111");
			if (!ButtonListGarage.activeSelf)
			{
				Garage.GetComponent<GarageManager>().HGomeGarage();
				Garage.gameObject.GetComponentInChildren<CarsPreview>().DisableUICam();
				GetComponentInChildren<RCC_CustomizerExample>().SaveStatsTemp();
				Debug.Log("OKKKK");
			}
			else
			{
				Debug.Log("CEST UN PEU JEEENNRE");
				Garage.SetActive(value: false);
			}
		}
		else if (Concess.activeSelf)
		{
			Concess.SetActive(value: false);
			Garage.SetActive(value: false);
			Debug.Log("22222");
		}
		else if (TuningShopUI.activeSelf)
		{
			if (TuningShopUIHome.activeSelf)
			{
				TuningShopUI.SetActive(value: false);
				return;
			}
			TuningShopUIHome.SetActive(value: true);
			TuningShopUISpoiler.SetActive(value: false);
			TuningShopUICovering.SetActive(value: false);
		}
	}

	public void UpdateTime()
	{
		LastTime = ActualTime;
		ActualTime = Time.time - TimetimeSheet;
		AddingTime = ActualTime - LastTime;
		ObscuredPrefs.SetFloat("TOTALTIME", ObscuredPrefs.GetFloat("TOTALTIME") + AddingTime);
		int num = Convert.ToInt32(ObscuredPrefs.GetFloat("TOTALTIME"));
		Minute = num / 60;
		if (Minute < 60)
		{
			PlayingTime.text = PlayingTimetrad + ":\n00:" + Minute;
		}
		else
		{
			int num2 = Minute / 60;
			int num3 = Minute - num2 * 60;
			PlayingTime.text = PlayingTimetrad + ":\n" + num2 + "h" + num3;
			if (Minute / 60 >= 1)
			{
				SteamUserStats.SetAchievement("TIME1H");
				SteamUserStats.StoreStats();
			}
			if (Minute / 60 >= 10)
			{
				SteamUserStats.SetAchievement("TIME10H");
				SteamUserStats.StoreStats();
			}
			if (Minute / 60 >= 50)
			{
				SteamUserStats.SetAchievement("TIME50H");
				SteamUserStats.StoreStats();
			}
			if (Minute / 60 >= 100)
			{
				SteamUserStats.SetAchievement("TIME100H");
				SteamUserStats.StoreStats();
			}
			StartCoroutine(TimeUpTempo());
		}
		PourcentageNoss = ObscuredPrefs.GetInt("BoostQuantity") * 100 / UnityEngine.Object.FindObjectOfType<SRNosManager>().NosLevelMax;
		if (PourcentageNoss < 0)
		{
			PourcentageNOS.text = "0%";
		}
		else
		{
			PourcentageNOS.text = PourcentageNoss + "%";
		}
		WarnLvl.GetComponent<Image>().fillAmount = 1f - (float)PourcentageNoss / 100f;
	}

	private IEnumerator TimeUpTempo()
	{
		yield return new WaitForSeconds(30f);
		UpdateTime();
	}

	public void Default()
	{
		SetButtonBase();
		Mp3FileBrowser.SetActive(value: false);
		PS4TUTO.SetActive(value: false);
		LOGITUTO.SetActive(value: false);
		MenuDriverLicence.SetActive(value: true);
		MenuSettings.SetActive(value: false);
		MenuHelp.SetActive(value: false);
		MenuCredit.SetActive(value: false);
		MenuLeaderboard.SetActive(value: false);
		MenuExit.SetActive(value: false);
		Btn_Settings.interactable = true;
		Btn_Help.interactable = true;
		Btn_Credit.interactable = true;
		Keyboard.interactable = true;
		XboxSprite.SetActive(value: true);
		KeyboardSprite.SetActive(value: false);
		Btn_DriverLicence.interactable = false;
		Btn_DriverLicence.Select();
		Btn_DriverLicence.interactable = true;
		Btn_DriverLicence.Select();
		if ((bool)UnityEngine.Object.FindObjectOfType<SRDLCManager>())
		{
			UnityEngine.Object.FindObjectOfType<SRDLCManager>().moretunebtn();
		}
		if ((bool)UnityEngine.Object.FindObjectOfType<SRMyCarsLogo>())
		{
			UnityEngine.Object.FindObjectOfType<SRMyCarsLogo>().SetCarsIcon();
		}
	}

	public void DriverLicenceMenu()
	{
		MenuDriverLicence.SetActive(value: true);
		MenuSettings.SetActive(value: false);
		MenuLeaderboard.SetActive(value: false);
		MenuHelp.SetActive(value: false);
		MenuCredit.SetActive(value: false);
		Btn_Settings.interactable = true;
		Btn_Help.interactable = true;
		Btn_Credit.interactable = true;
		UnityEngine.Object.FindObjectOfType<SRMyCarsLogo>().SetCarsIcon();
	}

	public void DisableOtherbtn()
	{
		Btn_Settings.interactable = false;
		Btn_Help.interactable = false;
		Btn_Credit.interactable = false;
		Btn_DriverLicence.interactable = false;
	}

	public void SettingsMenu()
	{
		Mp3FileBrowser.SetActive(value: false);
		MenuDriverLicence.SetActive(value: false);
		MenuSettings.SetActive(value: true);
		MenuHelp.SetActive(value: false);
		MenuLeaderboard.SetActive(value: false);
		MenuCredit.SetActive(value: false);
		Btn_DriverLicence.interactable = true;
		Btn_Help.interactable = true;
		Btn_Credit.interactable = true;
		Btn_Settings.GetComponent<Button>().Select();
		UnityEngine.Object.FindObjectOfType<SRSoundManager>().SetBtnAnim();
	}

	public void HelpMenu()
	{
		MenuDriverLicence.SetActive(value: false);
		MenuSettings.SetActive(value: false);
		MenuHelp.SetActive(value: true);
		MenuLeaderboard.SetActive(value: false);
		MenuCredit.SetActive(value: false);
		Btn_DriverLicence.interactable = true;
		Btn_Settings.interactable = true;
		Btn_Credit.interactable = true;
	}

	public void CreditMenu()
	{
		MenuDriverLicence.SetActive(value: false);
		MenuSettings.SetActive(value: false);
		MenuLeaderboard.SetActive(value: false);
		MenuHelp.SetActive(value: false);
		MenuCredit.SetActive(value: true);
		Btn_DriverLicence.interactable = true;
		Btn_Settings.interactable = true;
		Btn_Help.interactable = true;
		Btn_Credit.GetComponent<Button>().Select();
	}

	public void LeaderboardeMenu()
	{
		LB1.SetActive(value: false);
		LB2.SetActive(value: false);
		LB3.SetActive(value: false);
		LB4.SetActive(value: false);
		LB5.SetActive(value: false);
		LB6.SetActive(value: false);
		LB7.SetActive(value: false);
		LB8.SetActive(value: false);
		LB9.SetActive(value: false);
		LB10.SetActive(value: false);
		LB11.SetActive(value: false);
		LB12.SetActive(value: false);
		LB13.SetActive(value: false);
		LB14.SetActive(value: false);
		MenuDriverLicence.SetActive(value: false);
		MenuSettings.SetActive(value: false);
		MenuHelp.SetActive(value: false);
		MenuCredit.SetActive(value: false);
		MenuLeaderboard.SetActive(value: true);
		Btn_Settings.interactable = true;
		Btn_Help.interactable = true;
		Btn_Credit.interactable = true;
		Btn_DriverLicence.interactable = true;
	}

	public void LBMENU2()
	{
		Btn_Leaderboard.interactable = false;
		Btn_Leaderboard.interactable = true;
		Btn_Leaderboard.GetComponent<Button>().Select();
		UnityEngine.Object.FindObjectOfType<LeaderboardUsersManager>().Start();
	}

	public void DLCCashPage()
	{
		UnityEngine.Object.FindObjectOfType<SRDLCManager>().Cashpage();
	}

	public void XboxOn()
	{
		Keyboard.interactable = true;
		XboxSprite.SetActive(value: true);
		KeyboardSprite.SetActive(value: false);
	}

	public void KeyboardOn()
	{
		XBOX.interactable = true;
		XboxSprite.SetActive(value: false);
		KeyboardSprite.SetActive(value: true);
	}

	public void SetValue()
	{
		HarunaDownHilleCarsIcon.sprite = CarsIcon[ObscuredPrefs.GetInt("UsedCarsAkinaNew")];
		HarunaUpHillCarsIcon.sprite = CarsIcon[ObscuredPrefs.GetInt("UsedCarsAkinaReverseNew")];
		IroDownHillCarsIcon.sprite = CarsIcon[ObscuredPrefs.GetInt("UsedCarsIrohazakaNew")];
		IroUpHillCarsIcon.sprite = CarsIcon[ObscuredPrefs.GetInt("UsedCarsIrohazakaReverseNew")];
		AkagiUpHillCarsIco.sprite = CarsIcon[ObscuredPrefs.GetInt("UsedCarsAkagiNew")];
		AkagiDownHillCarsIcon.sprite = CarsIcon[ObscuredPrefs.GetInt("UsedCarsAkagiReverseNew")];
		USUIUpHillCarsIcon.sprite = CarsIcon[ObscuredPrefs.GetInt("UsedCarsUSUIReverseNew")];
		USUIDownHillCarsIcon.sprite = CarsIcon[ObscuredPrefs.GetInt("UsedCarsUSUINew")];
		if (ObscuredPrefs.GetInt("BestRunTimeAkinaNew") < 210)
		{
			ObscuredPrefs.SetInt("BestRunTimeAkinaNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeIrohazakaNew") < 210)
		{
			ObscuredPrefs.SetInt("BestRunTimeIrohazakaNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeAkagiNew") < 110)
		{
			ObscuredPrefs.SetInt("BestRunTimeAkagiNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeAkinaReverseNew") < 130)
		{
			ObscuredPrefs.SetInt("BestRunTimeAkinaReverseNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeIrohazakaReverseNew") < 210)
		{
			ObscuredPrefs.SetInt("BestRunTimeIrohazakaReverseNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeAkagiReverseNew") < 110)
		{
			ObscuredPrefs.SetInt("BestRunTimeAkagiReverseNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeUSUIReverseNew") < 110)
		{
			ObscuredPrefs.SetInt("BestRunTimeUSUIReverseNew", 0);
		}
		if (ObscuredPrefs.GetInt("BestRunTimeUSUINew") < 110)
		{
			ObscuredPrefs.SetInt("BestRunTimeUSUINew", 0);
		}
		AkinaRunCount2.text = string.Concat(ObscuredPrefs.GetInt("RunCountAkina"));
		AkagiRunCount2.text = string.Concat(ObscuredPrefs.GetInt("RunCountAkagi"));
		IroRunCount2.text = string.Concat(ObscuredPrefs.GetInt("RunCountIrohazaka"));
		AkinaBestTime2.text = ObscuredPrefs.GetInt("BestRunTimeAkinaNew") + "<size=10> 's </size>";
		AkagiBestTime2.text = ObscuredPrefs.GetInt("BestRunTimeAkagiNew") + "<size=10> 's </size>";
		IroBestTime2.text = ObscuredPrefs.GetInt("BestRunTimeIrohazakaNew") + "<size=10> 's </size>";
		AkinaBestTimeReverse.text = ObscuredPrefs.GetInt("BestRunTimeAkinaReverseNew") + "<size=10> 's </size>";
		AkagiBestTimeReverse.text = ObscuredPrefs.GetInt("BestRunTimeAkagiReverseNew") + "<size=10> 's </size>";
		IroBestTimeReverse.text = ObscuredPrefs.GetInt("BestRunTimeIrohazakaReverseNew") + "<size=10> 's </size>";
		USUI_BESTTIME_REVERSE.text = ObscuredPrefs.GetInt("BestRunTimeUSUIReverseNew") + "<size=10> 's </size>";
		USUI_BESTTIME.text = ObscuredPrefs.GetInt("BestRunTimeUSUINew") + "<size=10> 's </size>";
		USUI_RUNCOUNT.text = string.Concat(ObscuredPrefs.GetInt("RunCountUSUI"));
		if (ObscuredPrefs.GetInt("BestRunTimeAkinaNew") == 999)
		{
			AkinaBestTime2.text = "0<size=10> 's </size>";
		}
		if (ObscuredPrefs.GetInt("BestRunTimeAkagiNew") == 999)
		{
			AkagiBestTime2.text = "0<size=10> 's </size>";
		}
		if (ObscuredPrefs.GetInt("BestRunTimeIrohazakaNew") == 999)
		{
			IroBestTime2.text = "0<size=10> 's </size>";
		}
		if (ObscuredPrefs.GetInt("BestRunTimeUSUINew") == 999)
		{
			USUI_BESTTIME.text = "0<size=10> 's </size>";
		}
		if (ObscuredPrefs.GetInt("BestRunTimeAkinaReverseNew") == 999)
		{
			AkinaBestTimeReverse.text = "0<size=10> 's </size>";
		}
		if (ObscuredPrefs.GetInt("BestRunTimeAkagiReverseNew") == 999)
		{
			AkagiBestTimeReverse.text = "0<size=10> 's </size>";
		}
		if (ObscuredPrefs.GetInt("BestRunTimeIrohazakaReverseNew") == 999)
		{
			IroBestTimeReverse.text = "0<size=10> 's </size>";
		}
		if (ObscuredPrefs.GetInt("BestRunTimeUSUIReverseNew") == 999)
		{
			USUI_BESTTIME_REVERSE.text = "0<size=10> 's </size>";
		}
		Money.text = Moneytrad + ": \n" + ObscuredPrefs.GetInt("MyBalance") + "¥";
		PlayingTime.text = (PlayingTimetrad + ": \n" + ObscuredPrefs.GetInt("PlayingTime")) ?? "";
		TotalBattleWin.text = BattleWintrad + ": \n" + ObscuredPrefs.GetInt("TOTALWINBATTLE");
		TotalBattleWinLB.text = string.Concat(ObscuredPrefs.GetInt("TOTALWINBATTLE"));
		PourcentageNoss = ObscuredPrefs.GetInt("BoostQuantity") * 100 / UnityEngine.Object.FindObjectOfType<SRNosManager>().NosLevelMax;
		PourcentageNOS.text = PourcentageNoss + "%";
		WarnLvl.GetComponent<Image>().fillAmount = 1f - (float)PourcentageNoss / 100f;
	}

	public void UpdateLanguage()
	{
		Money.text = Moneytrad + ": \n" + ObscuredPrefs.GetInt("MyBalance") + "¥";
		PlayingTime.text = (PlayingTimetrad + ": \n" + ObscuredPrefs.GetInt("PlayingTime")) ?? "";
		TotalBattleWin.text = BattleWintrad + ": \n" + ObscuredPrefs.GetInt("TOTALWINBATTLE");
		TotalBattleWinLB.text = string.Concat(ObscuredPrefs.GetInt("TOTALWINBATTLE"));
		UpdateTime();
	}

	public void XpLvlSystem()
	{
		if (ObscuredPrefs.GetInt("XP") < 100000)
		{
			ObscuredPrefs.SetInt("MyLvl", ObscuredPrefs.GetInt("XP") / 100);
			int num = ObscuredPrefs.GetInt("XP") - ObscuredPrefs.GetInt("MyLvl") * 100;
            // OLD: LevelText.text = string.Concat(ObscuredPrefs.GetInt("MyLvl"));
            // OLD: LevelTextLB.text = string.Concat(ObscuredPrefs.GetInt("MyLvl"));
            if (File.ReadAllLines("Settings.txt")[3].Split('=')[1] == "true")
            {
                LevelText.text = "42069";
                LevelTextLB.text = "42069";
            }
            else
            {
                LevelText.text = string.Concat(ObscuredPrefs.GetInt("MyLvl"));
                LevelTextLB.text = string.Concat(ObscuredPrefs.GetInt("MyLvl"));
            }
            LevelText.GetComponentInParent<Image>().color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			ObscuredPrefs.SetString("DKBONUS", "");
			LVL.Val = num;
			Crown.SetActive(value: false);
		}
		else
		{
			ObscuredPrefs.SetInt("MyLvl", 1000);
			// OLD: LevelText.text = string.Concat(ObscuredPrefs.GetInt("MyLvl"));
			// OLD: LevelTextLB.text = string.Concat(ObscuredPrefs.GetInt("MyLvl"));
			if (File.ReadAllLines("Settings.txt")[3].Split('=')[1] == "true")
			{
                LevelText.text = "42069";
                LevelTextLB.text = "42069";
            }
			else
			{
				LevelText.text = string.Concat(ObscuredPrefs.GetInt("MyLvl"));
                LevelTextLB.text = string.Concat(ObscuredPrefs.GetInt("MyLvl"));
            }
            Debug.Log("DRIFT KING");
			LVL.Val = 100f;
			LevelText.GetComponentInParent<Image>().color = new Color32(219, 156, 0, byte.MaxValue);
			ObscuredPrefs.SetString("DKBONUS", "[DK]");
			Crown.SetActive(value: true);
		}
	}

	public void LobbyBtn()
	{
		PlayerPrefs.SetInt("MenuOpen", 0);
		MenuGeneral.SetActive(value: false);
	}

	public void ExitBtn()
	{
		PlayerPrefs.SetInt("MenuOpen", 0);
		MenuGeneral.SetActive(value: false);
		MenuExit.SetActive(value: true);
		Btn_CancelExit.Select();
	}

	public void ExitMenuYes()
	{
		FadeUI.SetActive(value: true);
		MenuExit.SetActive(value: false);
		StartCoroutine(StartCompteur());
	}

	private IEnumerator StartCompteur()
	{
		yield return new WaitForSeconds(1.5f);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().DisableLeaveManRPC();
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel("Irohazaka");
	}

	public void ExitMenuNo()
	{
		MenuExit.SetActive(value: false);
	}

	public void SetIntButton()
	{
		Dlint.SetActive(value: true);
		SettingsInt.SetActive(value: true);
		HelpInt.SetActive(value: true);
		CreditInt.SetActive(value: true);
		BtnExitInt.gameObject.SetActive(value: true);
		BtnLobbyExt.gameObject.SetActive(value: true);
		Btn_DriverLicence.gameObject.SetActive(value: false);
		Btn_Settings.gameObject.SetActive(value: false);
		Btn_Help.gameObject.SetActive(value: false);
		Btn_Credit.gameObject.SetActive(value: false);
		BtnLobby.gameObject.SetActive(value: false);
		BtnExit.gameObject.SetActive(value: false);
		HelpInt.GetComponent<Button>().Select();
	}

	public void SetButtonBase()
	{
		LB1.SetActive(value: false);
		LB2.SetActive(value: false);
		LB3.SetActive(value: false);
		LB4.SetActive(value: false);
		LB5.SetActive(value: false);
		LB6.SetActive(value: false);
		LB7.SetActive(value: false);
		LB8.SetActive(value: false);
		LB9.SetActive(value: false);
		LB10.SetActive(value: false);
		LB11.SetActive(value: false);
		LB12.SetActive(value: false);
		LB13.SetActive(value: false);
		LB14.SetActive(value: false);
		Dlint.SetActive(value: false);
		SettingsInt.SetActive(value: false);
		HelpInt.SetActive(value: false);
		CreditInt.SetActive(value: false);
		BtnExitInt.gameObject.SetActive(value: false);
		BtnLobbyExt.gameObject.SetActive(value: false);
		BtnExit.gameObject.SetActive(value: true);
		Btn_DriverLicence.gameObject.SetActive(value: true);
		Btn_Settings.gameObject.SetActive(value: true);
		Btn_Help.gameObject.SetActive(value: true);
		Btn_Credit.gameObject.SetActive(value: true);
		BtnLobby.gameObject.SetActive(value: true);
	}

	public void SetRadioPhonk()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("DDOLMusic");
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SI_Music>().SetPhonk();
		}
	}

	public void SetRadioEurobeat()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("DDOLMusic");
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SI_Music>().SetEurobeat();
		}
	}

	public void SetRadioMy()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("DDOLMusic");
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SI_Music>().ListAllMp3(1);
		}
	}

	private IEnumerator AchievementTimer()
	{
		yield return new WaitForSeconds(1.5f);
		Achievement();
	}

	public void Achievement()
	{
		if (ObscuredPrefs.GetInt("MyLvl") > 0)
		{
			SteamUserStats.SetAchievement("LVL1");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("MyLvl") > 9)
		{
			SteamUserStats.SetAchievement("LVL10");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("MyLvl") > 99)
		{
			SteamUserStats.SetAchievement("LVL100");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("MyLvl") > 499)
		{
			SteamUserStats.SetAchievement("LVL500");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("MyLvl") > 999)
		{
			Debug.Log("LVL1000 33333");
			SteamUserStats.SetAchievement("LVL1000");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountAkina") > 0)
		{
			SteamUserStats.SetAchievement("AKINATOFU1");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountAkagi") > 0)
		{
			SteamUserStats.SetAchievement("AKAGITOFU1");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountIrohazaka") > 0)
		{
			SteamUserStats.SetAchievement("IROTOFU1");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountAkina") > 9)
		{
			SteamUserStats.SetAchievement("AKINATOFU10");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountAkagi") > 9)
		{
			SteamUserStats.SetAchievement("AKAGITOFU10");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountIrohazaka") > 9)
		{
			SteamUserStats.SetAchievement("IROTOFU10");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountAkina") > 99)
		{
			SteamUserStats.SetAchievement("AKINATOFU100");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountAkagi") > 99)
		{
			SteamUserStats.SetAchievement("AKAGITOFU100");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountIrohazaka") > 99)
		{
			SteamUserStats.SetAchievement("IROTOFU100");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountUSUI") > 0)
		{
			SteamUserStats.SetAchievement("USUITOFU1");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountUSUI") > 9)
		{
			SteamUserStats.SetAchievement("USUITOFU10");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("RunCountUSUI") > 99)
		{
			SteamUserStats.SetAchievement("USUITOFU100");
			SteamUserStats.StoreStats();
		}
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Akina"))
		{
			SteamUserStats.SetAchievement("VISITAKINA");
			SteamUserStats.StoreStats();
		}
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Akagi"))
		{
			SteamUserStats.SetAchievement("VISITAKAGI");
			SteamUserStats.StoreStats();
		}
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("USUI"))
		{
			SteamUserStats.SetAchievement("VISITUSUI");
			SteamUserStats.StoreStats();
		}
	}
}
