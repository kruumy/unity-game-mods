using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public class UnityItemDetailEvent : UnityEvent<bool, SteamItemDetails_t[]>
{
}
