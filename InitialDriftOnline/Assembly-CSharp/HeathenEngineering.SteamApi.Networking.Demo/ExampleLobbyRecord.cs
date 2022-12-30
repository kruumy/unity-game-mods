using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.Networking.Demo;

public class ExampleLobbyRecord : LobbyRecordBehvaiour
{
	public SteamworksLobbySettings LobbySettings;

	public Text lobbyId;

	public Text lobbySize;

	public Button connectButton;

	public Text buttonLabel;

	[Header("List Record")]
	public LobbyHunterLobbyRecord record;

	public override void SetLobby(LobbyHunterLobbyRecord record, SteamworksLobbySettings lobbySettings)
	{
		Debug.Log("Setting lobby data for " + record.lobbyId);
		LobbySettings = lobbySettings;
		this.record = record;
		lobbyId.text = (string.IsNullOrEmpty(record.name) ? "<unknown>" : record.name);
		if (record.metadata.ContainsKey("gamemode"))
		{
			lobbySize.text = record.maxSlots.ToString();
		}
	}

	public void Selected()
	{
		OnSelected.Invoke(record.lobbyId);
	}

	private void Update()
	{
		if (record.lobbyId != CSteamID.Nil && LobbySettings.lobbies[0].id.m_SteamID == record.lobbyId.m_SteamID)
		{
			connectButton.interactable = false;
			buttonLabel.text = "You are here!";
		}
		else
		{
			connectButton.interactable = true;
			buttonLabel.text = "Join lobby!";
		}
	}
}
