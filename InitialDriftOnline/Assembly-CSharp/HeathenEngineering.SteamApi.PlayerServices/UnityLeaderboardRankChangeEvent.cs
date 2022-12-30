using System;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class UnityLeaderboardRankChangeEvent : UnityEvent<LeaderboardRankChangeData>
{
}
