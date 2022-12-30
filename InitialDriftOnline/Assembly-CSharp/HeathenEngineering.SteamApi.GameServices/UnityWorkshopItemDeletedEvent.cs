using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopItemDeletedEvent : UnityEvent<DeleteItemResult_t>
{
}
