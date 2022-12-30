using System.Collections;
using Photon.Pun;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;
using ZionBandwidthOptimizer.Examples;

public class SRTeleporteur : MonoBehaviour
{
	public GameObject SaveChat;

	public GameObject UIFade;

	public GameObject Radar;

	public GameObject Compteur;

	public GameObject chat;

	public GameObject uim;

	public string Target;

	public int OnlyOne;

	private PlayerIndex playerIndex;

	private GamePadState state;

	private GamePadState prevState;

	private void Start()
	{
		OnlyOne = 0;
		if (PlayerPrefs.GetInt("TutoTP" + Target) == 0 && (bool)GetComponentInChildren<HUDNavigationElement>())
		{
			GetComponentInChildren<HUDNavigationElement>().showIndicator = true;
		}
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider player)
	{
		if (player.gameObject.GetComponentInParent<RCC_PhotonNetwork>().isMine)
		{
			SaveChat.GetComponent<ChatGui>().ChangeMapChat();
			UIFade.SetActive(value: true);
			Compteur.SetActive(value: false);
			Radar.SetActive(value: false);
			chat.SetActive(value: false);
			uim.SetActive(value: false);
			StartCoroutine(FadeWait(player.gameObject));
			player.gameObject.GetComponentInParent<SRPlayerCollider>().DisableLeaveManRPC();
			GamePad.SetVibration(playerIndex, 0f, 0f);
		}
		if (PlayerPrefs.GetInt("TutoTP" + Target) == 0 && player.GetComponentInParent<RCC_PhotonNetwork>().isMine && (bool)GetComponentInChildren<HUDNavigationElement>())
		{
			PlayerPrefs.SetInt("TutoTP" + Target, 10);
			GetComponentInChildren<HUDNavigationElement>().showIndicator = false;
		}
	}

	private IEnumerator FadeWait(GameObject Player)
	{
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Akina"))
		{
			PlayerPrefs.SetString("WhereYouFrom", "AKINA");
			Player.GetComponentInParent<Rigidbody>().velocity = base.transform.forward * 5f;
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Irohazaka") && Target == "Akina")
		{
			Player.GetComponentInParent<Rigidbody>().velocity = base.transform.right * 5f;
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Irohazaka") && Target == "Akagi")
		{
			Player.GetComponentInParent<Rigidbody>().velocity = base.transform.right * -5f;
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Irohazaka") && Target == "USUI")
		{
			Player.GetComponentInParent<Rigidbody>().velocity = base.transform.right * 5f;
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Akagi"))
		{
			PlayerPrefs.SetString("WhereYouFrom", "AKAGI");
			Player.GetComponentInParent<Rigidbody>().velocity = base.transform.right * 5f;
		}
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("USUI"))
		{
			PlayerPrefs.SetString("WhereYouFrom", "USUI");
			Player.GetComponentInParent<Rigidbody>().velocity = base.transform.right * -5f;
		}
		yield return new WaitForSeconds(1f);
		MapTP();
	}

	private void MapTP()
	{
		if (Target == "Akina" && OnlyOne == 0)
		{
			OnlyOne = 1;
			GamePad.SetVibration(playerIndex, 0f, 0f);
			PlayerPrefs.SetInt("IcomeFromOtherMap", 50);
			PlayerPrefs.SetInt("WAITTPLOBBYBTN", 0);
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.LoadLevel("Akina");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.SetActive(value: false);
		}
		else if (Target == "Irohazaka" && OnlyOne == 0)
		{
			OnlyOne = 1;
			GamePad.SetVibration(playerIndex, 0f, 0f);
			PlayerPrefs.SetInt("IcomeFromOtherMap", 50);
			PlayerPrefs.SetInt("WAITTPLOBBYBTN", 0);
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.LoadLevel("Irohazaka");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.SetActive(value: false);
		}
		else if (Target == "Akagi" && OnlyOne == 0)
		{
			OnlyOne = 1;
			GamePad.SetVibration(playerIndex, 0f, 0f);
			PlayerPrefs.SetInt("IcomeFromOtherMap", 50);
			PlayerPrefs.SetInt("WAITTPLOBBYBTN", 0);
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.LoadLevel("Akagi");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.SetActive(value: false);
		}
		else if (Target == "USUI" && OnlyOne == 0)
		{
			OnlyOne = 1;
			GamePad.SetVibration(playerIndex, 0f, 0f);
			PlayerPrefs.SetInt("IcomeFromOtherMap", 50);
			PlayerPrefs.SetInt("WAITTPLOBBYBTN", 0);
			PhotonNetwork.LeaveRoom();
			PhotonNetwork.LoadLevel("USUI");
			RCC_SceneManager.Instance.activePlayerVehicle.gameObject.SetActive(value: false);
		}
	}
}
