using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Network/Photon/RCC Photon Scene Manager")]
public class RCC_PhotonManagerLobby : MonoBehaviourPunCallbacks
{
	private InputField playerName;

	private string MapName;

	public GameObject SpawnerAuto;

	private void Start()
	{
	}

	private void ConnectToServer()
	{
		if (!PhotonNetwork.IsConnectedAndReady)
		{
			RCC_SceneManager.Instance.activePlayerCanvas.SetDisplayType(RCC_UIDashboardDisplay.DisplayType.Off);
			PhotonNetwork.ConnectUsingSettings();
		}
		if (PhotonNetwork.IsConnectedAndReady)
		{
			RCC_SceneManager.Instance.activePlayerCanvas.SetDisplayType(RCC_UIDashboardDisplay.DisplayType.Full);
		}
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}

	private void OnGUI()
	{
		if (!PhotonNetwork.IsConnectedAndReady)
		{
			GUI.color = Color.red;
		}
		GUILayout.Label("State: " + PhotonNetwork.NetworkClientState);
		GUI.color = Color.white;
		GUILayout.Label("Total Player Count: " + PhotonNetwork.PlayerList.Length);
		GUILayout.Label("Ping: " + PhotonNetwork.GetPing());
	}

	public override void OnJoinedLobby()
	{
		PhotonNetwork.JoinOrCreateRoom("Lobby", null, null);
	}

	public override void OnJoinRandomFailed(short a, string b)
	{
		MonoBehaviour.print("Joining to random room has failed!, Creating new room...");
		RCC_InfoLabel.Instance.ShowInfo("Joining to random room has failed!, Creating new room...");
	}

	public override void OnJoinedRoom()
	{
		SpawnerAuto.GetComponent<RCC_PhotonDemo>().Spawn();
		SetPlayerName(PlayerPrefs.GetString("PLAYERNAMEE"));
	}

	public void SetPlayerName(string name)
	{
		PhotonNetwork.NickName = name;
		RCC_SceneManager.Instance.activePlayerCanvas.SetDisplayType(RCC_UIDashboardDisplay.DisplayType.Full);
	}
}
