using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public class SteamworksSteamIDEvent : UnityEvent<CSteamID>
{
}
