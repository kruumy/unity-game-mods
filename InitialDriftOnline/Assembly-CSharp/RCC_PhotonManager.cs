using System.Collections;
using CodeStage.AntiCheat.Storage;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using ZionBandwidthOptimizer.Examples;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Network/Photon/RCC Photon Scene Manager")]
public class RCC_PhotonManager : MonoBehaviourPunCallbacks
{
	public InputField playerName;

	public string MapName;

	public GameObject SpawnerAuto;

	public GameObject CanvasRoom;

	public GameObject Radar;

	public GameObject TopCam;

	private void Start()
	{
		if (!PhotonNetwork.IsConnected)
		{
			PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = PlayerPrefs.GetString("SelectedRegion");
			PhotonNetwork.ConnectUsingSettings();
			OnConnectedToMaster();
		}
	}

	private void ConnectToServer()
	{
		RCC_InfoLabel.Instance.ShowInfo("Connecting to photon server");
		if (!PhotonNetwork.IsConnectedAndReady)
		{
			playerName.gameObject.SetActive(value: false);
			RCC_SceneManager.Instance.activePlayerCanvas.SetDisplayType(RCC_UIDashboardDisplay.DisplayType.Off);
			PhotonNetwork.ConnectUsingSettings();
		}
		if (PhotonNetwork.IsConnectedAndReady)
		{
			playerName.gameObject.SetActive(value: false);
			RCC_SceneManager.Instance.activePlayerCanvas.SetDisplayType(RCC_UIDashboardDisplay.DisplayType.Full);
		}
	}

	public override void OnConnectedToMaster()
	{
		if (!PhotonNetwork.InLobby)
		{
			PhotonNetwork.JoinLobby();
		}
	}

	private void OnGUI()
	{
		if (!PhotonNetwork.IsConnectedAndReady)
		{
			GUI.color = Color.red;
		}
	}

	public override void OnJoinedLobby()
	{
		CanvasRoom.SetActive(value: true);
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		base.OnDisconnected(cause);
		PhotonNetwork.Reconnect();
	}

	public override void OnJoinRandomFailed(short a, string b)
	{
		RCC_InfoLabel.Instance.ShowInfo("Joining to random room has failed!, Creating new room...");
		PhotonNetwork.CreateRoom(null);
	}

	public override void OnJoinedRoom()
	{
		CanvasRoom.SetActive(value: false);
		SpawnerAuto.SetActive(value: true);
		StartCoroutine(radaron());
		PlayerPrefs.SetInt("UniStormLoad", 1);
		SpawnerAuto.GetComponent<RCC_PhotonDemo>().LauncherSpawn();
		SetPlayerName(PlayerPrefs.GetString("PLAYERNAMEE"));
	}

	private IEnumerator radaron()
	{
		yield return new WaitForSeconds(0.2f);
		Radar.SetActive(value: true);
	}

	public void SetPlayerName(string name)
	{
		PhotonNetwork.NickName = name;
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		Object.FindObjectOfType<RCC_PhotonDemo>().NickHead();
		Object.FindObjectOfType<RCC_PhotonDemo>().SkinDefined(ObscuredPrefs.GetInt("SkinNumberInUse"));
		StartCoroutine(radaron2(newPlayer));
		Object.FindObjectOfType<SRAdminTools>().Sendd();
		Object.FindObjectOfType<Becquet>().SetAfterSpawn();
		Object.FindObjectOfType<SkinManager>().SendMySkinColorToOther();
	}

	private IEnumerator radaron2(Player newPlayer)
	{
		yield return new WaitForSeconds(1f);
		Object.FindObjectOfType<Becquet>().SetAfterSpawn();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_PhotonNetwork>().Sendinfo0();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_PhotonNetwork>().Sendinfo();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_PhotonNetwork>().Sendinfo2();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_PhotonNetwork>().Sendinfo3();
		RCC_SceneManager.Instance.activePlayerVehicle.gameObject.GetComponent<RCC_PhotonNetwork>().Sendinfo4();
	}
}
