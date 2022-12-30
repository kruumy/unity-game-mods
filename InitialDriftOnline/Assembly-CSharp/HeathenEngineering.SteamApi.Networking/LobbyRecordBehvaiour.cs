using HeathenEngineering.SteamApi.Foundation;
using UnityEngine;

namespace HeathenEngineering.SteamApi.Networking;

public class LobbyRecordBehvaiour : MonoBehaviour
{
	public UnitySteamIdEvent OnSelected;

	public virtual void SetLobby(LobbyHunterLobbyRecord record, SteamworksLobbySettings lobbySettings)
	{
	}
}
