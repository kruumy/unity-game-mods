using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopGetUserItemVoteResultEvent : UnityEvent<GetUserItemVoteResult_t>
{
}
