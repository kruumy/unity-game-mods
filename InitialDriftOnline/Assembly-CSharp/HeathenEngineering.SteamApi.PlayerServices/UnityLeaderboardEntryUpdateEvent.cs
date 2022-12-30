using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class UnityLeaderboardEntryUpdateEvent : UnityEvent<LeaderboardEntry_t>
{
}
