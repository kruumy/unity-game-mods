using System.Collections;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using SickscoreGames.HUDNavigationSystem;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZionBandwidthOptimizer.Examples;

public class RaceManager : MonoBehaviour
{
	public ObscuredInt WinningTuneDansCetteMap;

	public ObscuredInt XpAGagnerDansCetteMap;

	[Space]
	public GameObject UIAskToPlayer;

	[Space]
	public GameObject UIMessage;

	public Sprite ManetteBtn;

	public Sprite KeyboardBtn;

	private ObscuredInt TuPeuxAsk;

	private ObscuredInt TempoASK;

	private ObscuredInt TuPeuxAccept;

	public KeyCode FightTouch;

	private ObscuredString CibleName;

	private ObscuredString CiblePhotonName;

	public GameObject RaceNotification;

	public GameObject ToffuManager;

	public GameObject RunningLogo;

	public GameObject CocheInvitationSent;

	public GameObject MenuUI;

	private ObscuredString LeMec;

	private ObscuredString LeMecPhotonName;

	public Transform StartingPointP1;

	public Transform StartingPointP2;

	[Space]
	public GameObject StartingCount;

	public AudioClip CountClick;

	private GameObject Enemyy;

	[Space]
	public GameObject RaceNotifBtnYes;

	[Space]
	public GameObject RaceNotifBtnNo;

	private ObscuredInt OtherLeaveWithAltF4;

	private ObscuredInt BattleBtnAnswer;

	private ObscuredInt mononcleone;

	[Space]
	public ObscuredString ImpossibleNow;

	public ObscuredString ForRaceAgainst;

	public ObscuredString InvitationSentTo;

	public ObscuredString ChallengeYou;

	public ObscuredString BattleImpossibleNow;

	public ObscuredString BattleInterrupted;

	public ObscuredString YouWinThisBattle;

	public ObscuredString YouLooseTheBattle;

	public ObscuredString AsLeftTheBattleFinishFor;

	[Space]
	public TextMeshPro TxtCompteur;

	public GameObject AnimCompteur;

	private ObscuredBool CDD = false;

	private void Start()
	{
		ObscuredCheatingDetector.StartDetection(OnCheaterDetected);
		PlayerPrefs.SetInt("ImInRun", 0);
		PlayerPrefs.SetString("RaceEnemyBlaze", "");
		TuPeuxAsk = 1;
		TempoASK = 0;
		UIAskToPlayer.SetActive(value: false);
		RaceNotification.SetActive(value: false);
		RunningLogo.SetActive(value: false);
		StartingCount.SetActive(value: false);
		RaceNotifBtnYes.SetActive(value: true);
		RaceNotifBtnNo.SetActive(value: true);
		OtherLeaveWithAltF4 = 0;
		BattleBtnAnswer = 0;
		TuPeuxAccept = 0;
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
		if (Input.GetAxis("Xbox_DPadVertical") == 1f || Input.GetKeyDown(FightTouch))
		{
			GameObject.Find(CiblePhotonName);
			if ((int)TuPeuxAsk == 1 && (int)TempoASK == 0 && !ObscuredPrefs.GetBool("TOFU RUN") && PlayerPrefs.GetInt("ImInRun") == 0)
			{
				TempoASK = 1;
				TuPeuxAsk = 0;
				StartCoroutine(TempoASKC());
				SendRaceInvitation();
				Debug.Log(string.Concat("DEMANDE DE COMBAT ENVOYER A ", CibleName, " / ", CiblePhotonName));
				RaceNotifBtnYes.SetActive(value: true);
				RaceNotifBtnNo.SetActive(value: true);
			}
			else if (ObscuredPrefs.GetBool("TOFU RUN") || PlayerPrefs.GetInt("ImInRun") != 0)
			{
				UIAskToPlayer.GetComponent<Text>().text = ImpossibleNow;
				StartCoroutine(CloseMsg());
				TuPeuxAsk = 0;
				TempoASK = 0;
			}
		}
		if (PlayerPrefs.GetInt("ImInRun") == 1 && !Enemyy && (int)OtherLeaveWithAltF4 == 0)
		{
			OtherLeaveWithAltF4 = 1;
			Debug.Log("LAUTRE A QUITTER CHEEEEFFFFFFFF");
			OtherLeaveRun();
		}
		if ((int)BattleBtnAnswer == 1)
		{
			if (RaceNotification.activeSelf && Input.GetKeyDown(KeyCode.Joystick1Button0) && (int)TuPeuxAccept == 1 && RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<BoxCollider>().gameObject.tag == "Player")
			{
				AcceptRace();
			}
			else if (Input.GetKeyDown(KeyCode.Joystick1Button1))
			{
				DenyRace();
			}
		}
	}

