using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.Networking.Demo;

public class FindMatchButton : MonoBehaviour
{
	[FormerlySerializedAs("SteamSettings")]
	public SteamSettings steamSettings;

	[FormerlySerializedAs("LobbySettings")]
	public SteamworksLobbySettings lobbySettings;

	[FormerlySerializedAs("QuickMatchFilter")]
	public LobbyHunterFilter quickMatchFilter;

	public Button quickMatchButton;

	public Text quickMatchLabel;

	private void Update()
	{
		quickMatchButton.interactable = !lobbySettings.Manager.IsSearching && !lobbySettings.Manager.IsQuickSearching;
		if (quickMatchButton.interactable)
		{
			if (!lobbySettings.InLobby)
			{
				quickMatchLabel.text = "Quick Match";
			}
			else
			{
				quickMatchLabel.text = "Leave Lobby";
			}
		}
		else
		{
			quickMatchLabel.text = "Searching";
		}
	}

	public void SimpleFindMatch()
	{
		if (!lobbySettings.InLobby)
		{
			Debug.Log("[FindMatchButton.SimpleFindMatch] Startomg a quickmatch search for a lobby that matches the filter defined in [FindMatchButton.quickMatchFilter].");
			lobbySettings.Manager.QuickMatch(quickMatchFilter, steamSettings.client.userData.DisplayName, autoCreate: true);
		}
		else
		{
			lobbySettings.LeaveAllLobbies();
		}
	}

	public void CreateMatch()
	{
		Debug.Log("[FindMatchButton.CreateMatch] Quick match found 0 matches, creating a new lobby with 4 slots.");
		lobbySettings.CreateLobby(ELobbyType.k_ELobbyTypePublic, 4);
	}

	public void GetHelp()
	{
		Application.OpenURL("https://partner.steamgames.com/doc/features/multiplayer/matchmaking");
	}

	public void KickMember(string id)
	{
		lobbySettings.KickMember(new CSteamID(ulong.Parse(id)));
	}

	public void OnEnterLobby(SteamLobby lobby)
	{
		lobby.Name = steamSettings.client.userData.DisplayName + "'s Lobby";
		Debug.Log("Entered lobby: " + lobby.Name);
	}

	public void OnExitLobby(SteamLobby lobby)
	{
		Debug.Log("Exiting lobby: " + lobby.Name);
	}
}
