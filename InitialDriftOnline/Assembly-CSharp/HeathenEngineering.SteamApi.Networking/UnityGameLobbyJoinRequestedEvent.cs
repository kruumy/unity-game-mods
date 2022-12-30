using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public class UnityGameLobbyJoinRequestedEvent : UnityEvent<GameLobbyJoinRequested_t>
{
}
