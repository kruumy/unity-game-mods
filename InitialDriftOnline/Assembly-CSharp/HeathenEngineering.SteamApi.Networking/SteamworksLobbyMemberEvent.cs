using System;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public class SteamworksLobbyMemberEvent : UnityEvent<SteamworksLobbyMember>
{
}
