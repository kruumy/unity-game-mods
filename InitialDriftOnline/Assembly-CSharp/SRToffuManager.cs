using System.Collections;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using ZionBandwidthOptimizer.Examples;

public class SRToffuManager : MonoBehaviour
{
	public ObscuredInt RecompenseDeCetteMap;

	public ObscuredInt XpGagnerDansCetteMap;

	[Space]
	public GameObject UIMessage;

	[Space]
	public GameObject TofuIcon;

	[Space]
	public GameObject LogoTarget;

	[Space]
	public GameObject LogoTarget2;

	public ObscuredString MapName;

	public ObscuredString HaveAgoodDrive;

	public ObscuredString InterruptedDelivery;

	public ObscuredString DeliveryCompleted;

	public ObscuredString DeliveryTime;

	private ObscuredString GoMSG;

	private ObscuredInt Compteur;

	private ObscuredInt TempsDeLivraison;

	public Text timeui;

	private ObscuredBool CDD = false;

	private void Start()
	{
		ObscuredCheatingDetector.StartDetection(OnCheaterDetected);
		ObscuredPrefs.SetBool("TOFU RUN", value: false);
		Compteur = 0;
		TofuIcon.SetActive(value: false);
		timeui.text = "";
		timeui.gameObject.SetActive(value: false);
	}

	private void OnCheaterDetected()
	{
		CDD = true;
	}

	private void OnGUI()
	{
		if ((bool)CDD && RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			Debug.Log("Detected");
		}
	}

	private void Update()
	{
		if (ObscuredPrefs.GetBool("TOFU RUN"))
		{
			LogoTarget.SetActive(value: true);
			LogoTarget2.SetActive(value: true);
		}
	}

	public void YesBTN()
	{
		if (Object.FindObjectOfType<RCC_Camera>().playerCar != RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>())
		{
			Object.FindObjectOfType<SRPlayerListRoom>().CaseParPlayer[0].GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().SetMineCam();
		}
		ObscuredPrefs.SetBool("TOFU RUN", value: true);
		StartCoroutine(StartCompteur());
		StartCoroutine(checkcompteur());
		GoMSG = ((string)HaveAgoodDrive) ?? "";
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().TofuDeliveryStart();
		UIMessage.GetComponent<Text>().text = GoMSG;
		UIMessage.GetComponent<Animator>().Play("UIMessage");
		TofuIcon.SetActive(value: true);
	}

	private IEnumerator StartCompteur()
	{
		Compteur = 0;
		int i = 0;
		while (ObscuredPrefs.GetBool("TOFU RUN"))
		{
			yield return new WaitForSeconds(1f);
			Compteur = i;
			Debug.Log("COUNT IN PROGRESS = " + Compteur);
			i++;
		}
	}

	private IEnumerator checkcompteur()
	{
		yield return new WaitForSeconds(10f);
		if ((int)Compteur < 5)
		{
			Debug.Log("RELANCE DU COMPTEUR");
			StartCoroutine(StartCompteur());
		}
	}

	public void StopDelivery()
	{
		Debug.Log("aaaaa");
		if (ObscuredPrefs.GetBool("TOFU RUN"))
		{
			UIMessage.GetComponent<Text>().text = InterruptedDelivery;
			UIMessage.GetComponent<Animator>().Play("UIMessage");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Animator>().Play("CagetteDesappear");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().TofuDeliveryStop();
			Debug.Log("FFFFFFFFFFFF");
		}
		ObscuredPrefs.SetBool("TOFU RUN", value: false);
		timeui.text = "";
		TofuIcon.SetActive(value: false);
		Compteur = 0;
		LogoTarget.SetActive(value: false);
		LogoTarget2.SetActive(value: false);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<BoxCollider>().gameObject.tag = "Player";
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		StopAllCoroutines();
	}

