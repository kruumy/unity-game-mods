using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopSetUserItemVoteResultEvent : UnityEvent<SetUserItemVoteResult_t>
{
}
