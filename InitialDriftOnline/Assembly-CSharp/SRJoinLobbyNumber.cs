using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class SRJoinLobbyNumber : MonoBehaviourPunCallbacks
{
	private string LobbyNumber;

	private string RoomName;

	public string Mapname;

	public int CarsNumberInList;

	public SRPhotonM Main;

	private void Start()
	{
		string[] array = base.gameObject.transform.name.Split('e');
		LobbyNumber = array[1];
		RoomName = Mapname + LobbyNumber;
	}

	private void Update()
	{
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		base.OnRoomListUpdate(roomList);
		foreach (RoomInfo room in roomList)
		{
			string[] array = base.gameObject.transform.name.Split('e');
			LobbyNumber = array[1];
			RoomName = Mapname + LobbyNumber;
			if (room.PlayerCount < room.MaxPlayers || room.PlayerCount == 0)
			{
				GetComponent<Button>().interactable = true;
			}
			else if (room.Name == RoomName && room.PlayerCount >= room.MaxPlayers)
			{
				GetComponent<Button>().interactable = false;
			}
			else
			{
				GetComponent<Button>().interactable = true;
			}
		}
	}

	public void JoinLobbyNum()
	{
		RespawnCube[] array = Object.FindObjectsOfType<RespawnCube>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Detection = 0;
		}
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 16;
		PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, null);
	}

	public void SetMiddle()
	{
		Main.SetMiddle(CarsNumberInList);
	}
}