	private IEnumerator TempoASKC()
	{
		yield return new WaitForSeconds(5f);
		TempoASK = 0;
	}

	public void AskToPlayer(string EnemyPhoton, string EnemyUI)
	{
		CibleName = EnemyUI;
		CiblePhotonName = EnemyPhoton;
		UIAskToPlayer.SetActive(value: true);
		UIAskToPlayer.GetComponent<Animator>().Play("UATPappear");
		UIAskToPlayer.GetComponent<Text>().text = string.Concat("<color=#fffff>", ForRaceAgainst, EnemyUI, "</color>");
		if (PlayerPrefs.GetString("ControllerTypeChoose") == "Xbox360One")
		{
			UIAskToPlayer.GetComponentInChildren<Image>().sprite = ManetteBtn;
		}
		else
		{
			UIAskToPlayer.GetComponentInChildren<Image>().sprite = KeyboardBtn;
		}
		TuPeuxAsk = 1;
	}

	public void StopAskInfo()
	{
		UIAskToPlayer.GetComponent<Animator>().Play("UATPDesappear");
		StartCoroutine(CloseUATP());
		RaceNotification.GetComponent<Animator>().Play("Desappear");
		StartCoroutine(CloseNotif1sec());
		TuPeuxAsk = 0;
		TuPeuxAccept = 0;
	}

	public void SendRaceInvitation()
	{
		CocheInvitationSent.SetActive(value: true);
		UIAskToPlayer.GetComponent<Text>().text = string.Concat(InvitationSentTo, CibleName);
		StartCoroutine(CloseMsg());
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendRaceInvitation(CiblePhotonName, PlayerPrefs.GetString("PLAYERNAMEE"));
	}

	private IEnumerator CloseMsg()
	{
		yield return new WaitForSeconds(1f);
		UIAskToPlayer.GetComponent<Animator>().Play("UATPDesappear");
		StartCoroutine(CloseUATP());
	}

	public void ShowMyInvitation(string Sender, string EnvoyeurDelaDemande)
	{
		if (PlayerPrefs.GetInt("ImInRun") == 0 && !ObscuredPrefs.GetBool("TOFU RUN"))
		{
			RaceNotifBtnYes.SetActive(value: true);
			RaceNotifBtnNo.SetActive(value: true);
			LeMec = Sender;
			LeMecPhotonName = EnvoyeurDelaDemande;
			TuPeuxAsk = 0;
			UIAskToPlayer.GetComponent<Animator>().Play("UATPDesappear");
			StartCoroutine(CloseUATP());
			RaceNotification.SetActive(value: false);
			RaceNotification.SetActive(value: true);
			RaceNotification.GetComponentInChildren<Text>().text = "<color=#B95353>" + Sender + "</color> \n" + ChallengeYou;
			BattleBtnAnswer = 1;
			TuPeuxAccept = 1;
		}
	}

	private IEnumerator CloseUATP()
	{
		yield return new WaitForSeconds(1f);
		CocheInvitationSent.SetActive(value: false);
		UIAskToPlayer.SetActive(value: false);
	}

