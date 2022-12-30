using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public class UnityLobbyGameCreatedEvent : UnityEvent<LobbyGameCreated_t>
{
}
