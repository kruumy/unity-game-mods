using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public class UnityLobbyMatchListEvent : UnityEvent<LobbyMatchList_t>
{
}