	public void AcceptRace()
	{
		if (RaceNotification.activeSelf && PlayerPrefs.GetInt("ImInRun") == 0 && !ObscuredPrefs.GetBool("TOFU RUN") && RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<BoxCollider>().gameObject.tag == "Player")
		{
			if (Object.FindObjectOfType<RCC_Camera>().playerCar != RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>())
			{
				Object.FindObjectOfType<SRPlayerListRoom>().CaseParPlayer[0].GetComponentInChildren<IDHome>().transform.gameObject.GetComponentInChildren<SRCheckOtherPlayerCam>().SetMineCam();
			}
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;
			BattleBtnAnswer = 0;
			RaceNotification.GetComponent<Animator>().Play("Desappear");
			StartCoroutine(CloseNotif1sec());
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().SendRunAcceptationRPC(LeMecPhotonName);
			ToffuManager.GetComponent<SRToffuManager>().StopDelivery();
			LetsGoRaceP2();
		}
		else
		{
			BattleBtnAnswer = 0;
			UIMessage.GetComponent<Text>().text = BattleImpossibleNow;
			UIMessage.GetComponent<Animator>().Play("UIMessageShort");
			RaceNotification.GetComponent<Animator>().Play("Desappear");
			StartCoroutine(CloseNotif1sec());
		}
	}

	public void DenyRace()
	{
		BattleBtnAnswer = 0;
		RaceNotification.GetComponent<Animator>().Play("Desappear");
		StartCoroutine(CloseNotif1sec());
	}

	private IEnumerator CloseNotif1sec()
	{
		yield return new WaitForSeconds(1f);
		RaceNotification.SetActive(value: false);
	}

	public void LetsGoRaceP1()
	{
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;
		OtherLeaveWithAltF4 = 0;
		Enemyy = GameObject.Find(CiblePhotonName);
		Enemyy.GetComponent<HUDNavigationElement>().showIndicator = true;
		PlayerPrefs.SetString("RaceEnemyBlaze", "");
		PlayerPrefs.SetString("RaceEnemyBlaze", Enemyy.name);
		Debug.Log("LANCEMENT DE LA COURSE P1 / Adversaire : " + Enemyy.name);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 1000f;
		StartCoroutine(DecompteRunningP1());
		StartCoroutine(ReposP1());
	}

	private IEnumerator ReposP1()
	{
		yield return new WaitForSeconds(2f);
		PlayerPrefs.SetInt("ImInRun", 1);
		_ = Vector3.zero;
		_ = Quaternion.identity;
		Vector3 position = StartingPointP1.position;
		Quaternion rotation = StartingPointP1.rotation;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position = position;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.rotation = rotation;
	}

	private IEnumerator DecompteRunningP1()
	{
		AnimCompteur.GetComponent<Animator>().Play("3DTime");
		yield return new WaitForSeconds(1f);
		_ = Vector3.zero;
		_ = Quaternion.identity;
		Vector3 position = StartingPointP1.position;
		Quaternion rotation = StartingPointP1.rotation;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position = position;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.rotation = rotation;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;
		StartCoroutine(DecompteRunningIE());
		StartingCount.SetActive(value: true);
	}

	public void LetsGoRaceP2()
	{
		OtherLeaveWithAltF4 = 0;
		Enemyy = GameObject.Find(LeMecPhotonName);
		Enemyy.GetComponent<HUDNavigationElement>().showIndicator = true;
		PlayerPrefs.SetString("RaceEnemyBlaze", "");
		PlayerPrefs.SetString("RaceEnemyBlaze", Enemyy.name);
		Debug.Log("LANCEMENT DE LA COURSE P2 / Adversaire : " + Enemyy.name);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 1000f;
		StartCoroutine(DecompteRunningP2());
		StartCoroutine(ReposP2());
	}

	private IEnumerator DecompteRunningP2()
	{
		AnimCompteur.GetComponent<Animator>().Play("3DTime");
		yield return new WaitForSeconds(1f);
		PlayerPrefs.SetInt("ImInRun", 1);
		_ = Vector3.zero;
		_ = Quaternion.identity;
		Vector3 position = StartingPointP2.position;
		Quaternion rotation = StartingPointP2.rotation;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position = position;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.rotation = rotation;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;
		StartCoroutine(DecompteRunningIE());
		StartingCount.SetActive(value: true);
	}

