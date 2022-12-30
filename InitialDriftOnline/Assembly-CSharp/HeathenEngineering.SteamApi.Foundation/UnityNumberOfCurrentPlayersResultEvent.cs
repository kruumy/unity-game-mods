using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
public class UnityNumberOfCurrentPlayersResultEvent : UnityEvent<NumberOfCurrentPlayers_t>
{
}