	public void StopDelivery2()
	{
		if (ObscuredPrefs.GetBool("TOFU RUN"))
		{
			UIMessage.GetComponent<Text>().text = InterruptedDelivery;
			UIMessage.GetComponent<Animator>().Play("UIMessage");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Animator>().Play("CagetteDesappear");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().TofuDeliveryStop();
			Debug.Log("FFFFFFFFFFFF");
		}
		ObscuredPrefs.SetBool("TOFU RUN", value: false);
		timeui.text = "";
		TofuIcon.SetActive(value: false);
		Compteur = 0;
		LogoTarget.SetActive(value: false);
		LogoTarget2.SetActive(value: false);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<BoxCollider>().gameObject.tag = "Player";
		StopAllCoroutines();
	}

	public void FinDeLivraison()
	{
		timeui.text = "";
		ObscuredPrefs.SetInt("XP", ObscuredPrefs.GetInt("XP") + (int)XpGagnerDansCetteMap);
		ObscuredPrefs.SetBool("TOFU RUN", value: false);
		TempsDeLivraison = Compteur;
		Debug.Log("TDL = " + Compteur);
		UIMessage.GetComponent<Text>().text = DeliveryCompleted;
		UIMessage.GetComponent<Animator>().Play("UIMessageShort");
		TofuIcon.GetComponent<Animator>().Play("TofuIconFinish");
		LogoTarget.GetComponent<Animator>().Play("End");
		LogoTarget2.GetComponent<Animator>().Play("End");
		if ((int)TempsDeLivraison < ObscuredPrefs.GetInt(string.Concat("BestRunTime", MapName, ObscuredPrefs.GetString("TOFULOCATION"))) || ObscuredPrefs.GetInt(string.Concat("BestRunTime", MapName, ObscuredPrefs.GetString("TOFULOCATION"))) == 0)
		{
			ObscuredPrefs.SetInt(string.Concat("BestRunTime", MapName, ObscuredPrefs.GetString("TOFULOCATION")), TempsDeLivraison);
			ObscuredPrefs.SetInt(string.Concat("UsedCars", MapName, ObscuredPrefs.GetString("TOFULOCATION")), PlayerPrefs.GetInt("DROPDOWNSELECTOR"));
			Debug.Log("USED CARS : " + PlayerPrefs.GetInt("DROPDOWNSELECTOR"));
			Debug.Log("PP : BestRunTime" + (string)MapName + ObscuredPrefs.GetString("TOFULOCATION") + " TIME : " + TempsDeLivraison);
			if (ObscuredPrefs.GetBool("RespawnInTofu"))
			{
				Debug.Log("TU AS RESPAWN CHEAT !");
				ObscuredPrefs.SetInt(string.Concat("CheatingTimeTofu", MapName, ObscuredPrefs.GetString("TOFULOCATION")), Compteur);
				ObscuredPrefs.SetBool("RespawnInTofu", value: false);
			}
		}
		ObscuredPrefs.SetInt("RunCount" + MapName, ObscuredPrefs.GetInt("RunCount" + MapName) + 1);
		ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") + (int)RecompenseDeCetteMap);
		StartCoroutine(AnimTargetLogo());
		ObscuredPrefs.SetInt("TOTALWINMONEY", ObscuredPrefs.GetInt("TOTALWINMONEY") + (int)RecompenseDeCetteMap);
		if (ObscuredPrefs.GetInt("TOTALWINMONEY") > 999)
		{
			SteamUserStats.SetAchievement("EARN1K");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("TOTALWINMONEY") > 4999)
		{
			SteamUserStats.SetAchievement("EARN5K");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("TOTALWINMONEY") > 9999)
		{
			SteamUserStats.SetAchievement("EARN10K");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("TOTALWINMONEY") > 99999)
		{
			SteamUserStats.SetAchievement("EARN100K");
			SteamUserStats.StoreStats();
		}
		Object.FindObjectOfType<CloudDataManager>().SaveData();
	}

	private IEnumerator AnimTargetLogo()
	{
		yield return new WaitForSeconds(3.5f);
		LogoTarget.SetActive(value: false);
		LogoTarget2.SetActive(value: false);
		TofuIcon.SetActive(value: false);
		yield return new WaitForSeconds(0f);
		UIMessage.GetComponent<Text>().text = string.Concat((string)DeliveryTime, " : ", TempsDeLivraison, " 's");
		UIMessage.GetComponent<Animator>().Play("UIMessage");
	}
}