	private IEnumerator ReposP2()
	{
		yield return new WaitForSeconds(2f);
		_ = Vector3.zero;
		_ = Quaternion.identity;
		Vector3 position = StartingPointP2.position;
		Quaternion rotation = StartingPointP2.rotation;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.position = position;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.transform.rotation = rotation;
	}

	private IEnumerator DecompteRunningIE()
	{
		mononcleone = 0;
		MenuUI.SetActive(value: false);
		TxtCompteur.text = "5";
		StartingCount.GetComponentInChildren<Text>().text = "";
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().speed = 0f;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().engineRPM = 0f;
		GetComponent<AudioSource>().PlayOneShot(CountClick);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 1000f;
		yield return new WaitForSeconds(1f);
		StartingCount.GetComponentInChildren<Text>().text = "";
		TxtCompteur.text = "4";
		GetComponent<AudioSource>().PlayOneShot(CountClick);
		yield return new WaitForSeconds(1f);
		StartingCount.GetComponentInChildren<Text>().text = "";
		TxtCompteur.text = "3";
		GetComponent<AudioSource>().PlayOneShot(CountClick);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().speed = 0f;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().engineRPM = 0f;
		yield return new WaitForSeconds(1f);
		StartingCount.GetComponentInChildren<Text>().text = "";
		TxtCompteur.text = "2";
		GetComponent<AudioSource>().PlayOneShot(CountClick);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().speed = 0f;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().engineRPM = 0f;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().currentGear = 0;
		yield return new WaitForSeconds(1f);
		MenuUI.SetActive(value: false);
		StartingCount.GetComponentInChildren<Text>().text = "";
		TxtCompteur.text = "1";
		GetComponent<AudioSource>().PlayOneShot(CountClick);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 1000f;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInParent<SRPlayerCollider>().AppelRPCSetGhostModeV2(10);
		yield return new WaitForSeconds(1f);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().enabled = true;
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_CarControllerV3>().currentGear = 0;
		StartingCount.GetComponentInChildren<Text>().text = "";
		TxtCompteur.text = "GO";
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<Rigidbody>().drag = 0.01f;
		RunningLogo.SetActive(value: true);
		yield return new WaitForSeconds(1f);
		StartingCount.SetActive(value: false);
	}

	public void StopRun()
	{
		if (PlayerPrefs.GetInt("ImInRun") == 1)
		{
			OtherLeaveWithAltF4 = 0;
			if ((bool)Enemyy)
			{
				Enemyy.GetComponent<HUDNavigationElement>().showIndicator = false;
			}
			if ((bool)Enemyy)
			{
				RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().JeStopLaCourse(Enemyy.gameObject.name);
			}
			UIMessage.GetComponent<Text>().text = BattleInterrupted;
			UIMessage.GetComponent<Animator>().Play("UIMessageShort");
		}
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		PlayerPrefs.SetInt("ImInRun", 0);
		RunningLogo.SetActive(value: false);
		StartingCount.SetActive(value: false);
		if ((bool)Enemyy)
		{
			Enemyy.GetComponent<HUDNavigationElement>().showIndicator = false;
		}
	}

	public void OtherLeaveRun()
	{
		if ((bool)Enemyy)
		{
			Enemyy.GetComponent<HUDNavigationElement>().showIndicator = false;
		}
		OtherLeaveWithAltF4 = 1;
		if (PlayerPrefs.GetInt("ImInRun") == 1 && (int)OtherLeaveWithAltF4 == 1 && (int)mononcleone == 0)
		{
			mononcleone = 10;
			OtherLeaveWithAltF4 = 0;
			RaceNotification.GetComponent<Animator>().Play("Appear");
			RaceNotifBtnYes.SetActive(value: false);
			RaceNotifBtnNo.SetActive(value: false);
			RaceNotification.SetActive(value: false);
			RaceNotification.SetActive(value: true);
			RaceNotification.GetComponent<Animator>().Play("Appear");
			RaceNotification.GetComponentInChildren<Text>().text = string.Concat("<color=#B95353>", LeMec, "</color> ", AsLeftTheBattleFinishFor);
			StartCoroutine(Deborde());
		}
	}

	private IEnumerator Deborde()
	{
		yield return new WaitForSeconds(3f);
		StartCoroutine(CloseNotifAutomatique());
	}

	private IEnumerator CloseNotifAutomatique()
	{
		yield return new WaitForSeconds(3f);
		RaceNotification.GetComponent<Animator>().Play("Desappear");
		yield return new WaitForSeconds(1f);
		RaceNotification.SetActive(value: false);
		RaceNotifBtnYes.SetActive(value: true);
		RaceNotifBtnNo.SetActive(value: true);
	}

	private IEnumerator CloseNotif()
	{
		yield return new WaitForSeconds(2f);
		RaceNotification.SetActive(value: false);
		RunningLogo.SetActive(value: false);
	}

	public void FinishFirst()
	{
		RaceNotification.SetActive(value: false);
		RunningLogo.SetActive(value: false);
		Debug.Log("FINISH FIRST");
		PlayerPrefs.SetInt("ImInRun", 0);
		if ((bool)Enemyy)
		{
			Enemyy.GetComponent<HUDNavigationElement>().showIndicator = false;
		}
		ObscuredPrefs.SetInt("MyBalance", ObscuredPrefs.GetInt("MyBalance") + (int)WinningTuneDansCetteMap);
		ObscuredPrefs.SetInt("XP", ObscuredPrefs.GetInt("XP") + (int)XpAGagnerDansCetteMap);
		OtherLeaveWithAltF4 = 0;
		UIMessage.GetComponent<Text>().text = string.Concat((string)YouWinThisBattle, " ! (", WinningTuneDansCetteMap, "¥)");
		UIMessage.GetComponent<Animator>().Play("UIMessage");
		PlayerPrefs.SetInt("ImInRun", 0);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<BoxCollider>().tag = "Player";
		ObscuredPrefs.SetInt("TOTALWINBATTLE", ObscuredPrefs.GetInt("TOTALWINBATTLE") + 1);
		Debug.Log("FINISH FIRST 2");
		if (ObscuredPrefs.GetInt("TOTALWINBATTLE") > 0)
		{
			SteamUserStats.SetAchievement("1RACEWIN");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("TOTALWINBATTLE") > 9)
		{
			SteamUserStats.SetAchievement("10RACEWIN");
			SteamUserStats.StoreStats();
		}
		if (ObscuredPrefs.GetInt("TOTALWINBATTLE") > 99)
		{
			SteamUserStats.SetAchievement("100RACEWIN");
			SteamUserStats.StoreStats();
		}
		ObscuredPrefs.SetInt("TOTALWINMONEY", ObscuredPrefs.GetInt("TOTALWINMONEY") + (int)WinningTuneDansCetteMap);
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
		RaceNotification.SetActive(value: false);
		RunningLogo.SetActive(value: false);
		Object.FindObjectOfType<CloudDataManager>().SaveData();
	}

	public void FinishSecond()
	{
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<SRPlayerCollider>().AppelRPCSetGhostModeV2(8);
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponentInChildren<BoxCollider>().tag = "Player";
		PlayerPrefs.SetInt("ImInRun", 0);
		Enemyy.GetComponent<HUDNavigationElement>().showIndicator = false;
		ObscuredPrefs.SetInt("XP", ObscuredPrefs.GetInt("XP") + (int)XpAGagnerDansCetteMap / 3);
		OtherLeaveWithAltF4 = 0;
		UIMessage.GetComponent<Text>().text = YouLooseTheBattle;
		UIMessage.GetComponent<Animator>().Play("UIMessage");
		PlayerPrefs.SetInt("ImInRun", 0);
		RaceNotification.SetActive(value: false);
		RunningLogo.SetActive(value: false);
		Debug.Log("FINISH 2ND !!!!!!!!");
		Object.FindObjectOfType<CloudDataManager>().SaveData();
	}
}
